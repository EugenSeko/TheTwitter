﻿using System;
using System.Linq;
using System.Threading.Tasks;
using InterTwitter.Helpers.ProcessHelpers;
using InterTwitter.Models;
using InterTwitter.Services.Registration;
using Xamarin.Essentials;

namespace InterTwitter.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private IRegistrationService _registrationService;

        public AuthorizationService(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        #region -- Public properties --

        public int UserId
        {
            get => Preferences.Get(nameof(UserId), 0);
            set => Preferences.Set(nameof(UserId), value);
        }

        #endregion

        #region -- Public helpers --

        public async Task<AOResult<UserModel>> CheckUserAsync(string email, string password)
        {
            var result = new AOResult<UserModel>();

            try
            {
                var user = _registrationService?.GetUsers()?.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
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

        public async Task<AOResult<UserModel>> GetCurrentUserAsync()
        {
            return await _registrationService?.GetByIdAsync(UserId);
        }

        #endregion
    }
}
