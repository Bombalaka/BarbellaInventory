using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BarbellaInventory.Models
{
    public class BarbieSet
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string? Id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Name cannot exceed 20 characters")]
        [BsonElement("Name")]
        public string? Name { get; set; }
        [Required]
        [BsonElement("Description")]
        public string? Description { get; set; }
        
    }
}