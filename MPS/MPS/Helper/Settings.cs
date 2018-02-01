using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace MPS.Helper
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string PasswordKey = "password_key";
        private const string LastDeviceGuidKey = "lastdeviceguid_key";
        private static readonly string PasswordDefault = string.Empty;
        private static readonly Guid LastDeviceGuidDefault = Guid.Empty;
        #endregion


        public static string Password
        {
            get => AppSettings.GetValueOrDefault(PasswordKey, PasswordDefault);
            set => AppSettings.AddOrUpdateValue(PasswordKey, value);
        }

        public static Guid LastDeviceGuid
        {
            get => AppSettings.GetValueOrDefault(LastDeviceGuidKey, LastDeviceGuidDefault);
            set => AppSettings.AddOrUpdateValue(LastDeviceGuidKey, value);
        }

    }
}
