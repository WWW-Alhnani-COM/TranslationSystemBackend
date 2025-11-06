namespace TranslationDB.Models.DTOs
{
    public class ProjectAssignmentDto
    {
        public string UserId { get; set; }
        public string AssignmentType { get; set; } // "Translator", "Reviewer"
    }

    public class UpdateStatusDto
    {
        public string Status { get; set; }
    }

    public class LockParagraphDto
    {
        public string UserId { get; set; }
    }

    public class RevisionRequestDto
    {
        public string Comments { get; set; }
    }

    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}