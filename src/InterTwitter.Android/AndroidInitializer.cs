using InterTwitter.Droid.Renderers;
using InterTwitter.Droid.Services.PermissionsService;
using InterTwitter.Droid.Services.VideoProcessing;
using InterTwitter.Helpers;
using InterTwitter.Services;
using InterTwitter.Services.VideoProcessing;
using Prism;
using Prism.Ioc;

namespace InterTwitter.Droid
{
    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IPermissionService, PermissionService>();
            containerRegistry.RegisterSingleton<IVideoProcessingService, VideoProcessingService>();
            containerRegistry.RegisterSingleton<IKeyboardHelper, KeyboardHelper>();
        }
    }
}
