using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coliseum.App.Models;
using System.Security;

namespace Coliseum.App.Services;

public interface IApiService
{
    Task<AuthResponse> LoginAsync(string email, SecureString password);
    Task<AuthResponse> RegisterAsync(string email, SecureString password, string name, string bio);
    Task<List<Post>> GetFeedAsync();
    Task<List<Post>> SearchPostsAsync(string query);
    Task<Post> GetPostAsync(Guid id);
    Task<User> GetUserAsync(Guid id);
    Task<List<Post>> GetUserPostsAsync(Guid userId);
    Task LikePostAsync(Guid postId);
}
