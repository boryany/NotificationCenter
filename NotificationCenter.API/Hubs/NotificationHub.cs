using Microsoft.AspNetCore.SignalR;

namespace NotificationCenter.API.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }

        public override Task OnConnectedAsync()
        {
            var clientId = Context.User?.FindFirst("clientId")?.Value;
            if (clientId != null)
            {
                // Add the connection to a group identified by clientId
                Groups.AddToGroupAsync(Context.ConnectionId, clientId);
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var clientId = Context.User?.FindFirst("clientId")?.Value;
            if (clientId != null)
            {
                // Remove the connection from the group
                Groups.RemoveFromGroupAsync(Context.ConnectionId, clientId);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
