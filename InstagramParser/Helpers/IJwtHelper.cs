namespace InstagramParser.Helpers;

public interface IJwtHelper
{
    public string GenerateToken(string userId);
}