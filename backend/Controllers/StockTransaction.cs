using Microsoft.AspNetCore.Mvc;
using MiniStok.Api.Models;
using MiniStok.Api.Services;
using MongoDB.Driver;

namespace MiniStok.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class StockTransactionController : ControllerBase
  {
    private readonly MongoDbService _mongoDbService;

    public StockTransactionController(MongoDbService mongoDbService)
    {
      _mongoDbService = mongoDbService;
    }

    [HttpPost]
    public async Task<ActionResult<StockTransaction>> CreateTransaction([FromBody] StockTransaction transaction)
    {
      var itemCollection = _mongoDbService.GetCollection<Item>("items");
      var transactionCollection = _mongoDbService.GetCollection<StockTransaction>("stockTransactions");

      
      var item = await itemCollection.Find(x => x.Id == transaction.ItemId).FirstOrDefaultAsync();
      if (item == null)
        return NotFound("Item not found");

      
      if (transaction.Type == "IN")
        item.StockQuantity += transaction.Quantity;
      else if (transaction.Type == "OUT")
        item.StockQuantity -= transaction.Quantity;

      
      await itemCollection.ReplaceOneAsync(x => x.Id == item.Id, item);

      
      await transactionCollection.InsertOneAsync(transaction);

      return CreatedAtAction(nameof(GetTransactions), new { id = transaction.Id }, transaction);
    }

    public async Task<ActionResult<List<object>>> GetTransactions()
    {
      var transactionCollection = _mongoDbService.GetCollection<StockTransaction>("stockTransactions");
      var itemCollection = _mongoDbService.GetCollection<Item>("items");

      var transactions = await transactionCollection.Find(_ => true).ToListAsync();

      var result = transactions.Select(t =>
      {
        var item = itemCollection.Find(x => x.Id == t.ItemId).FirstOrDefault();
        return new
        {
          t.Id,
          t.ItemId,
          ItemName = item?.Name ?? "Bilinmiyor",
          t.Type,
          t.Quantity,
          t.Note,
          t.Date
        };
      }).ToList();

      return Ok(result);
    }
    [HttpGet("summary")]
    public async Task<ActionResult<List<object>>> GetTransactionsSummary()
    {
      var transactionCollection = _mongoDbService.GetCollection<StockTransaction>("stockTransactions");
      var itemCollection = _mongoDbService.GetCollection<Item>("items");

      
      var transactions = await transactionCollection.Find(_ => true).ToListAsync();

      
      var summary = transactions
        .GroupBy(t => t.ItemId)
        .Select(g =>
        {
          var item = itemCollection.Find(x => x.Id == g.Key).FirstOrDefault();
          return new
          {
            ItemId = g.Key,
            ItemName = item?.Name ?? "Unknown",
            TotalIN = g.Where(x => x.Type == "IN").Sum(x => x.Quantity),
            TotalOUT = g.Where(x => x.Type == "OUT").Sum(x => x.Quantity),
            CurrentStock = item?.StockQuantity ?? 0
          };
        })
        .ToList();

      return Ok(summary);
    }
    
  }
  
}