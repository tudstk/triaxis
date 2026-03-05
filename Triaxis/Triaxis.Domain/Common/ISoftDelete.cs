namespace Triaxis.Domain.Common;

public interface ISoftDelete
{
    DateTime? DeletedAt { get; set; }
    Guid? DeletedByUserId { get; set; }
}
