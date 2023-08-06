﻿using Data;
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
		// App Settings
		var exec = System.Reflection.Assembly.GetExecutingAssembly().Location;
		var path = System.IO.Path.GetDirectoryName(exec);
		AppDomain.CurrentDomain.SetData("DataDirectory", path);

		//builder.Configuration.AddJsonFile("appsettings.json");
		var a = System.Reflection.Assembly.GetExecutingAssembly();
		using var stream = a.GetManifestResourceStream("LingoApp.appsettings.json");
		var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
		builder.Configuration.AddConfiguration(config);

		// APIs
		builder.Services.AddSingleton<TmdbManager>();
		builder.Services.AddSingleton<OpenSubtitlesManager>();
		builder.Services.AddScoped<OpenAiManager>();

		// Database
		builder.Services.AddScoped<LiteDbDataAccess>();
		builder.Services.AddScoped<SetData>();
		builder.Services.AddScoped<SerieSearchData>();

		return builder.Build();
	}
}