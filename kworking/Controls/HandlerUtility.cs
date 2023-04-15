
namespace kworking.Controls;

public class HandlerUtility
{
    public static void ModifyEntry()
    {
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
#if ANDROID
                handler.PlatformView.Background = null;
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif IOS || MACCATALYST
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth= 0;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            }

        });
    }
}
