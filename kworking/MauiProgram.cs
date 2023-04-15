using kworking.Controls;
using Microsoft.Extensions.Logging;

namespace kworking;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("Montserrat-ExtraLightItalic.ttf", "OpenSansRegular");
				fonts.AddFont("Montserrat-ExtraLightItalic.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIconsSharp-Regular.otf", "MI");
                fonts.AddFont("MaterialIcons-Regular.ttf", "IconFontTypes");
                fonts.AddFont("Montserrat-ExtraLightItalic.ttf", "Mont");
            });
        Controls.HandlerUtility.ModifyEntry();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddSingleton<ChatHub>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<ListChatPage>();
        builder.Services.AddSingleton<ChatPage>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<ListChatViewModel>();
        builder.Services.AddSingleton<ChatViewModel>();
        builder.Services.AddSingleton<ServiceProvider>();
        builder.Services.AddSingleton<ProfilePage>();
        builder.Services.AddSingleton<ProfileViewModel>();
        builder.Services.AddSingleton<OrdersPage>();
        builder.Services.AddSingleton<OrdersViewModel>();
        builder.Services.AddSingleton<NewOrderPage>();
        builder.Services.AddSingleton<NewOrderViewModel>();
        builder.Services.AddSingleton<StockPage>();
        builder.Services.AddSingleton<StockViewModel>();
        return builder.Build();
	}
}
