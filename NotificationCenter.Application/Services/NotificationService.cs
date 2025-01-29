using Microsoft.AspNetCore.SignalR;
using NotificationCenter.Application.Hubs;
using NotificationCenter.Application.Interfaces;
using NotificationCenter.Domain.Entities;
using NotificationCenter.Domain.Enums;
using NotificationCenter.Domain.Repositories;


namespace NotificationCenter.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly INotificationHub _hub;
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationService(INotificationRepository repo, INotificationHub hub, IHubContext<NotificationHub> hubContext)
        {
            _repo = repo;
            _hub = hub;
            _hubContext = hubContext;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            // Save notification to the database
            var savedNotification = await _repo.AddNotificationAsync(notification);

            // Notify the client in real time
            await _hubContext.Clients
                .User(savedNotification.ClientId.ToString()) // Target specific user
                .SendAsync("ReceiveNotification", savedNotification);
        }

        public async Task ProcessNotificationEventAsync(NotificationEvent notificationEvent)
        {
            if (notificationEvent != null)
            {
                if (notificationEvent.Id == 0)
                {
                    // new event
                    notificationEvent = await _repo.AddNotificationEventAsync(notificationEvent);
                }
                var notifications = new List<Notification>();

                switch (notificationEvent.TargetGroup)
                {
                    case TargetGroup.All:
                        var allClients = await _repo.GetAllClientsAsync();
                        notifications.AddRange(allClients.Select(client => new Notification
                        {
                            ClientId = client.Id,
                            EventId = notificationEvent.Id,
                            Message = notificationEvent.EventName,
                            CreatedAt = DateTime.UtcNow
                        }));
                        break;

                    case TargetGroup.AllCompany:
                        var businessClients = await _repo.GetBusinessClientsAsync();
                        notifications.AddRange(businessClients.Select(client => new Notification
                        {
                            ClientId = client.Id,
                            EventId = notificationEvent.Id,
                            Message = notificationEvent.EventName,
                            CreatedAt = DateTime.UtcNow
                        }));
                        break;

                    case TargetGroup.AllIndividual:
                        var individualClients = await _repo.GetIndividualClientsAsync();
                        notifications.AddRange(individualClients.Select(client => new Notification
                        {
                            ClientId = client.Id,
                            EventId = notificationEvent.Id,
                            Message = notificationEvent.EventName,
                            CreatedAt = DateTime.UtcNow
                        }));
                        break;

                    case TargetGroup.Company:
                    case TargetGroup.Individual:
                        if (notificationEvent.RequestId.HasValue)
                        {
                            var request = await _repo.GetRequestByIdAsync(notificationEvent.RequestId.Value);
                            if (request != null)
                            {
                                notifications.Add(new Notification
                                {
                                    ClientId = request.ClientId,
                                    EventId = notificationEvent.Id,
                                    Message = $"{notificationEvent.EventName} - request id: {request.Id}",
                                    CreatedAt = DateTime.UtcNow
                                });
                            }
                        }
                        else if (notificationEvent.CertificateId.HasValue)
                        {
                            var certificate = await _repo.GetCertificateByIdAsync(notificationEvent.CertificateId.Value);
                            if (certificate != null)
                            {
                                notifications.Add(new Notification
                                {
                                    ClientId = certificate.ClientId,
                                    EventId = notificationEvent.Id,
                                    Message = $"{notificationEvent.EventName} - certificate id: {certificate.Id}",
                                    CreatedAt = DateTime.UtcNow
                                });
                            }
                        }
                        break;

                    default:
                        throw new InvalidOperationException("Invalid TargetGroup specified for NotificationEvent.");
                }

                // Save notifications to the database
                await _repo.AddNotificationsAsync(notifications);

                // Notify clients in real-time via SignalR
                foreach (var notification in notifications)
                {
                    await _hubContext.Clients
                        .User(notification.ClientId.ToString())
                        .SendAsync("ReceiveNotification", new NotificationDTO
                        {
                            Message = notification.Message,
                            CreatedAt = notification.CreatedAt,
                            Id = notification.Id,
                            ClientId = notification.ClientId,
                            EventId = notification.EventId,
                            IsRead = notification.IsRead
                        });
                }
            }

        }
    }

}
