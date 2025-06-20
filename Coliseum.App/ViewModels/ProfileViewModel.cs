using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Coliseum.App.Models;
using Coliseum.App.Services;

namespace Coliseum.App.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private ObservableCollection<Post> _posts;
    private User _user;
    private bool _isRefreshing;
    private Guid _userId;

    public ObservableCollection<Post> Posts
    {
        get => _posts;
        set => SetProperty(ref _posts, value);
    }

    public User User
    {
        get => _user;
        set => SetProperty(ref _user, value);
    }

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }

    public ProfileViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Posts = new ObservableCollection<Post>();
    }

    public void Initialize(Guid userId)
    {
        _userId = userId;
        LoadProfileAsync().FireAndForget();
    }

    private async Task LoadProfileAsync()
    {
        try
        {
            IsBusy = true;
            User = await _apiService.GetUserAsync(_userId);
            Posts.Clear();
            var userPosts = await _apiService.GetUserPostsAsync(_userId);
            foreach (var post in userPosts)
            {
                Posts.Add(post);
            }
        }
        catch (Exception ex)
        {
            // Handle error
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadProfileAsync();
    }

    [RelayCommand]
    private async Task PostSelected(Post post)
    {
        if (post == null) return;

        await Shell.Current.GoToAsync($"//PostDetail?postId={post.Id}");
    }
}
