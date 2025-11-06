namespace TranslationSystemBackend.Models.Enums;

public enum ProjectStatus
{
    Created,        // 0
    FileUploaded,   // 1
    ParagraphsSplit,// 2
    InTranslation,  // 3
    InReview,       // 4
    PendingApproval,// 5
    Completed       // 6
}