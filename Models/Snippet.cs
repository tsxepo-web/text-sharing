using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Snippet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("slug")]
        public string? Slug { get; set; }

        [BsonElement("text")]
        public string? Text { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("editedAt")]
        public DateTime? EditedAt { get; set; }
        
        [BsonElement("previousVersions")]
        public List<string>? PreviousVersions { get; set; }
    }
}