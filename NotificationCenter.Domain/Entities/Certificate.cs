namespace NotificationCenter.Domain.Entities
{
    public class Certificate
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public required string SerialNumber { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public Client? Client { get; set; }  
    }
}
