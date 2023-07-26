using LingoLibrary.ApiManagers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LingoApp;

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
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
	builder.Services.AddBlazorWebViewDeveloperTools();
	builder.Logging.AddDebug();
#endif

		//builder.Services.AddSingleton<WeatherForecastService>();
		builder.Configuration.AddUserSecrets("088c9a1e-5a94-4e3f-9824-9ba0213dba14");
		var app = builder.Build();

		return app;
	}
}