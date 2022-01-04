using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Models.TweetViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace InterTwitter.Services
{
    public interface ITweetService
    {
        Task<AOResult<IEnumerable<TweetModel>>> GetAllTweetsAsync();

        Task<AOResult<UserModel>> GetAuthorAsync(int authorId);

        Task<AOResult<List<TweetModel>>> GetByUserTweetsAsync(int userid);

        Task<AOResult<IEnumerable<TweetModel>>> FindTweetsByKeywordsAsync(IEnumerable<string> keys);

        Task<AOResult> AddTweetAsync(TweetModel tweet);
    }
}
