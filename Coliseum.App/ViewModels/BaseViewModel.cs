using CommunityToolkit.Mvvm.ComponentModel;

namespace Coliseum.App.ViewModels;

public abstract class BaseViewModel : ObservableObject
{
    private bool _isBusy;
    
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public BaseViewModel()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
    }
}
