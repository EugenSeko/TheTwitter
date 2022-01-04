using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Resources.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterTwitter.Services
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IMockService _mockService;

        public BookmarkService(IMockService mockService)
        {
            _mockService = mockService;
        }

        #region -- Interface implementation --

        public async Task<AOResult<List<Bookmark>>> GetBookmarksAsync(int userId)
        {
            var result = new AOResult<List<Bookmark>>();

            try
            {
                var bookmarks = await _mockService.GetAsync<Bookmark>(x => x.UserId == userId);

                if (bookmarks != null)
                {
                    result.SetSuccess(bookmarks.ToList());
                }
                else
                {
                    result.SetFailure(Strings.NotFoundBookmark);
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(GetBookmarksAsync)}: exception", Strings.SomeIssues, ex);
            }

            return result;
        }

        public async Task<AOResult<List<Bookmark>>> GetNotificationsAsync(int userId)
        {
            var result = new AOResult<List<Bookmark>>();

            try
            {
                var bookmarks = await _mockService.GetAsync<Bookmark>(x => x.UserId != userId && x.Notification);

                if (bookmarks != null)
                {
                    result.SetSuccess(bookmarks.ToList());
                }
                else
                {
                    result.SetFailure(Strings.NotFoundBookmark);
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(GetNotificationsAsync)}: exception", Strings.SomeIssues, ex);
            }

            return result;
        }

        public async Task<AOResult> DeleteAllBookmarksAsync(int userId)
        {
            var result = new AOResult();
            try
            {
                var bookmark = await _mockService.FindAsync<Bookmark>(x => x.UserId == userId);

                if (bookmark != null)
                {
                    await _mockService.RemoveAllAsync<Bookmark>(x => x.UserId == userId);

                    result.SetSuccess();
                }
                else
                {
                    result.SetFailure(Strings.NotFoundBookmark);
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(DeleteAllBookmarksAsync)}: exception", Strings.SomeIssues, ex);
            }

            return result;
        }

        public async Task<AOResult> DeleteBoormarkAsync(int tweetId, int userId)
        {
            var result = new AOResult();
            try
            {
                var bookmark = await _mockService.FindAsync<Bookmark>(x => x.UserId == userId && x.TweetId == tweetId);

                if (bookmark != null)
                {
                    result.SetSuccess();
                    await _mockService.RemoveAsync<Bookmark>(bookmark);
                }
                else
                {
                    result.SetFailure(Strings.NotFoundBookmark);
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(DeleteBoormarkAsync)}: exception", Strings.SomeIssues, ex);
            }

            return result;
        }

        public async Task<AOResult<int>> AddBookmarkAsync(int tweetId, int userId)
        {
            var result = new AOResult<int>();
            try
            {
                if (!await _mockService.AnyAsync<Bookmark>(x => x.TweetId == tweetId && x.UserId == userId))
                {
                    var bookmark = new Bookmark
                    {
                        TweetId = tweetId,
                        UserId = userId,
                        Notification = true,
                    };
                    var id = await _mockService.AddAsync<Bookmark>(bookmark);
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
                result.SetError($"{nameof(AddBookmarkAsync)}: exception", Strings.WrongResult, ex);
            }

            return result;
        }

        public async Task<AOResult> AnyAsync(int tweetId, int userId)
        {
            var result = new AOResult();
            try
            {
                var any = await _mockService.AnyAsync<Bookmark>(x => x.TweetId == tweetId && x.UserId == userId);
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
                result.SetError($"Exception: {nameof(AddBookmarkAsync)}", Strings.WrongResult, ex);
            }

            return result;
        }

        #endregion
    }
}
