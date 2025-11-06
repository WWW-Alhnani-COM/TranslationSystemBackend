using Microsoft.AspNetCore.Mvc;
using TranslationDB.Models.DTOs;
using TranslationDB.Services;
using TranslationDB.Services.Interfaces;

namespace TranslationDB.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("stats/{userId}")]
        public async Task<IActionResult> GetDashboardStats(string userId, [FromQuery] string userRole)
        {
            try
            {
                var stats = await _dashboardService.GetDashboardStats(userId, userRole);
                return HandleResult(stats);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving dashboard stats: {ex.Message}");
            }
        }

        [HttpGet("recent-activities")]
        public async Task<IActionResult> GetRecentActivities()
        {
            try
            {
                var activities = await _dashboardService.GetRecentActivities();
                return HandleResult(activities);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving recent activities: {ex.Message}");
            }
        }

        [HttpGet("project-progress/{projectId}")]
        public async Task<IActionResult> GetProjectProgress(int projectId)
        {
            try
            {
                var progress = await _dashboardService.GetProjectProgress(projectId);
                return HandleResult(progress);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving project progress: {ex.Message}");
            }
        }
    }
}