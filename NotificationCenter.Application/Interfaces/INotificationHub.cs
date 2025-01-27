namespace NotificationCenter.Application.Interfaces
{
    public interface INotificationHub
    {
        Task SendNotificationAsync(int clientId, string message);
        Task SendNotificationToClient(string clientGroup, string message);
    }
}
