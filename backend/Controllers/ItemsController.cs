using Microsoft.AspNetCore.Mvc;
using MiniStok.Api.Models;
using MiniStok.Api.Services;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniStok.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class ItemsController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        
        public ItemsController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

       
        [HttpGet]
        public async Task<ActionResult<List<Item>>> GetItems()
        {
            var collection = _mongoDbService.GetCollection<Item>("items"); 
            var items = await collection.Find(_ => true).ToListAsync(); 
            return Ok(items);
        }

        
        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem([FromBody] Item item)
        {
            var collection = _mongoDbService.GetCollection<Item>("items");
            item.StockQuantity = 0; 
            item.IsActive = true; 
            await collection.InsertOneAsync(item); 
            return CreatedAtAction(nameof(GetItems), new { id = item.Id }, item); // 201 Created
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(string id, [FromBody] Item updatedItem)
        {
            var collection = _mongoDbService.GetCollection<Item>("items");
            var filter = Builders<Item>.Filter.Eq(i => i.Id, id); 
            var update = Builders<Item>.Update
                        .Set(i => i.Name, updatedItem.Name)
                        .Set(i => i.Unit, updatedItem.Unit)
                        .Set(i => i.IsActive, updatedItem.IsActive);
            var result = await collection.UpdateOneAsync(filter, update);
            if (result.MatchedCount == 0) return NotFound(); 
            return NoContent(); 
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            var collection = _mongoDbService.GetCollection<Item>("items");
            var result = await collection.DeleteOneAsync(i => i.Id == id);
            if (result.DeletedCount == 0) return NotFound(); 
            return NoContent(); 
        }
    }
}
