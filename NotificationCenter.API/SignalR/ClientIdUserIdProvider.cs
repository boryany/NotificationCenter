using Microsoft.AspNetCore.SignalR;

namespace NotificationCenter.API.SignalR
{
    public class ClientIdUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var clientIdClaim = connection.User?.FindFirst("clientId");
            return clientIdClaim?.Value;   
        }
    }
}
