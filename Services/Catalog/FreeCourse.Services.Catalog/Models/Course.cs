using MongoDB.Bson.Serialization.Attributes;

namespace FreeCourse.Services.Catalog.Models
{
    public class Course
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public Decimal Price { get; set; }

        public string UserId { get; set; }
        public string Picture { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        public DateTime CreateTime { get; set; }

        public Feature Feature { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string CategoryId { get; set; }

        [BsonIgnore]
        public Category Category { get; set; }

    }
}
