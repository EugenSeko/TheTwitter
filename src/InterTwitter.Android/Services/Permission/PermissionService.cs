using InterTwitter.Services;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace InterTwitter.Droid.Services.PermissionsService
{
    public class PermissionService : IPermissionService
    {
        public async Task<PermissionStatus> CheckStatusAsync<T>() where T : Permissions.BasePermission, new()
        {
            return await Permissions.CheckStatusAsync<T>();
        }

        public async Task<PermissionStatus> RequestAsync<T>() where T : Permissions.BasePermission, new()
        {
            var status = await CheckStatusAsync<T>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<T>();
            }

            return status;
        }
    }
}