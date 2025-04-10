using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using To_do_part_3.Handlers;

namespace To_do_part_3;

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
            })
            .ConfigureMauiHandlers(handlers =>
            {
                // Remove Editor underline for all platforms
#if ANDROID || IOS
                handlers.AddHandler(typeof(Editor), typeof(NoUnderlineEditorHandler));
                handlers.AddHandler(typeof(Entry), typeof(NoUnderlineEntryHandler));
#endif
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}