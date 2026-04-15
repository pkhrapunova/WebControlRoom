namespace WebControlRoom.Models
{
    public class EmailConfirmCode
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}