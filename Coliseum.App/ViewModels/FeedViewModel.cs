using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Coliseum.App.Models;
using Coliseum.App.Services;

namespace Coliseum.App.ViewModels;

public partial class FeedViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private ObservableCollection<Post> _posts;
    private bool _isRefreshing;

    public ObservableCollection<Post> Posts
    {
        get => _posts;
        set => SetProperty(ref _posts, value);
    }

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }

    public FeedViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Posts = new ObservableCollection<Post>();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        try
        {
            var posts = await _apiService.GetFeedAsync();
            Posts.Clear();
            foreach (var post in posts)
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
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task PostSelected(Post post)
    {
        if (post == null) return;

        // Navigate to PropertyDetailPage
        await Shell.Current.GoToAsync($"//PostDetail?postId={post.Id}");
    }

    protected override async void Initialize()
    {
        await RefreshAsync();
    }
}
