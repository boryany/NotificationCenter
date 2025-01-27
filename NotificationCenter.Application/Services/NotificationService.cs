using NotificationCenter.Application.Hubs;
using NotificationCenter.Application.Interfaces;
using NotificationCenter.Domain.Entities;
using NotificationCenter.Domain.Repositories;


namespace NotificationCenter.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly INotificationHub _hub;

        public NotificationService(INotificationRepository repo, INotificationHub hub)
        {
            _repo = repo;
            _hub = hub;
        }

        public async Task CreateNotificationAsync(int clientId, int eventId, params object[] parameters)
        {
            var notification = new Notification
            {
                ClientId = clientId,
                EventId = eventId,
                Message = FormatMessage(eventId, parameters),
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(notification);
            await _hub.SendNotificationToClient(clientId.ToString(), notification.Message);
        }

        private string FormatMessage(int eventId, object[] parameters)
        {
            // Format based on event type 
            return eventId switch
            {
                1 => $"Your certificate is expired: {parameters[0]} - {parameters[1]}, Serial number: {parameters[2]}",
                2 => $"Request {parameters[0]} is with status: {parameters[1]}",
                _ => "Unknown event."
            };
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(int clientId)
        {
            return await _repo.GetNotificationsByClientAsync(clientId);
        }
    
        public async Task NotifyClient(int clientId, string message)
        {
            await _hub.SendNotificationAsync(clientId, message);
        }

        public async Task NotifyClientDirectly(int clientId, string message)
        {
            await _hub.SendNotificationToClient(clientId.ToString(), message);
        }
    }


}
