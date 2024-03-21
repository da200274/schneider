using WebAppMongo.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace WebAppMongo.Services;

public class UserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(
        IOptions<SchneiderDatabaseSettings> schneiderDatabaseSettings)
    {
        var mongoClient = new MongoClient(schneiderDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(schneiderDatabaseSettings.Value.DatabaseName);

        _users = mongoDatabase.GetCollection<User>(
            schneiderDatabaseSettings.Value.UserCollectionName);

    }

    public async Task<List<User>> GetAsync()=>
        await _users.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(string id) =>
        await _users.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync (User newUser) =>
        await _users.InsertOneAsync(newUser);

    public async Task UpdateAsync(string id, User updateUser) =>
        await _users.ReplaceOneAsync(x => x.Id == id, updateUser);

    public async Task RemoveAsync(string id) =>
        await _users.DeleteOneAsync(x => x.Id == id);
}
