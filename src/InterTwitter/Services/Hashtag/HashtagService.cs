using InterTwitter.Helpers;
using InterTwitter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterTwitter.Services
{
    public class HashtagService : IHashtagService
    {
        private readonly IMockService _mockService;

        public HashtagService(IMockService mockService)
        {
            _mockService = mockService;
        }

        #region -- IHashtagManager implementation --

        public async Task<AOResult<int>> IncreaseHashtagPopularityByOne(HashtagModel hashtag)
        {
            var result = new AOResult<int>();

            try
            {
                if (!await _mockService.AnyAsync<HashtagModel>(x => x.Text.ToLower() == hashtag.Text.ToLower()))
                {
                    var id = await _mockService.AddAsync<HashtagModel>(hashtag);

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
                    result.SetFailure();
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(IncreaseHashtagPopularityByOne)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult> DecreaseHashtagPopularityByOne(HashtagModel hashtag)
        {
            var result = new AOResult();

            try
            {
                if (await _mockService.AnyAsync<HashtagModel>(x => x.Text.ToLower() == hashtag.Text.ToLower()))
                {
                    if (await _mockService.RemoveAsync<HashtagModel>(hashtag))
                    {
                        result.SetSuccess();
                    }
                    else
                    {
                        result.SetFailure();
                    }
                }
                else
                {
                    result.SetFailure();
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(DecreaseHashtagPopularityByOne)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult<IEnumerable<HashtagModel>>> GetPopularHashtags(int numbersOfHashtags)
        {
            var result = new AOResult<IEnumerable<HashtagModel>>();

            try
            {
                var allHashtags = await _mockService.GetAllAsync<HashtagModel>();

                var popularHashtags = allHashtags
                    ?.OrderByDescending(x => x.TweetsCount)
                    ?.Take(numbersOfHashtags);

                if (popularHashtags is not null)
                {
                    result.SetSuccess(popularHashtags);
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(GetPopularHashtags)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        #endregion
    }
}