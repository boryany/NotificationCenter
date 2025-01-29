using NotificationCenter.Domain.Entities;

namespace NotificationCenter.Domain.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification?> GetByIdAsync(int id);
        Task<IEnumerable<Notification>> GetAllAsync();
        Task AddAsync(Notification notification);
        Task<Notification> AddNotificationAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(int id);
        Task<IEnumerable<Certificate>> GetExpiringCertificatesAsync(DateTime utcNow);
        Task<IEnumerable<Notification>> GetNotificationsByClientAsync(int clientId);
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<IEnumerable<Client>> GetBusinessClientsAsync();
        Task<IEnumerable<Client>> GetIndividualClientsAsync();
        Task<Request?> GetRequestByIdAsync(int requestId);
        Task<Certificate?> GetCertificateByIdAsync(int certificateId);
        Task AddNotificationsAsync(IEnumerable<Notification> notifications);
        Task<NotificationEvent> AddNotificationEventAsync(NotificationEvent notificationEvent);

    }
}
