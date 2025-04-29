using MongoDB.Bson.Serialization.Attributes;

namespace FreeCourse.Services.Catalog.Models
{
    public class Category
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
