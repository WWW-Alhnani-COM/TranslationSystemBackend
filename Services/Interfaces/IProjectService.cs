using TranslationDB.Models;
using TranslationDB.Models.DTOs;
using TranslationManagementSystem.API.Models.DTOs;

namespace TranslationDB.Services.Interfaces
{
    public interface IProjectService
    {
        Task<ServiceResult<List<Project>>> GetAllProjects();
        Task<ServiceResult<Project>> GetProjectById(int id);
        Task<ServiceResult<Project>> CreateProject(ProjectCreateDto projectCreateDto);
        Task<ServiceResult<Project>> UpdateProject(Project project);
        Task<ServiceResult<bool>> DeleteProject(int id);
        Task<ServiceResult<List<Paragraph>>> GetProjectParagraphs(int projectId);
        Task<ServiceResult<ProjectAssignment>> AssignUserToProject(int projectId, ProjectAssignmentDto assignment);
        Task<ServiceResult<List<Project>>> GetUserProjects(string userId);
        Task<ServiceResult<Project>> UpdateProjectStatus(int projectId, string status);
    }
}