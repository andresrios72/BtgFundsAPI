using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BtgFundsApi.Models
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("transactionId")]
        public string TransactionId { get; set; } = null!; // UUID

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = null!;

        [BsonElement("fundId")]
        public int FundId { get; set; }

        [BsonElement("type")]
        public string Type { get; set; } = null!; // "subscription" o "cancellation"

        [BsonElement("amount")]
        public int Amount { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }
    }
}
