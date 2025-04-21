using CommunityToolkit.Maui;
using Controls.UserDialogs.Maui;
using CovrMe.Boots;
using CovrMe.Handlers;
using CovrMe.Views.Pages.Insurances;
using CovrMe.Views.Pages;
using CovrMe;
using Dynamsoft.CameraEnhancer.Maui;
using Dynamsoft.CameraEnhancer.Maui.Handlers;
using CovrMe.Boots;
using CovrMe.Handlers;
using CovrMe.Views.Pages;
using CovrMe.Views.Pages.Insurances;
using Maui.Plugins.PageResolver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
namespace CovrMe
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            #region Settings

            var assembly = typeof(Settings.Settings).Assembly;
            using var stream = assembly.GetManifestResourceStream("CovrMe.Settings.appsettings.json");

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            builder.Configuration.AddConfiguration(config);

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .Build();
            var settings = config.GetRequiredSection("Settings").Get<Settings.Settings>();

            App.IsLocalhost = (settings.BackendServiceEndpoint.Contains("localhost") || settings.BackendServiceEndpoint.Contains("10.0.2.2")) ? true : false;

            #endregion

            builder
             .UseMauiApp<App>()
             .UsePageResolver()
             .UseMauiCommunityToolkit()
             .UseUserDialogs(true, () =>
             {
#if ANDROID
                 var fontFamily = "OpenSans-Default.ttf";
#else
                    var fontFamily = "OpenSans-Regular";
#endif

                 HudDialogConfig.DefaultLoaderColor = Color.FromHex("#823189");
             })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
                            .ConfigureLifecycleEvents(events =>
                            {
#if ANDROID
                                events.AddAndroid(android => android
                                    .OnResume((activity) =>
                                    {
                                        CameraPage.enhancer?.Open();

                                    })
                    );
#endif
                            })
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(CameraView), typeof(CameraViewHandler));
#if IOS
                handlers.AddHandler<Entry, Handlers.EntryHandler>();
#endif
            })
            .ConfigureEssentials(essentials =>
            {
                essentials.UseVersionTracking();
            });


            #region Services

            builder.Services.RegisterServices(settings);

            #endregion

            #region Views
            BorderlessEntry.RemoveBorders();
            //builder.Services.RegisterViews();

#if IOS

            builder.Services.RegisterViewsiOS();

#else

            builder.Services.RegisterViewsAndroid();

#endif

            #endregion

            #region Routes

            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));

            #endregion
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
