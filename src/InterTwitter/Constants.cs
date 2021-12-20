﻿namespace InterTwitter
{
    public static class Constants
    {
        public static class Messages
        {
            public const string TAB_SELECTED = "TabSelected";

            public const string OPEN_SIDEBAR = "OpenSidebar";

            public const string TAB_CHANGE = "TabChange";

            public const string USER_PROFILE_CHANGED = "UserProfileChanged";
        }

        public static class RegexPatterns
        {
            public const string USERNAME_REGEX = @"^[A-Za-z ]{1,}$";

            public const string EMAIL_REGEX = @"^[\w\.]+@([\w-]+\.)+[\w-]{1,}$";

            public const string PASSWORD_REGEX = @"^(?=.*\d)(?=.*[A-ZА-ЯЁ]).{6,}$";
        }

        public static class NavigationKeys
        {
            public const string CURRENT_USER = "CurrentUser";

            public const string USER = "User";

            public const string MUTELIST = "NavigateToMutelist";

            public const string BLACKLIST = "NavigateToBlacklist";
        }

        public static class Navigation
        {
            public const string USER = nameof(USER);
            public const string MESSAGE = nameof(MESSAGE);
        }
    }
}
