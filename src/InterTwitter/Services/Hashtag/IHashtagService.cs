using InterTwitter.Helpers;
using InterTwitter.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterTwitter.Services
{
    public interface IHashtagService
    {
        Task<AOResult<int>> IncreaseHashtagPopularityByOne(HashtagModel hashtag);

        Task<AOResult> DecreaseHashtagPopularityByOne(HashtagModel hashtag);

        Task<AOResult<IEnumerable<HashtagModel>>> GetPopularHashtags(int hashtagsNumber);
    }
}
