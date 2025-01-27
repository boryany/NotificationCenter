using Microsoft.AspNetCore.SignalR;

using NotificationCenter.Domain.Entities;
using NotificationCenter.Domain.Repositories;

namespace NotificationCenter.Application.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotificationToClient(string clientId, string message)
        {
            await Clients.Group(clientId).SendAsync("ReceiveNotification", message);
        }
    }
}
