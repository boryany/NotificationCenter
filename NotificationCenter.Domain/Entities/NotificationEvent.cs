using NotificationCenter.Domain.Enums;

namespace NotificationCenter.Domain.Entities
{
    public class NotificationEvent
    {
        public int Id { get; set; }
        public required string EventName { get; set; }
        public TargetGroup TargetGroup { get; set; } 
        public Channel Channel { get; set; }
        public DateTime CreateTime { get; set; }

        public int? RequestId { get; set; }
        public Request? Request { get; set; }

        public int? CertificateId { get; set; }
        public Certificate? Certificate { get; set; }
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
