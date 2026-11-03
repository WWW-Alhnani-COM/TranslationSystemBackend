using Microsoft.AspNetCore.Mvc;
using TranslationDB.Models;
using TranslationDB.Services;
using TranslationDB.Services.Interfaces;

namespace TranslationDB.Controllers
{
    public class NotificationsController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications()
        {
            try
            {
                var notifications = await _notificationService.GetAllNotifications();
                return HandleResult(notifications);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving notifications: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(string id)
        {
            try
            {
                var notification = await _notificationService.GetNotificationById(id);
                return HandleResult(notification);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving notification: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification(Notification notification)
        {
            try
            {
                var result = await _notificationService.CreateNotification(notification);
                if (!result.Success)
                    return HandleError(result.Message);

                return CreatedAtAction(nameof(GetNotificationById), new { id = result.Data.Id }, 
                    new { success = true, data = result.Data, message = "Notification created successfully" });
            }
            catch (Exception ex)
            {
                return HandleError($"Error creating notification: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(string id, Notification notification)
        {
            try
            {
                if (id != notification.Id)
                    return HandleError("ID mismatch");

                var result = await _notificationService.UpdateNotification(notification);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "Notification updated successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error updating notification: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(string id)
        {
            try
            {
                var result = await _notificationService.DeleteNotification(id);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult<object?>(null, "Notification deleted successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error deleting notification: {ex.Message}");
            }
        }

        [HttpPost("{id}/mark-as-read")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            try
            {
                var result = await _notificationService.MarkAsRead(id);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "Notification marked as read");
            }
            catch (Exception ex)
            {
                return HandleError($"Error marking notification as read: {ex.Message}");
            }
        }

        [HttpPost("mark-all-as-read")]
        public async Task<IActionResult> MarkAllAsRead([FromBody] string userId)
        {
            try
            {
                var result = await _notificationService.MarkAllAsRead(userId);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult<object?>(null, "All notifications marked as read");
            }
            catch (Exception ex)
            {
                return HandleError($"Error marking notifications as read: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(string userId, [FromQuery] bool unreadOnly = false)
        {
            try
            {
                var notifications = await _notificationService.GetUserNotifications(userId, unreadOnly);
                return HandleResult(notifications);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving user notifications: {ex.Message}");
            }
        }
    }
}