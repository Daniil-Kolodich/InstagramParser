namespace Domain.AuthenticationDomain.Models.Requests;

public record RegisterUserRequest(string UserName, string Email, string Password);