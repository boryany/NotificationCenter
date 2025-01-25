namespace NotificationCenter.Domain.Entities
{
    public class Request
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public required string RequestType { get; set; }
        public DateTime RequestDate { get; set; }
        public required string Status { get; set; }

        public Client? Client { get; set; } 
    }
}
