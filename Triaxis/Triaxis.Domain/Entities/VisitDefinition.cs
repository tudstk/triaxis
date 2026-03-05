using Triaxis.Domain.Common;
using Triaxis.Domain.Common.Enums;

namespace Triaxis.Domain.Entities;

public class VisitDefinition : BaseEntity
{
    public Guid? StudyId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public VisitType VisitType { get; private set; }
    public bool IsDefault { get; private set; }
    public bool IsEnabled { get; private set; } = true;
    public int Order { get; private set; }
    public int? WindowDayTarget { get; set; }
    public int? WindowDayMinus { get; set; }
    public int? WindowDayPlus { get; set; }

    private VisitDefinition() { }

    public static VisitDefinition CreateDefault(string name, string code, VisitType visitType, int order)
    {
        return new VisitDefinition
        {
            Name = name,
            Code = code,
            VisitType = visitType,
            IsDefault = true,
            IsEnabled = true,
            Order = order,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static VisitDefinition CreateForStudy(Guid studyId, string name, string code, VisitType visitType, int order)
    {
        return new VisitDefinition
        {
            StudyId = studyId,
            Name = name,
            Code = code,
            VisitType = visitType,
            IsDefault = false,
            IsEnabled = true,
            Order = order,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Enable() { IsEnabled = true; UpdatedAt = DateTime.UtcNow; }
    public void Disable() { IsEnabled = false; UpdatedAt = DateTime.UtcNow; }

    public void UpdateWindow(int target, int minus, int plus)
    {
        WindowDayTarget = target;
        WindowDayMinus = minus;
        WindowDayPlus = plus;
        UpdatedAt = DateTime.UtcNow;
    }
}
