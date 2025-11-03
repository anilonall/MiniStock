using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MiniStok.Api.Models
{
  public class StockTransaction
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("itemId")]
    public string ItemId { get; set; } 

    [BsonElement("date")]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [BsonElement("type")]
    public string Type { get; set; } 

    [BsonElement("quantity")]
    public double Quantity { get; set; }

    [BsonElement("note")]
    public string Note { get; set; }
  }
}