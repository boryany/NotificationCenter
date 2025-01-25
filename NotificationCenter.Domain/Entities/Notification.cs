namespace NotificationCenter.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int EventId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Client Client { get; set; } 
    }
}
