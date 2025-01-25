namespace NotificationCenter.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsIndividual { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public ICollection<Notification> Notifications { get; set; }  
    }
}
