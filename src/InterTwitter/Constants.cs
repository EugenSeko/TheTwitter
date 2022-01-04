using System.Collections.Generic;
using System.Linq;

namespace InterTwitter
{
    public static class Constants
    {
        public static class Messages
        {
            public const string TAB_SELECTED = nameof(TAB_SELECTED);
            public const string OPEN_SIDEBAR = nameof(OPEN_SIDEBAR);
            public const string TAB_CHANGE = nameof(TAB_CHANGE);
            public const string USER_PROFILE_CHANGED = nameof(USER_PROFILE_CHANGED);
            public const string UPDATE_HASHTAGS = nameof(UPDATE_HASHTAGS);
        }

        public static class Values
        {
            public const int NUMBER_OF_POPULAR_HASHTAGS = 5;
        }

        public static class Methods
        {
            public static IEnumerable<string> GetUniqueWords(string text)
            {
                return text.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct();
            }
        }

        public static class RegexPatterns
        {
            public const string HASHTAG_PATTERN = @"^\#[0-9a-zA-Zа-яА-Я_]{1,30}$";
            public const string USERNAME_REGEX = @"^[A-Za-z ]{1,}$";
            public const string EMAIL_REGEX = @"^[\w\.]+@([\w-]+\.)+[\w-]{1,}$";
            public const string PASSWORD_REGEX = @"^(?=.*\d)(?=.*[A-ZА-ЯЁ]).{6,}$";
        }

        public static class Navigation
        {
            public const string CURRENT_USER = nameof(CURRENT_USER);
            public const string MUTELIST = nameof(MUTELIST);
            public const string BLACKLIST = nameof(BLACKLIST);
            public const string USER = nameof(USER);
            public const string MESSAGE = nameof(MESSAGE);
        }

        public static class DialogParameterKeys
        {
            public const string MESSAGE = nameof(MESSAGE);
            public const string TITLE = nameof(TITLE);
            public const string OK_BUTTON_TEXT = nameof(OK_BUTTON_TEXT);
            public const string CANCEL_BUTTON_TEXT = nameof(CANCEL_BUTTON_TEXT);
            public const string ACCEPT = nameof(ACCEPT);
        }

        public static class Limits
        {
            public const int MAX_COUNT_ATTACHED_GIF = 1;
            public const int MAX_COUNT_ATTACHED_PHOTOS = 6;
            public const int MAX_SIZE_ATTACHED_PHOTO = 5 * 1024 * 1024;
            public const int MAX_SIZE_ATTACHED_VIDEO = 15 * 1024 * 1024;
            public const int MAX_LENGTH_VIDEO = 180;
            public const int MAX_LENGTH_TEXT = 250;
        }
    }
}
