using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Resources.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterTwitter.Services
{
    public class LikeService : ILikeService
    {
        private readonly IMockService _mockService;

        public LikeService(IMockService mockService)
        {
            _mockService = mockService;
        }

        #region -- Interface implementation --

        public async Task<AOResult<List<LikeModel>>> GetLikesAsync(int userId)
        {
            var result = new AOResult<List<LikeModel>>();

            try
            {
                var likes = await _mockService.GetAsync<LikeModel>(x => x.UserId == userId);

                if (likes != null)
                {
                    result.SetSuccess(likes.ToList());
                }
                else
                {
                    result.SetFailure(Strings.NotFoundBookmark);
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(GetLikesAsync)}: exception", Strings.SomeIssues, ex);
            }

            return result;
        }

        public async Task<AOResult<List<LikeModel>>> GetNotificationsAsync(int userId)
        {
            var result = new AOResult<List<LikeModel>>();

            try
            {
                var likes = await _mockService.GetAsync<LikeModel>(x => x.UserId != userId && x.Notification);

                if (likes != null)
                {
                    result.SetSuccess(likes.ToList());
                }
                else
                {
                    result.SetFailure(Strings.NotFoundLike);
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(GetNotificationsAsync)}: exception", Strings.SomeIssues, ex);
            }

            return result;
        }

        public async Task<AOResult> DeleteLikeAsync(int tweetId, int userId)
        {
            var result = new AOResult();
            try
            {
                var like = await _mockService.FindAsync<LikeModel>(x => x.UserId == userId && x.TweetId == tweetId);

                if (like != null)
                {
                    result.SetSuccess();
                    await _mockService.RemoveAsync<LikeModel>(like);
                }
                else
                {
                    result.SetFailure(Strings.NotFoundLike);
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(DeleteLikeAsync)}: exception", Strings.SomeIssues, ex);
            }

            return result;
        }

        public async Task<AOResult<int>> AddLikeAsync(int tweetId, int userId)
        {
            var result = new AOResult<int>();
            try
            {
                if (!await _mockService.AnyAsync<LikeModel>(x => x.TweetId == tweetId && x.UserId == userId))
                {
                    var like = new LikeModel
                    {
                        TweetId = tweetId,
                        UserId = userId,
                        Notification = true,
                    };

                    int id = await _mockService.AddAsync(like);
                    if (id > 0)
                    {
                        result.SetSuccess(id);
                    }
                }
                else
                {
                    result.SetFailure();
                }
            }
            catch (Exception ex)
            {
                result.SetError($"Exception: {nameof(AddLikeAsync)}", Strings.WrongResult, ex);
            }

            return result;
        }

        public async Task<AOResult> AnyAsync(int tweetId, int userId)
        {
            var result = new AOResult();
            try
            {
                var any = await _mockService.AnyAsync<LikeModel>(x => x.TweetId == tweetId && x.UserId == userId);
                if (any)
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
                result.SetError($"Exception: {nameof(AnyAsync)}", Strings.WrongResult, ex);
            }

            return result;
        }

        public async Task<AOResult<int>> CountAsync(int tweetId)
        {
            var result = new AOResult<int>();
            try
            {
                var count = (await _mockService.GetAllAsync<LikeModel>()).Count(x => x.TweetId == tweetId);
                if (count >= 0)
                {
                    result.SetSuccess(count);
                }
                else
                {
                    result.SetFailure();
                }
            }
            catch (Exception ex)
            {
                result.SetError($"Exception: {nameof(CountAsync)}", Strings.WrongResult, ex);
            }

            return result;
        }

        #endregion
    }
}
