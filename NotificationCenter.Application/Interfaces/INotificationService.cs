using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationCenter.Domain.Entities;

namespace NotificationCenter.Application.Interfaces
{
    public interface INotificationService
    {
        Task AddNotificationAsync(Notification notification);
        Task ProcessNotificationEventAsync(NotificationEvent notificationEvent);
    }
}
