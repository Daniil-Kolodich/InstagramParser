namespace Domain.AuthenticationDomain.Models.Responses;

public record LoginUserResponse(int Id)
{
    private LoginUserResponse() : this(0)
    {
    }
}