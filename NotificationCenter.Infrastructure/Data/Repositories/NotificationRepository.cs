using Microsoft.EntityFrameworkCore;
using NotificationCenter.Domain.Entities;
using NotificationCenter.Domain.Repositories;

namespace NotificationCenter.Infrastructure.Data.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDbContext _context;

        public NotificationRepository(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _context.Notifications.ToListAsync();
        }
        public async Task<Notification> AddNotificationAsync(Notification notification)
        {            
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            
            return notification;
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<IEnumerable<Certificate>> GetExpiringCertificatesAsync(DateTime utcNow)
        {
            return await _context.Certificates
                .Where(c => c.ValidTo <= utcNow && c.ValidTo >= utcNow.AddDays(-30))
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByClientAsync(int clientId)
        {
            return await _context.Notifications
                .Where(n => n.ClientId == clientId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<IEnumerable<Client>> GetBusinessClientsAsync()
        {
            return await _context.Clients.Where(c => !c.IsIndividual).ToListAsync();
        }

        public async Task<IEnumerable<Client>> GetIndividualClientsAsync()
        {
            return await _context.Clients.Where(c => c.IsIndividual).ToListAsync();
        }

        public async Task<Request?> GetRequestByIdAsync(int requestId)
        {
            return await _context.Requests.FirstOrDefaultAsync(r => r.Id == requestId);
        }

        public async Task<Certificate?> GetCertificateByIdAsync(int certificateId)
        {
            return await _context.Certificates.FirstOrDefaultAsync(c => c.Id == certificateId);
        }

        public async Task AddNotificationsAsync(IEnumerable<Notification> notifications)
        {
            _context.Notifications.AddRange(notifications);
            await _context.SaveChangesAsync();
        }

        public async Task<NotificationEvent> AddNotificationEventAsync(NotificationEvent notificationEvent)
        {
            _context.NotificationEvents.Add(notificationEvent);

            await _context.SaveChangesAsync();

            return notificationEvent;
        }

    }
}

