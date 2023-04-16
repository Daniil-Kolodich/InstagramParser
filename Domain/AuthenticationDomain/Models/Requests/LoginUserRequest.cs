namespace Domain.AuthenticationDomain.Models.Requests;

public record LoginUserRequest(string Email, string Password);