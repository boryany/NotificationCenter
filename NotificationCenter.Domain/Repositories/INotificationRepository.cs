using NotificationCenter.Domain.Entities;

namespace NotificationCenter.Domain.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification> GetByIdAsync(int id);
        Task<IEnumerable<Notification>> GetAllAsync();
        Task AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(int id);
        Task<IEnumerable<Certificate>> GetExpiringCertificatesAsync(DateTime utcNow);
        Task<IEnumerable<Notification>> GetNotificationsByClientAsync(int clientId);
    }
}
