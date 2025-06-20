using Coliseum.App.ViewModels;

namespace Coliseum.App.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage()
    {
        InitializeComponent();
        BindingContext = App.ServiceProvider.GetRequiredService<SearchViewModel>();
    }
}
