using Microsoft.EntityFrameworkCore;
using TranslationDB.Data;
using TranslationDB.Models;
using TranslationDB.Models.DTOs;
using TranslationDB.Services.Interfaces;

namespace TranslationDB.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Notification>>> GetAllNotifications()
        {
            try
            {
                var notifications = await _context.Notifications
                    .Include(n => n.User)
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();

                return Success(notifications);
            }
            catch (Exception ex)
            {
                return Error<List<Notification>>($"Error retrieving notifications: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Notification>> GetNotificationById(string id)
        {
            try
            {
                var notification = await _context.Notifications
                    .Include(n => n.User)
                    .FirstOrDefaultAsync(n => n.Id == id);

                if (notification == null)
                {
                    return Error<Notification>("Notification not found");
                }

                return Success(notification);
            }
            catch (Exception ex)
            {
                return Error<Notification>($"Error retrieving notification: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Notification>> CreateNotification(Notification notification)
        {
            try
            {
                notification.Id = Guid.NewGuid().ToString();
                notification.CreatedAt = DateTime.UtcNow;
                notification.IsRead = false;

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                return Success(notification, "Notification created successfully");
            }
            catch (Exception ex)
            {
                return Error<Notification>($"Error creating notification: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Notification>> UpdateNotification(Notification notification)
        {
            try
            {
                var existingNotification = await _context.Notifications.FindAsync(notification.Id);
                if (existingNotification == null)
                {
                    return Error<Notification>("Notification not found");
                }

                // Update properties
                existingNotification.Type = notification.Type;
                existingNotification.Title = notification.Title;
                existingNotification.Message = notification.Message;
                existingNotification.RelatedEntityId = notification.RelatedEntityId;
                existingNotification.IsRead = notification.IsRead;

                if (notification.IsRead && !existingNotification.IsRead)
                {
                    existingNotification.ReadAt = DateTime.UtcNow;
                }

                _context.Notifications.Update(existingNotification);
                await _context.SaveChangesAsync();

                return Success(existingNotification, "Notification updated successfully");
            }
            catch (Exception ex)
            {
                return Error<Notification>($"Error updating notification: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteNotification(string id)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(id);
                if (notification == null)
                {
                    return Error<bool>("Notification not found");
                }

                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();

                return Success(true, "Notification deleted successfully");
            }
            catch (Exception ex)
            {
                return Error<bool>($"Error deleting notification: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Notification>> MarkAsRead(string id)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(id);
                if (notification == null)
                {
                    return Error<Notification>("Notification not found");
                }

                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;

                _context.Notifications.Update(notification);
                await _context.SaveChangesAsync();

                return Success(notification, "Notification marked as read");
            }
            catch (Exception ex)
            {
                return Error<Notification>($"Error marking notification as read: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> MarkAllAsRead(string userId)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Where(n => n.UserId == userId && !n.IsRead)
                    .ToListAsync();

                foreach (var notification in notifications)
                {
                    notification.IsRead = true;
                    notification.ReadAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                return Success(true, "All notifications marked as read");
            }
            catch (Exception ex)
            {
                return Error<bool>($"Error marking notifications as read: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<Notification>>> GetUserNotifications(string userId, bool unreadOnly = false)
        {
            try
            {
                var query = _context.Notifications
                    .Where(n => n.UserId == userId);

                if (unreadOnly)
                {
                    query = query.Where(n => !n.IsRead);
                }

                var notifications = await query
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();

                return Success(notifications);
            }
            catch (Exception ex)
            {
                return Error<List<Notification>>($"Error retrieving user notifications: {ex.Message}");
            }
        }
    }
}