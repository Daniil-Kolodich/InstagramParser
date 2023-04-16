using System.Net;

namespace Domain.SharedDomain;

internal sealed record DomainError(string ErrorMessage, HttpStatusCode Reason)
{
    public static implicit operator DomainException(DomainError error) => new DomainException(error);
}
