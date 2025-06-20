using Coliseum.App.ViewModels;
using Coliseum.App.Models;

namespace Coliseum.App.Views;

public partial class PostDetailPage : ContentPage
{
    public PostDetailPage(PostDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void HostAvatar_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is PostDetailViewModel viewModel && viewModel.Post?.Host?.Id != null)
        {
            Shell.Current.GoToAsync($"//ProfilePage?userId={viewModel.Post.Host.Id}");
        }
    }

    private void HostName_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is PostDetailViewModel viewModel && viewModel.Post?.Host?.Id != null)
        {
            Shell.Current.GoToAsync($"//ProfilePage?userId={viewModel.Post.Host.Id}");
        }
    }
}
