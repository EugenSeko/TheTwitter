using InterTwitter.Helpers.ProcessHelpers;
using InterTwitter.Models;
using InterTwitter.Resources.Strings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InterTwitter.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IMockService _mockService;

        public RegistrationService(IMockService mockService)
        {
            _mockService = mockService;
        }

        #region -- Public helpers --

        public async Task<AOResult> CheckTheCorrectEmailAsync(string email)
        {
            var result = new AOResult();
            try
            {
                var user = await _mockService.FindAsync<UserModel>(x => x.Email.ToLower() == email.ToLower());
                if (user != null)
                {
                    result.SetSuccess();
                }
                else
                {
                    result.SetFailure();
                }
            }
            catch (Exception ex)
            {
                result.SetError($"Exception: {nameof(CheckTheCorrectEmailAsync)}", Strings.AlertDatabase, ex);
            }

            return result;
        }

        public async Task<AOResult<UserModel>> GetByIdAsync(int id)
        {
            var result = new AOResult<UserModel>();
            try
            {
                var user = await _mockService.GetByIdAsync<UserModel>(id);
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
                result.SetError($"Exception: {nameof(GetByIdAsync)}", Strings.AlertDatabase, ex);
            }

            return result;
        }

        public async Task<AOResult<int>> AddAsync(UserModel user)
        {
            var result = new AOResult<int>();
            try
            {
                int id = await _mockService.AddAsync<UserModel>(user);
                if (id > 0)
                {
                    result.SetSuccess(id);
                }
                else
                {
                    result.SetFailure();
                }
            }
            catch (Exception ex)
            {
                result.SetError($"Exception: {nameof(AddAsync)}", Strings.AlertDatabase, ex);
            }

            return result;
        }

        #endregion
    }
}
