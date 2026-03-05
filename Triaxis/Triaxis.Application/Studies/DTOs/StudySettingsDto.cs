namespace Triaxis.Application.Studies.DTOs;

public record StudySettingsDto(
    bool EnableRandomization,
    bool EnableDoseManagement,
    bool EnableCohorts,
    bool EnableDrugManagement,
    bool EnableUnblinding,
    bool AllowRescreening,
    bool RequireWeightAtVisit,
    string SubjectNumberFormat,
    int SubjectNumberLength,
    string SubjectNumberAssignmentTrigger);
