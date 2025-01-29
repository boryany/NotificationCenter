using Microsoft.AspNetCore.SignalR;
using NotificationCenter.Application.Hubs;
using NotificationCenter.Application.Interfaces;
using NotificationCenter.Domain.Entities;

public class NotificationHubService : INotificationHub
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationHubService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendNotificationAsync(int clientId, string message)
    {
        // Send notification to a specific client group
        await _hubContext.Clients.Group(clientId.ToString()).SendAsync("ReceiveNotification", message);
    }

    public async Task SendNotificationMessage(string clientGroup, string message)
    {
        // Send notification to a specific client group
        await _hubContext.Clients.Group(clientGroup).SendAsync("ReceiveNotification", message);
    }

    public async Task SendNotification(string clientId, Notification notification)
    {
        // Send the notification to the specified client
        await _hubContext.Clients.User(clientId).SendAsync("ReceiveNotification", notification);
    }
}
