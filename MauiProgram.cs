using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using projeto_pi.Services;
using System.Reflection;
using System.Text.Json;

namespace projeto_pi
{
    public static class MauiProgram
    {
        private static IServiceProvider? _services;
        public static IServiceProvider Services => _services!;


        public static IConfiguration Configuration { get; private set; }


        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("projeto_pi.appsettings.json");
            var nomes = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach (var nome in nomes)
            {
                Console.WriteLine("RECURSO: " + nome);
            }
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream!)
                .Build();

            builder.Configuration.AddConfiguration(config);
            var baseUrl = config["BaseUrl"];



            builder.Services.AddSingleton(_ => new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            });
            builder.Services.AddSingleton<ApiService>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<QuizService>();
            Configuration = config;

            builder.Services.AddSingleton<IConfiguration>(Configuration);
            //builder.Services.AddSingleton<IServiceProvider>(Services);

#if DEBUG
            builder.Logging.AddDebug();
#endif
            var app= builder.Build();
            _services = app.Services;

            return app;
        }
    }
}
