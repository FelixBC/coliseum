using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Coliseum.App.Models;
using Coliseum.App.Services;

namespace Coliseum.App.ViewModels;

public partial class PostDetailViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private Post _post;
    private bool _isLiked;

    public Post Post
    {
        get => _post;
        set => SetProperty(ref _post, value);
    }

    public bool IsLiked
    {
        get => _isLiked;
        set => SetProperty(ref _isLiked, value);
    }

    public PostDetailViewModel(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task LoadPostAsync(Guid postId)
    {
        try
        {
            IsBusy = true;
            Post = await _apiService.GetPostAsync(postId);
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
    private async Task LikeAsync()
    {
        try
        {
            IsBusy = true;
            await _apiService.LikePostAsync(Post.Id);
            IsLiked = !IsLiked;
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
}
