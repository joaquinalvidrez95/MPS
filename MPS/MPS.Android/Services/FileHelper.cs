using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MPS.Droid.Services;
using MPS.Services;
using Xamarin.Forms;

[assembly:Dependency(typeof(FileHelper))]
namespace MPS.Droid.Services
{
    public class FileHelper : IFileHelper
    {
        public string LocalFilePath(string fileName)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, fileName);           
        }
    }
}