using TranslationDB.Models;
using TranslationDB.Models.DTOs;

namespace TranslationDB.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<List<User>>> GetAllUsers();
        Task<ServiceResult<User>> GetUserById(string id);
        Task<ServiceResult<User>> UpdateUser(User user);
        Task<ServiceResult<bool>> DeleteUser(string id);
        Task<ServiceResult<User>> DeactivateUser(string id);
        Task<ServiceResult<User>> ActivateUser(string id);
        Task<ServiceResult<List<Notification>>> GetUserNotifications(string userId);
    }
}