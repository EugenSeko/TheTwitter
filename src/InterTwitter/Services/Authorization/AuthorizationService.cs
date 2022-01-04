using InterTwitter.Helpers.ProcessHelpers;
using InterTwitter.Models;
using System;
using System.Threading.Tasks;

namespace InterTwitter.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IMockService _mockService;
        private readonly ISettingsManager _settingsManager;

        public AuthorizationService(
            IMockService mockService,
            ISettingsManager settingsManager)
        {
            _mockService = mockService;
            _settingsManager = settingsManager;
        }

        #region -- Public helpers --

        public async Task<AOResult<UserModel>> CheckUserAsync(string email, string password)
        {
            var result = new AOResult<UserModel>();

            try
            {
                var user = await _mockService.FindAsync<UserModel>(x => x.Email.ToLower() == email.ToLower() && x.Password == password);
                if (user != null)
                {
                    result.SetSuccess(user);
                }
                else
                {
                    result.SetFailure();
                }
            }
            catch (Exception ex)
            {
                result.SetError($"Exception: {nameof(CheckUserAsync)}", "Wrong result", ex);
            }

            return result;
        }
        #endregion
    }
}
