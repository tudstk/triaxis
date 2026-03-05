namespace Triaxis.Domain.Entities;

public class StudySettingsData
{
    public bool EnableRandomization { get; set; }
    public bool EnableDoseManagement { get; set; }
    public bool EnableCohorts { get; set; }
    public bool EnableDrugManagement { get; set; }
    public bool EnableUnblinding { get; set; }
    public bool AllowRescreening { get; set; }
    public bool RequireWeightAtVisit { get; set; }
    public string SubjectNumberFormat { get; set; } = "{SiteCode}-{000}";
    public int SubjectNumberLength { get; set; } = 3;
    public string SubjectNumberAssignmentTrigger { get; set; } = "Screened";

    public static StudySettingsData WithDefaults() => new();
}
