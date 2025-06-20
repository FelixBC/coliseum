using System.Net.Http.Json;
using Coliseum.App.Models;
using System.Net.Http.Headers;
using System.Security;
using System.Net;

namespace Coliseum.App.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AuthResponse> LoginAsync(string email, SecureString password)
    {
        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = new NetworkCredential("", password).Password
        };

        var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginRequest);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AuthResponse>()!;
    }

    public async Task<AuthResponse> RegisterAsync(string email, SecureString password, string name, string bio)
    {
        var registerRequest = new RegisterRequest
        {
            Email = email,
            Password = new NetworkCredential("", password).Password,
            Name = name,
            Bio = bio
        };

        var response = await _httpClient.PostAsJsonAsync("/api/auth/register", registerRequest);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AuthResponse>()!;
    }

    private async Task SetupAuthentication()
    {
        if (string.IsNullOrEmpty(_httpClient.DefaultRequestHeaders.Authorization?.Parameter))
        {
            var token = await SecureStorage.GetAsync("jwt_token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }

    public async Task<List<Post>> GetFeedAsync()
    {
        await SetupAuthentication();
        var response = await _httpClient.GetAsync("/api/posts");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Post>>() ?? new List<Post>();
    }

    public async Task<List<Post>> SearchPostsAsync(string query)
    {
        await SetupAuthentication();
        var response = await _httpClient.GetAsync($"/api/posts/Search?q={query}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Post>>() ?? new List<Post>();
    }

    public async Task<Post> GetPostAsync(Guid id)
    {
        await SetupAuthentication();
        var response = await _httpClient.GetAsync($"/api/posts/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Post>()!;
    }

    public async Task<User> GetUserAsync(Guid id)
    {
        await SetupAuthentication();
        var response = await _httpClient.GetAsync($"/api/users/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<User>()!;
    }

    public async Task<List<Post>> GetUserPostsAsync(Guid userId)
    {
        await SetupAuthentication();
        var response = await _httpClient.GetAsync($"/api/users/{userId}/posts");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Post>>() ?? new List<Post>();
    }

    public async Task LikePostAsync(Guid postId)
    {
        await SetupAuthentication();
        var response = await _httpClient.PostAsync($"/api/posts/{postId}/Like", null);
        response.EnsureSuccessStatusCode();
    }
}
