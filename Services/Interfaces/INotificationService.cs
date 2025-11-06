using TranslationDB.Models;
using TranslationDB.Models.DTOs;

namespace TranslationDB.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ServiceResult<List<Notification>>> GetAllNotifications();
        Task<ServiceResult<Notification>> GetNotificationById(string id);
        Task<ServiceResult<Notification>> CreateNotification(Notification notification);
        Task<ServiceResult<Notification>> UpdateNotification(Notification notification);
        Task<ServiceResult<bool>> DeleteNotification(string id);
        Task<ServiceResult<Notification>> MarkAsRead(string id);
        Task<ServiceResult<bool>> MarkAllAsRead(string userId);
        Task<ServiceResult<List<Notification>>> GetUserNotifications(string userId, bool unreadOnly = false);
    }
}