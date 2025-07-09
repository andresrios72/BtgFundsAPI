using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BtgFundsApi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("balance")]
        public int Balance { get; set; }

        [BsonElement("notificationPreference")]
        public string NotificationPreference { get; set; } = null!; // "email" o "sms"

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("phone")]
        public string Phone { get; set; } = null!;

        [BsonElement("fundsSubscribed")]
        public List<UserFundSubscription> FundsSubscribed { get; set; } = new();
    
        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } = null!;

        [BsonElement("role")]
        public string Role { get; set; } = "user"; // user o admin
    }

    public class UserFundSubscription
    {
        [BsonElement("fundId")]
        public int FundId { get; set; }

        [BsonElement("subscriptionDate")]
        public DateTime SubscriptionDate { get; set; }

        [BsonElement("amount")]
        public int Amount { get; set; }
    }
}
