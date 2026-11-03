using Microsoft.AspNetCore.Mvc;
using TranslationDB.Models;
using TranslationDB.Services.Interfaces;

namespace TranslationDB.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return HandleResult(users);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving users: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                return HandleResult(user);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving user: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, User user)
        {
            try
            {
                if (id != user.Id)
                    return HandleError("ID mismatch");

                var result = await _userService.UpdateUser(user);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "User updated successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error updating user: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var result = await _userService.DeleteUser(id);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(null, "User deleted successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error deleting user: {ex.Message}");
            }
        }

        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(string id)
        {
            try
            {
                var result = await _userService.DeactivateUser(id);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "User deactivated successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error deactivating user: {ex.Message}");
            }
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> ActivateUser(string id)
        {
            try
            {
                var result = await _userService.ActivateUser(id);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "User activated successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error activating user: {ex.Message}");
            }
        }

        [HttpGet("{id}/notifications")]
        public async Task<IActionResult> GetUserNotifications(string id)
        {
            try
            {
                var notifications = await _userService.GetUserNotifications(id);
                return HandleResult(notifications);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving notifications: {ex.Message}");
            }
        }
    }
}