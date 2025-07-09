namespace BtgFundsApi.DTOs
{
    public class UserRegisterRequest
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string NotificationPreference { get; set; } = null!; // "email" o "sms"
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "user"; // "user" o "admin"
    }
}
