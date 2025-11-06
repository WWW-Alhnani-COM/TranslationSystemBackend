using Microsoft.EntityFrameworkCore;
using TranslationDB.Data;
using TranslationDB.Models;
using TranslationDB.Models.DTOs;
using TranslationDB.Services.Interfaces;

namespace TranslationDB.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<User>>> GetAllUsers()
        {
            try
            {
                var users = await _context.Users
                    .Where(u => u.IsActive)
                    .OrderBy(u => u.FullName)
                    .ToListAsync();

                return Success(users);
            }
            catch (Exception ex)
            {
                return Error<List<User>>($"Error retrieving users: {ex.Message}");
            }
        }

        public async Task<ServiceResult<User>> GetUserById(string id)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

                if (user == null)
                {
                    return Error<User>("User not found");
                }

                return Success(user);
            }
            catch (Exception ex)
            {
                return Error<User>($"Error retrieving user: {ex.Message}");
            }
        }

        public async Task<ServiceResult<User>> UpdateUser(User user)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(user.Id);
                if (existingUser == null)
                {
                    return Error<User>("User not found");
                }

                // Update allowed properties
                existingUser.UserName = user.UserName;
                existingUser.FullName = user.FullName;
                existingUser.ProfileImageUrl = user.ProfileImageUrl;
                existingUser.Role = user.Role;

                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();

                return Success(existingUser, "User updated successfully");
            }
            catch (Exception ex)
            {
                return Error<User>($"Error updating user: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteUser(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return Error<bool>("User not found");
                }

                // Soft delete by deactivating
                user.IsActive = false;
                await _context.SaveChangesAsync();

                return Success(true, "User deleted successfully");
            }
            catch (Exception ex)
            {
                return Error<bool>($"Error deleting user: {ex.Message}");
            }
        }

        public async Task<ServiceResult<User>> DeactivateUser(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return Error<User>("User not found");
                }

                user.IsActive = false;
                await _context.SaveChangesAsync();

                return Success(user, "User deactivated successfully");
            }
            catch (Exception ex)
            {
                return Error<User>($"Error deactivating user: {ex.Message}");
            }
        }

        public async Task<ServiceResult<User>> ActivateUser(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return Error<User>("User not found");
                }

                user.IsActive = true;
                await _context.SaveChangesAsync();

                return Success(user, "User activated successfully");
            }
            catch (Exception ex)
            {
                return Error<User>($"Error activating user: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<Notification>>> GetUserNotifications(string userId)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Where(n => n.UserId == userId)
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();

                return Success(notifications);
            }
            catch (Exception ex)
            {
                return Error<List<Notification>>($"Error retrieving notifications: {ex.Message}");
            }
        }
    }
}
