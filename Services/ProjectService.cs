using Microsoft.EntityFrameworkCore;
using TranslationDB.Data;
using TranslationDB.Models;
using TranslationDB.Enums;
using TranslationDB.Models.DTOs;
using TranslationDB.Services.Interfaces;
using TranslationManagementSystem.API.Models.DTOs;

namespace TranslationDB.Services
{
    public class ProjectService : BaseService, IProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Project>>> GetAllProjects()
        {
            try
            {
                var projects = await _context.Projects
                    .Include(p => p.Creator)
                    .Include(p => p.Paragraphs)
                    .Include(p => p.ProjectAssignments)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();

                return Success(projects);
            }
            catch (Exception ex)
            {
                return Error<List<Project>>($"Error retrieving projects: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Project>> GetProjectById(int id)
        {
            try
            {
                var project = await _context.Projects
                    .Include(p => p.Creator)
                    .Include(p => p.Paragraphs)
                    .Include(p => p.ProjectAssignments)
                        .ThenInclude(pa => pa.User)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (project == null)
                {
                    return Error<Project>("Project not found");
                }

                return Success(project);
            }
            catch (Exception ex)
            {
                return Error<Project>($"Error retrieving project: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Project>> CreateProject(ProjectCreateDto projectCreateDto)
        {
            try
            {
                var project = new Project
                {
                    Name = projectCreateDto.Name,
                    Description = projectCreateDto.Description,
                    SourceLanguage = projectCreateDto.SourceLanguage,
                    TargetLanguages = projectCreateDto.TargetLanguages,
                    Status = ProjectStatus.Created,
                    CreatedById = "system", // Replaced with the authenticated user ID when project creation is secured.
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Deadline = projectCreateDto.Deadline,
                    Settings = projectCreateDto.Settings
                };

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                return Success(project, "Project created successfully");
            }
            catch (Exception ex)
            {
                return Error<Project>($"Error creating project: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Project>> UpdateProject(Project project)
        {
            try
            {
                var existingProject = await _context.Projects.FindAsync(project.Id);
                if (existingProject == null)
                {
                    return Error<Project>("Project not found");
                }

                // Update properties
                existingProject.Name = project.Name;
                existingProject.Description = project.Description;
                existingProject.SourceLanguage = project.SourceLanguage;
                existingProject.TargetLanguages = project.TargetLanguages;
                existingProject.Status = project.Status;
                existingProject.Deadline = project.Deadline;
                existingProject.Settings = project.Settings;
                existingProject.UpdatedAt = DateTime.UtcNow;

                _context.Projects.Update(existingProject);
                await _context.SaveChangesAsync();

                return Success(existingProject, "Project updated successfully");
            }
            catch (Exception ex)
            {
                return Error<Project>($"Error updating project: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteProject(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null)
                {
                    return Error<bool>("Project not found");
                }

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                return Success(true, "Project deleted successfully");
            }
            catch (Exception ex)
            {
                return Error<bool>($"Error deleting project: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<Paragraph>>> GetProjectParagraphs(int projectId)
        {
            try
            {
                var paragraphs = await _context.Paragraphs
                    .Where(p => p.ProjectId == projectId)
                    .OrderBy(p => p.Order)
                    .ToListAsync();

                return Success(paragraphs);
            }
            catch (Exception ex)
            {
                return Error<List<Paragraph>>($"Error retrieving paragraphs: {ex.Message}");
            }
        }

        public async Task<ServiceResult<ProjectAssignment>> AssignUserToProject(int projectId, ProjectAssignmentDto assignment)
        {
            try
            {
                var project = await _context.Projects.FindAsync(projectId);
                if (project == null)
                {
                    return Error<ProjectAssignment>("Project not found");
                }

                var user = await _context.Users.FindAsync(assignment.UserId);
                if (user == null)
                {
                    return Error<ProjectAssignment>("User not found");
                }

                var projectAssignment = new ProjectAssignment
                {
                    Id = Guid.NewGuid().ToString(),
                    ProjectId = projectId,
                    UserId = assignment.UserId,
                    AssignmentType = assignment.AssignmentType,
                    AssignedAt = DateTime.UtcNow
                };

                _context.ProjectAssignments.Add(projectAssignment);
                await _context.SaveChangesAsync();

                return Success(projectAssignment, "User assigned to project successfully");
            }
            catch (Exception ex)
            {
                return Error<ProjectAssignment>($"Error assigning user to project: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<Project>>> GetUserProjects(string userId)
        {
            try
            {
                var userProjects = await _context.ProjectAssignments
                    .Where(pa => pa.UserId == userId)
                    .Include(pa => pa.Project)
                        .ThenInclude(p => p.Paragraphs)
                    .Select(pa => pa.Project)
                    .Distinct()
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();

                return Success(userProjects);
            }
            catch (Exception ex)
            {
                return Error<List<Project>>($"Error retrieving user projects: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Project>> UpdateProjectStatus(int projectId, string status)
        {
            try
            {
                var project = await _context.Projects.FindAsync(projectId);
                if (project == null)
                {
                    return Error<Project>("Project not found");
                }

                if (Enum.TryParse<ProjectStatus>(status, true, out var projectStatus))
                {
                    project.Status = projectStatus;
                    project.UpdatedAt = DateTime.UtcNow;

                    _context.Projects.Update(project);
                    await _context.SaveChangesAsync();

                    return Success(project, "Project status updated successfully");
                }
                else
                {
                    return Error<Project>("Invalid status value");
                }
            }
            catch (Exception ex)
            {
                return Error<Project>($"Error updating project status: {ex.Message}");
            }
        }

        public Task<ServiceResult<Project>> CreateProject(ProjectCreateDto projectCreateDto)
        {
            throw new NotImplementedException();
        }
    }
}