using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationCenter.API.DTOs;
using NotificationCenter.Application.Interfaces;
using NotificationCenter.Domain.Entities;
using NotificationCenter.Domain.Repositories;

[ApiController]
[Route("api/notifications")]
public class NotificationssController : ControllerBase
{
    private readonly INotificationRepository _notificationRepository;

    private readonly INotificationService _notificationService;

    public NotificationssController(INotificationService notificationService, INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
        _notificationService = notificationService;
    }

    [Authorize]
    [HttpGet("{clientId}/notifications")]
    public async Task<IActionResult> GetNotifications(int clientId, int skip = 0, int take = 10)
    {
        var notifications = await _notificationRepository
            .GetNotificationsByClientAsync(clientId);

        var paginatedNotifications = notifications
            .Skip(skip)
            .Take(take)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            })
            .ToList();

        return Ok(paginatedNotifications);
    }

    #region Test Endpoints
     
    [AllowAnonymous]
    [HttpPost("addnotificationevent")]
    public async Task<IActionResult> AddNotificationEvent(int clientId, [FromBody] NotificationEvent notificationEvent)
    {
        if (notificationEvent == null || notificationEvent.Id > 0)
        {
            return BadRequest("Invalid notification data.");
        }

        await _notificationService.ProcessNotificationEventAsync(notificationEvent);
        return Ok("Notification event added and client notified.");
    }

    [HttpPost("{clientId}/addnotification")]
    public async Task<IActionResult> AddNotification(int clientId, [FromBody] Notification notification)
    {
        if (notification == null || notification.ClientId != clientId)
        {
            return BadRequest("Invalid notification data.");
        }

        await _notificationService.AddNotificationAsync(notification);
        return Ok("Notification added and client notified.");
    }

    #endregion

}
