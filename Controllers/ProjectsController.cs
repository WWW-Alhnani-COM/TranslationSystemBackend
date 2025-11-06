using Microsoft.AspNetCore.Mvc;
using TranslationDB.Models;
using TranslationDB.Models.DTOs;
using TranslationDB.Services.Interfaces;

namespace TranslationDB.Controllers
{
    public class ProjectsController : BaseController
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                var result = await _projectService.GetAllProjects();
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving projects: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            try
            {
                var result = await _projectService.GetProjectById(id);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving project: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectCreateDto projectCreateDto)
        {
            try
            {
                var result = await _projectService.CreateProject(projectCreateDto);
                if (!result.Success)
                    return HandleError(result.Message);

                return CreatedAtAction(nameof(GetProjectById), new { id = result.Data.Id }, 
                    new { success = true, data = result.Data, message = "Project created successfully" });
            }
            catch (Exception ex)
            {
                return HandleError($"Error creating project: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            try
            {
                if (id != project.Id)
                    return HandleError("ID mismatch");

                var result = await _projectService.UpdateProject(project);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "Project updated successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error updating project: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                var result = await _projectService.DeleteProject(id);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(null, "Project deleted successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error deleting project: {ex.Message}");
            }
        }

        [HttpGet("{id}/paragraphs")]
        public async Task<IActionResult> GetProjectParagraphs(int id)
        {
            try
            {
                var result = await _projectService.GetProjectParagraphs(id);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving paragraphs: {ex.Message}");
            }
        }

        [HttpPost("{id}/assign")]
        public async Task<IActionResult> AssignUserToProject(int id, [FromBody] ProjectAssignmentDto assignment)
        {
            try
            {
                var result = await _projectService.AssignUserToProject(id, assignment);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "User assigned to project successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error assigning user to project: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserProjects(string userId)
        {
            try
            {
                var result = await _projectService.GetUserProjects(userId);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving user projects: {ex.Message}");
            }
        }

        [HttpPost("{id}/update-status")]
        public async Task<IActionResult> UpdateProjectStatus(int id, [FromBody] UpdateStatusDto statusDto)
        {
            try
            {
                var result = await _projectService.UpdateProjectStatus(id, statusDto.Status);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "Project status updated successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error updating project status: {ex.Message}");
            }
        }
    }
}