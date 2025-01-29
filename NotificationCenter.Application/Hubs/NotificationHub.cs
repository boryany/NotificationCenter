using Microsoft.AspNetCore.SignalR;

using NotificationCenter.Domain.Entities;
using NotificationCenter.Domain.Repositories;

namespace NotificationCenter.Application.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotificationMessage(string clientId, string message)
        {
            await Clients.Group(clientId).SendAsync("ReceiveNotification", message);
        }

        public async Task SendNotification(string clientId, Notification notification)
        {
            await Clients.User(clientId).SendAsync("ReceiveNotification", notification);
        }
    }
}
