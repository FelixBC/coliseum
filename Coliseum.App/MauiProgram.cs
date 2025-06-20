using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Coliseum.App.Services;

namespace Coliseum.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		// Register ApiService with HttpClient
		builder.Services.AddHttpClient<IApiService, ApiService>(client =>
		{
			// For Android emulator
			client.BaseAddress = new Uri("https://10.0.2.2:5001/");
		});

		return builder.Build();
	}
}
