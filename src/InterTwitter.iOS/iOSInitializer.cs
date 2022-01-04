using InterTwitter.Helpers;
using InterTwitter.iOS.Renderers;
using InterTwitter.iOS.Services.PermissionsService;
using InterTwitter.iOS.Services.VideoProcessing;
using InterTwitter.Services;
using InterTwitter.Services.VideoProcessing;
using Prism;
using Prism.Ioc;

namespace InterTwitter.iOS
{
    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IPermissionService, PermissionService>();
            containerRegistry.RegisterSingleton<IVideoProcessingService, VideoProcessingService>();
            containerRegistry.RegisterSingleton<IKeyboardHelper, KeyboardHelper>();
        }
    }
}