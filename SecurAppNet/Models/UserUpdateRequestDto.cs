namespace SecurAppNet.Models
{
    public class UserUpdateRequestDto
    {
        public required string Username { get; set; }
        public required bool IsAdmin { get; set; }
    }
}