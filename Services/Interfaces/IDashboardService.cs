using TranslationDB.Models.DTOs;
using TranslationManagementSystem.API.Models.DTOs;

namespace TranslationDB.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<ServiceResult<DashboardStatsDto>> GetDashboardStats(string userId, string userRole);
        Task<ServiceResult<List<object>>> GetRecentActivities();
        Task<ServiceResult<object>> GetProjectProgress(int projectId);
    }
}