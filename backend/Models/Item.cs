using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MiniStok.Api.Models
{
  public class Item
  {
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } 

    [BsonElement("code")]
    public string Code { get; set; } 

    [BsonElement("name")]
    public string Name { get; set; } 

    [BsonElement("unit")]
    public string Unit { get; set; } 

    [BsonElement("stockQuantity")]
    public double StockQuantity { get; set; } 

    [BsonElement("isActive")]
    public bool IsActive { get; set; } 

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
  }
}