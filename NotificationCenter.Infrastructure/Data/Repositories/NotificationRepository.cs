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

        public async Task<Notification> GetByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _context.Notifications.ToListAsync();
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
    }
}