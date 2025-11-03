using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MiniStok.Api.Services
{
  public class MongoDbService
  {
    private readonly IMongoDatabase _database;

    public MongoDbService(IConfiguration configuration)
    {
      
     
      var connectionString = "mongodb://localhost:27017"; 
      var dbName = "MiniStokDB"; 

      
      var client = new MongoClient(connectionString); 
      _database = client.GetDatabase(dbName); 
      
    }
    
    public IMongoCollection<T> GetCollection<T>(string name) 
    {
      return _database.GetCollection<T>(name); 
    }
  }
}

