using Microsoft.AspNetCore.SignalR;
using NotificationCenter.Application.Hubs;
using NotificationCenter.Application.Interfaces;

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

    public async Task SendNotificationToClient(string clientGroup, string message)
    {
        // Send notification to a specific client group
        await _hubContext.Clients.Group(clientGroup).SendAsync("ReceiveNotification", message);
    }
}
