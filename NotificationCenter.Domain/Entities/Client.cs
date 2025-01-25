namespace NotificationCenter.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsIndividual { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }

        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<Request> Requests { get; set; } = new List<Request>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
