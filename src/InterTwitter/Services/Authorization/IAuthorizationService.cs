using InterTwitter.Helpers.ProcessHelpers;
using InterTwitter.Models;
using System.Threading.Tasks;

namespace InterTwitter.Services
{
    public interface IAuthorizationService
    {
        Task<AOResult<UserModel>> CheckUserAsync(string login, string password);
    }
}
