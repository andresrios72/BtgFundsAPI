using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BtgFundsApi.Models
{
    public class Fund
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("minAmount")]
        public int MinAmount { get; set; }

        [BsonElement("category")]
        public string Category { get; set; } = null!;
    }
}
