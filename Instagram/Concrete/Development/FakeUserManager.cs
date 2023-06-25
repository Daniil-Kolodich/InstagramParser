using System.Net.Http.Json;
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
        var response = await _httpClient.GetAsync($"https://www.instagram.com/{username}/?__a=1&__d=dis",
            cancellationToken);
        response.EnsureSuccessStatusCode();

        var result =  JsonConvert.DeserializeObject<UserResponse>(await response.Content.ReadAsStringAsync(cancellationToken));
        var user = result.Graphql!.User;
        return new User
        {
            Pk = user.Key(),
            Username = user.Username,
            FullName = user.FullName,
            FollowingCount = user.FollowingCount.Count,
            FollowerCount = user.FollowerCount.Count
        };
    }
}

class UserResponse
{
    public FakeUser? User { get; set; }
    public UserWrapper? Graphql { get; set; }    
}

class UserWrapper
{
    public FakeUser User { get; set; } = null!;
}