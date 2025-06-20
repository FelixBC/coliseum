using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Coliseum.App.Models;
using Coliseum.App.Services;

namespace Coliseum.App.ViewModels;

public partial class SearchViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private ObservableCollection<Post> _posts;
    private string _searchQuery;
    private bool _isRefreshing;

    public ObservableCollection<Post> Posts
    {
        get => _posts;
        set => SetProperty(ref _posts, value);
    }

    public string SearchQuery
    {
        get => _searchQuery;
        set => SetProperty(ref _searchQuery, value);
    }

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }

    public SearchViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Posts = new ObservableCollection<Post>();
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery)) return;

        IsRefreshing = true;
        try
        {
            var posts = await _apiService.SearchPostsAsync(SearchQuery);
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
    private async Task RefreshAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery)) return;
        await SearchAsync();
    }

    [RelayCommand]
    private async Task PostSelected(Post post)
    {
        if (post == null) return;

        await Shell.Current.GoToAsync($"//PostDetail?postId={post.Id}");
    }
}
