using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CrossPlatformTintedImage.Android;
using Android.Content.Res;
using Xamarin.Forms;
using Orientation = Android.Content.Res.Orientation;

namespace MPS.Droid
{
    [Activity(Label = "MPS", Icon = "@drawable/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            RequestedOrientation = ScreenOrientation.Portrait;

            base.OnCreate(bundle);
            
            //Rg.Plugins.Popup.Popup.Init(this, bundle);
            //Forms.SetFlags("FastRenderers_Experimental");
            global::Xamarin.Forms.Forms.Init(this, bundle);
            TintedImageRenderer.Init();
            LoadApplication(new App());
        }

        //public override void OnConfigurationChanged(Configuration newConfig)
        //{
        //    base.OnConfigurationChanged(newConfig);
        //    switch (newConfig.Orientation)
        //    {
        //        case Orientation.Landscape:
        //            switch (Device.Idiom)
        //            {
        //                case TargetIdiom.Phone:
        //                    LockRotation(Orientation.Portrait);
        //                    break;
        //                case TargetIdiom.Tablet:
        //                    LockRotation(Orientation.Landscape);
        //                    break;
        //            }
        //            break;
        //        case Orientation.Portrait:
        //            switch (Device.Idiom)
        //            {
        //                case TargetIdiom.Phone:
        //                    LockRotation(Orientation.Portrait);
        //                    break;
        //                case TargetIdiom.Tablet:
        //                    LockRotation(Orientation.Landscape);
        //                    break;
        //            }
        //            break;
        //    }
            
        //}

        //private void LockRotation(Orientation orientation)
        //{
        //    switch (orientation)
        //    {
        //        case Orientation.Portrait:
        //            RequestedOrientation = ScreenOrientation.Portrait;
        //            break;
        //        case Orientation.Landscape:
        //            RequestedOrientation = ScreenOrientation.Landscape;
        //            break;
        //    }
        //}

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            RequestedOrientation = ScreenOrientation.Portrait;

        }
    }
}

