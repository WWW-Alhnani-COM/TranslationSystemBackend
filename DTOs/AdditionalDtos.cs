namespace TranslationDB.Models.DTOs
{
    public class ProjectAssignmentDto
    {
        public string UserId { get; set; } = string.Empty;
        public string AssignmentType { get; set; } = string.Empty;
    }

    public class UpdateStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }

    public class LockParagraphDto
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class RevisionRequestDto
    {
        public string Comments { get; set; } = string.Empty;
    }

    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; } = default!;
        public string Message { get; set; } = string.Empty;
    }
}
