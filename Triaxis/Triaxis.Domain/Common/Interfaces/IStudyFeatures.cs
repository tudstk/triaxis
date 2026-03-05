namespace Triaxis.Domain.Common.Interfaces;

public interface IStudyFeatures
{
    bool IsEnabled(Guid studyId, string featureKey);
}
