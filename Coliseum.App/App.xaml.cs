using Microsoft.Extensions.DependencyInjection;
using Coliseum.App.ViewModels;

namespace Coliseum.App;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// Register services
		ServiceProvider = RegisterServices();
	}

	private IServiceProvider RegisterServices()
	{
		var services = new ServiceCollection();

		// Register view models
		services.AddSingleton<FeedViewModel>();
		services.AddSingleton<SearchViewModel>();
		services.AddSingleton<ProfileViewModel>();

		return services.BuildServiceProvider();
	}

	public static IServiceProvider ServiceProvider { get; private set; }

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}