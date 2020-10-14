using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestNikita.Models
{
  public class DataService
  {
    IMongoCollection<Transfer> Transfers;

    IMongoCollection<User> Users;

    public DataService()
    {
      CreateConnectionToCluster("Users");
      CreateConnectionToCluster("Transfers");
    }

    private void CreateConnectionToCluster(string dbName)
    {
      string connectionString = $"mongodb+srv://nikita:nikita@cluster0.xczbr.mongodb.net/{dbName}?retryWrites=true&w=majority";

      var connectionToCluster = new MongoUrlBuilder(connectionString);

      MongoClient clientCluster = new MongoClient(connectionString);

      IMongoDatabase database = clientCluster.GetDatabase(connectionToCluster.DatabaseName);

      if (dbName == "Transfers")
      {
        Transfers = database.GetCollection<Transfer>("Transfers");
      }

      if (dbName == "Users")
      {
        Users = database.GetCollection<User>("Users");
      }
    }

    public async Task<string> CreateUser(CreateUser userForCreate)
    {
      User existUser = await Users.Find(user => user.Name == userForCreate.Name).FirstOrDefaultAsync();

      if (existUser == null)
      {
        User newUser = new User
        {
          Name = userForCreate.Name,
          Password = userForCreate.Password,
          Role = "user"
        };

        await Users.InsertOneAsync(newUser);

        return userForCreate.Name;
      }

      return null;
    }

    public async Task<User> GetUser(string username, string password)
    {
      return await Users.Find(user => user.Name == username && user.Password == password).FirstOrDefaultAsync();
    }

    public async Task<User> GetUser(string userId)
    {
      return await Users.Find(new BsonDocument("_id", new ObjectId(userId))).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Transfer>> getTransfers(string userId, string role)
    {
      if (role == "admin")
      {
        return await Transfers.Find(_ => true).ToListAsync();
      }

      return await Transfers.Find(transfer => transfer.UserId == userId).ToListAsync();
    }

    public async Task<Transfer> GetTransfer(string sessionId, string role, string transferId)
    {
      if (role == "admin")
      {
        return await Transfers.Find(transfer => transfer.Id == transferId).FirstOrDefaultAsync();
      }

      return await Transfers.Find(transfer => transfer.Id == transferId && transfer.UserId == sessionId).FirstOrDefaultAsync();
    }

    public async Task CreateTransfer(Transfer transfer)
    {
      await Transfers.InsertOneAsync(transfer);
    }

    public async Task Remove(string sessionId, string role, string transferId)
    {
      if (role == "admin")
      {
        await Transfers.DeleteOneAsync(new BsonDocument("_id", new ObjectId(transferId)));
        return;
      }

      await Transfers.DeleteOneAsync(transfer => transfer.Id == transferId && transfer.UserId == sessionId);
    }

    public async Task RemoveAll()
    {
      await Transfers.DeleteManyAsync(p => true);
    }
  }
}