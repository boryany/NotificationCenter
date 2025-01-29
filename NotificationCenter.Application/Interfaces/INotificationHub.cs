using NotificationCenter.Domain.Entities;

namespace NotificationCenter.Application.Interfaces
{
    public interface INotificationHub
    {
        Task SendNotificationAsync(int clientId, string message);
        Task SendNotificationMessage(string clientGroup, string message);
        Task SendNotification(string clientId, Notification notification);
    }
}
