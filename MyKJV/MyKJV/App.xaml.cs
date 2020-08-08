using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyKJV.Services;
using MyKJV.Views;
using MyKJV.Tools;
using Android.Support.V4.Content;
using Android;
using static Android.Manifest;
using System.Threading.Tasks;

namespace MyKJV
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected async override   void OnStart()
        {
            await Task.Delay(1);//  FtpHandler.SendDbToFTP();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
