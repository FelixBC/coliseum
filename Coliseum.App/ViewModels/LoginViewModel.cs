using System.Security;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Coliseum.App.Services;

namespace Coliseum.App.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private string _email;
    private SecureString _password;

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public SecureString Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public LoginViewModel(IApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || Password == null || Password.Length == 0)
        {
            return;
        }

        try
        {
            IsBusy = true;
            var response = await _apiService.LoginAsync(Email, Password);
            await SecureStorage.SetAsync("jwt_token", response.Token);
            Shell.Current.GoToAsync("//FeedPage");
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
    private async Task RegisterAsync()
    {
        await Shell.Current.GoToAsync("//RegisterPage");
    }
}
