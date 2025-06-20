using Coliseum.App.ViewModels;

namespace Coliseum.App.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = App.ServiceProvider.GetRequiredService<ProfileViewModel>();
    }
}
