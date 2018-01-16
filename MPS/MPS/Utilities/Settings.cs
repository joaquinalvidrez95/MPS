using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace MPS.Utilities
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string PasswordKey = "password_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault(PasswordKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(PasswordKey, value);
            }
        }

    }
}
