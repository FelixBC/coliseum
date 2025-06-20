using Coliseum.App.ViewModels;

namespace Coliseum.App.Views;

public partial class FeedPage : ContentPage
{
    public FeedPage()
    {
        InitializeComponent();
        BindingContext = App.ServiceProvider.GetRequiredService<FeedViewModel>();
    }
}
