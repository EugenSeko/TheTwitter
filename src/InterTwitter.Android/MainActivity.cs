using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using FFImageLoading.Forms.Platform;
using InterTwitter.Droid.Renderers;
using InterTwitter.Helpers;
using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;

namespace InterTwitter.Droid
{
    [Activity(
        Label = "@string/ApplicationName", 
        Icon = "@mipmap/icon_owl",
        LaunchMode = LaunchMode.SingleTask, 
        Theme = "@style/MainTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, 
        ScreenOrientation = ScreenOrientation.Portrait)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        #region -- Overrides --

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Rg.Plugins.Popup.Popup.Init(this);
            CachedImageRenderer.Init(true);
            CachedImageRenderer.InitImageViewHandler();
            KeyboardHelper.Init(this);

            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion
    }
}