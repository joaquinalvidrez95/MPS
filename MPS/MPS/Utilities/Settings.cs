using System;
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
        private const string LastDeviceGuidKey = "lastdeviceguid_key";
        private static readonly string PasswordDefault = string.Empty;
        private const string LastDeviceGuidDefault = "00000000-0000-0000-0000-f0c77f868a9c";

        #endregion


        public static string Password
        {
            get => AppSettings.GetValueOrDefault(PasswordKey, PasswordDefault);
            set => AppSettings.AddOrUpdateValue(PasswordKey, value);
        }

        public static Guid LastDeviceGuid
        {
            get => new Guid(AppSettings.GetValueOrDefault(LastDeviceGuidKey, LastDeviceGuidDefault));
            set => AppSettings.AddOrUpdateValue(LastDeviceGuidKey, value);
        }

    }
}
