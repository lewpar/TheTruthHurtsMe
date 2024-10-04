using TheTruthHurtsMe.Components;
using TheTruthHurtsMe.Configuration;
using TheTruthHurtsMe.Services;

namespace TheTruthHurtsMe;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        await DotEnv.LoadAsync(Environment.CurrentDirectory);
        
        DotEnv.Ensure("OPENAPI_KEY");
        DotEnv.Ensure("SPOTIFY_ID");
        DotEnv.Ensure("SPOTIFY_SECRET");
        
        await ConfigureServicesAsync(builder.Services);
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }

    private static async Task ConfigureServicesAsync(IServiceCollection services)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        var openApiKey = DotEnv.Get("OPENAPI_KEY");
        
        services.AddSingleton(new GPTService(openApiKey));
        
        var spotifyClientId = DotEnv.Get("SPOTIFY_ID");
        var spotifySecret = DotEnv.Get("SPOTIFY_SECRET");
        
        var spotifyService = new SpotifyService();
        await spotifyService.InitializeAsync(spotifyClientId, spotifySecret);
        services.AddSingleton(spotifyService);

        var quizService = new QuizService();
        await quizService.LoadQuizAsync();
        services.AddSingleton(quizService);
    }
}