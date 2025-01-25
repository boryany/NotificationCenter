using Microsoft.AspNetCore.SignalR;
using NotificationCenter.Domain.Entities;

namespace NotificationCenter.Infrastructure.SignalR
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
    }
}
