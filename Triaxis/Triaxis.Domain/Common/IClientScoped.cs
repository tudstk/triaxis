namespace Triaxis.Domain.Common;

public interface IClientScoped
{
    Guid ClientId { get; }
}
