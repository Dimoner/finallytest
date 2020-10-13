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

    public async Task CreateUser(CreateUser user)
    {
      User newUser = new User
      {
        Name = user.Name,
        Password = user.Password,
        Role = "user"
      };

      await Users.InsertOneAsync(newUser);
    }

    public async Task<User> GetUser(string username, string password)
    {
      return await Users.Find(user => user.Name == username && user.Password == password).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Transfer>> getTransfers(GetHistory getHistory)
    {
      var builder = new FilterDefinitionBuilder<Transfer>();
      var filter = builder.Empty;
      return await Transfers.Find(transfer => transfer.UserId == getHistory.UserId).ToListAsync();
    }

    public async Task<Transfer> GetTransfer(GetOneHistory getOneHistory)
    {
      return await Transfers.Find(transfer => transfer.Id == getOneHistory.TransferId && transfer.UserId == getOneHistory.UserId).FirstOrDefaultAsync();
    }

    public async Task CreateTransfer(Transfer transfer)
    {
      await Transfers.InsertOneAsync(transfer);
    }

    public async Task Remove(string id)
    {
      await Transfers.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
    }

    public async Task RemoveAll()
    {
      await Transfers.DeleteManyAsync(p => true);
    }
  }
}