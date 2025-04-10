#if ANDROID
using AndroidX.Core.Content;
using Android.Graphics;
using Microsoft.Maui.Platform;
#endif

using Microsoft.Maui.Handlers;

namespace To_do_part_3.Handlers;

public class NoUnderlineEntryHandler : EntryHandler
{
#if ANDROID
    protected override AndroidX.AppCompat.Widget.AppCompatEditText CreatePlatformView()
    {
        var view = base.CreatePlatformView();
        view.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
        return view;
    }
#endif
}