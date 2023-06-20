using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Instagram.Concrete.Development;

internal class FakeUserManager : IUserManager
{
    private readonly HttpClient _httpClient;

    public FakeUserManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent,
            "Instagram 76.0.0.15.395 Android (24/7.0; 640dpi; 1440x2560; samsung; SM-G930F; herolte; samsungexynos8890; en_US; 138226743)");
    }

    // TODO: Add escaping here
    public async Task<User> GetUserById(string id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"https://i.instagram.com/api/v1/users/{id}/info/", cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var result =  JsonConvert.DeserializeObject<UserResponse>(content);
        return await GetUserByUsername(result.User!.Username, cancellationToken);
    }

    public async Task<User> GetUserByUsername(string username, CancellationToken cancellationToken)
    {
        // https://www.instagram.com/cristiano/?__a=1&__d=dis
        // https://www.instagram.com/api/v1/users/web_profile_info/?username={username}
        var response = await _httpClient.GetAsync($"https://www.instagram.com/{username}/?__a=1&__d=dis", cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        content = content.Replace("\"id\"", "\"pk\"");
        var result =  JsonConvert.DeserializeObject<UserResponse>(content);
        var user = result.Graphql!.User;
        return new User()
        {
            Pk = user.Pk,
            Username = user.Username
        };
    }
}

class UserResponse
{
    public UserShort User { get; set; } = null!;
    public UserWrapper Graphql { get; set; }    
}

class UserWrapper
{
    public UserShort User { get; set; } = null!;
}