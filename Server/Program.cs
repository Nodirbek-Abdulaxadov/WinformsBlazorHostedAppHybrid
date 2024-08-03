using ActualLab.Fusion;
using ActualLab.Fusion.Server;
using ActualLab.Rpc.Server;
using Blazored.LocalStorage;
using Client;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using MongoDbCore;
using MudBlazor;
using MudBlazor.Services;
using Server.Components;
using Services;

namespace Server;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        var options = new WebApplicationOptions
        {
            Args = args,
            ContentRootPath = AppContext.BaseDirectory,
            WebRootPath = "wwwroot"
        };
        var builder = WebApplication.CreateBuilder(options);
        var services = builder.Services;
        var cfg = builder.Configuration;
        var env = builder.Environment;

        services.AddMongoDbContext<AppDbContext>(cfg.GetSection("MongoDB").Get<MongoDbCoreOptions>()!);


        #region ActualLab.Fusion
        //IComputedState.DefaultOptions.FlowExecutionContext = true;
        services.AddFusionServices();
        services.AddScoped<UInjector>();
        #endregion

        #region MudBlazor and Pages
        services.AddRazorPages();
        services.AddServerSideBlazor(o => o.DetailedErrors = true);
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 10000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });
        #endregion

        #region Localization
        services.AddLocalization();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[] { "uz-Latn", "en-US", "ru-RU" };
            options.DefaultRequestCulture = new RequestCulture("uz-Latn");
            options.AddSupportedCultures(supportedCultures);
            options.AddSupportedUICultures(supportedCultures);
        });
        #endregion

        #region UI Services
        services.AddBlazoredLocalStorage();
        services.AddScoped<PageHistoryState>();
        #endregion

        builder.WebHost.UseWebRoot("wwwroot");
        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        #region Loggin Proxy Socket
        //app.UseHttpLogging();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedProto
        });

        app.UseWebSockets(new WebSocketOptions()
        {
            KeepAliveInterval = TimeSpan.FromSeconds(30)
        });
        #endregion

        app.UseFusionSession();
        app.UseRequestLocalization();

        app.UseResponseCaching();
        //app.UseResponseCompression();
        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

        #region Mapping
        app.MapRazorPages();
        app.MapControllers();
        app.MapBlazorHub();
        app.MapRpcWebSocketServer();
        //app.MapFallbackToPage("/_Host");
        #endregion

        Task.Run(() => app.Run());

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}