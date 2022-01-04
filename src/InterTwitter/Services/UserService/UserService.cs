using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterTwitter.Services
{
    public class UserService : IUserService
    {
        private readonly IMockService _mockService;
        private readonly ISettingsManager _settingsManager;
        private readonly int _currentUserId;

        public UserService(IMockService mockService, ISettingsManager settingsManager)
        {
            _mockService = mockService;
            _settingsManager = settingsManager;
            _currentUserId = _settingsManager.UserId;
        }

        #region -- IUserService Implementation --

        public async Task<AOResult> DeleteUserAsync(UserModel user)
        {
            var result = new AOResult();
            try
            {
                await _mockService.RemoveAsync<UserModel>(user);
                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(DeleteUserAsync)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult<IEnumerable<UserModel>>> GetAllUsersAsync()
        {
            var result = new AOResult<IEnumerable<UserModel>>();
            try
            {
                var users = await _mockService.GetAllAsync<UserModel>();
                if (users != null)
                {
                    result.SetSuccess(users);
                }
                else
                {
                    result.SetFailure("Users not found!");
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(GetAllUsersAsync)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult<UserModel>> GetUserAsync(int id)
        {
            var result = new AOResult<UserModel>();
            try
            {
                var user = await _mockService.FindAsync<UserModel>(x => x.Id == id);
                if (user != null)
                {
                    result.SetSuccess(user);
                }
                else
                {
                    result.SetFailure("User not found!");
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(GetUserAsync)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult<int>> InsertUserAsync(UserModel user)
        {
            var result = new AOResult<int>();
            try
            {
                if (!await _mockService.AnyAsync<UserModel>(x => x.Email == user.Email))
                {
                    var id = await _mockService.AddAsync<UserModel>(user);

                    if (id > 0)
                    {
                        result.SetSuccess(id);
                    }
                    else
                    {
                        result.SetFailure();
                    }
                }
                else
                {
                    result.SetFailure("Such user already exists");
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(InsertUserAsync)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult> UpdateUserAsync(UserModel user)
        {
            var result = new AOResult();
            try
            {
                user = await _mockService.UpdateAsync<UserModel>(user);
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
                result.SetError($"{nameof(UpdateUserAsync)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult> AddToBlacklistAsync(int userId)
        {
            var result = new AOResult();
            try
            {
                var id = 1;
                if ((await _mockService.GetAllAsync<BlockModel>()).Count() > 0)
                {
                    id = (await _mockService.GetAllAsync<BlockModel>()).Max(x => x.Id);
                    id++;
                }

                await _mockService.AddAsync<BlockModel>(new BlockModel
                {
                    UserId = _currentUserId,
                    BlockedUserId = userId,
                    Id = id,
                });
                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(AddToBlacklistAsync)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult> AddToMuteListAsync(int userId)
        {
            var result = new AOResult();
            try
            {
                var id = 1;
                if ((await _mockService.GetAllAsync<MuteModel>()).Count() > 0)
                {
                    id = (await _mockService.GetAllAsync<MuteModel>()).Max(x => x.Id);
                }

                await _mockService.AddAsync<MuteModel>(new MuteModel
                {
                    UserId = _currentUserId,
                    MutedUserId = userId,
                });
                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(AddToMuteListAsync)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult<bool>> CheckIfUserIsBlockedAsync(int userId)
        {
            var result = new AOResult<bool>();
            try
            {
                if (await _mockService.AnyAsync<BlockModel>(x => x.UserId == _currentUserId && x.BlockedUserId == userId))
                {
                    result.SetSuccess(true);
                }
                else
                {
                    result.SetSuccess(false);
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(CheckIfUserIsBlockedAsync)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult<bool>> CheckIfUserIsMutedAsync(int userId)
        {
            var result = new AOResult<bool>();
            try
            {
                if (await _mockService.AnyAsync<MuteModel>(x => x.UserId == _currentUserId && x.MutedUserId == userId))
                {
                    result.SetSuccess(true);
                }
                else
                {
                    result.SetSuccess(false);
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(CheckIfUserIsMutedAsync)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult> RemoveFromBlacklistAsync(int userId)
        {
            var result = new AOResult();
            try
            {
                var userToRemove = await _mockService.FindAsync<BlockModel>(x => x.UserId == _currentUserId && x.BlockedUserId == userId);
                if (userToRemove != null)
                {
                    await _mockService.RemoveAsync<BlockModel>(userToRemove);
                    result.SetSuccess();
                }
                else
                {
                    result.SetFailure("No such user in blacklist");
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(RemoveFromBlacklistAsync)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult> RemoveFromMutelistAsync(int userId)
        {
            var result = new AOResult();
            try
            {
                var userToRemove = await _mockService.FindAsync<MuteModel>(x => x.UserId == _currentUserId && x.MutedUserId == userId);
                if (userToRemove != null)
                {
                    await _mockService.RemoveAsync<MuteModel>(userToRemove);
                    result.SetSuccess();
                }
                else
                {
                    result.SetFailure("No such user in mutelist");
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(RemoveFromMutelistAsync)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult<IEnumerable<UserModel>>> GetAllMutedUsersAsync()
        {
            var result = new AOResult<IEnumerable<UserModel>>();
            try
            {
                var muteList = await _mockService.GetAsync<MuteModel>(x => x.UserId == _currentUserId);

                var resultList = await _mockService.GetAsync<UserModel>(x => muteList.Any(u => u.MutedUserId == x.Id));

                if (muteList.Count() > 0)
                {
                    result.SetSuccess(resultList);
                }
                else
                {
                    result.SetFailure("Mutelist is empty");
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(GetAllMutedUsersAsync)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult<IEnumerable<UserModel>>> GetAllBlockedUsersAsync()
        {
            var result = new AOResult<IEnumerable<UserModel>>();
            try
            {
                var blacklist = await _mockService.GetAsync<BlockModel>(x => x.UserId == _currentUserId);

                var resultList = await _mockService.GetAsync<UserModel>(x => blacklist.Any(u => x.Id == u.BlockedUserId));

                if (blacklist.Count() > 0)
                {
                    result.SetSuccess(resultList);
                }
                else
                {
                    result.SetFailure("Blacklis is empty");
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(GetAllBlockedUsersAsync)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        #endregion

    }
}
