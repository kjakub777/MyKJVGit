using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyKJV.Services;
using MyKJV.Views;
using MyKJV.Tools;

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

        protected override async void OnStart()
        {
        await StarterData.EnsureDataExists();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
