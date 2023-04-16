using System.Net;

namespace Domain.SharedDomain;

public class DomainException : ArgumentException
{
    public HttpStatusCode Reason { get; init; }

    internal DomainException(DomainError error) : base(error.ErrorMessage)
    {
        Reason = error.Reason;
    }
}