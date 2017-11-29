using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPS.Data;
using MPS.Services;
using Xamarin.Forms;

namespace MPS
{
    public partial class App : Application
    {
        private static MessageDatabase _database;

        public static MessageDatabase Database => _database ?? (_database = new MessageDatabase(
                                                      DependencyService.Get<IFileHelper>().LocalFilePath("Messages.db")));

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
