namespace Domain.AuthenticationDomain.Models.Responses;

public record RegisterUserResponse(int Id)
{
    private RegisterUserResponse() : this(0)
    {
    }
}