using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyKJV.Models;
using MyKJV.ViewModels;

namespace MyKJV.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    { 
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            //MenuPages.Add((int)MenuItemType.Bible, (NavigationPage)tb.Children[0]);
            //MenuPages.Add((int)MenuItemType.Memorized, (NavigationPage)tb.Children[1]);
            //MenuPages.Add((int)MenuItemType.Memorized, (NavigationPage)Detail);
            //MenuPages.Add((int)MenuItemType.Memorized, (NavigationPage)Detail);
            MenuPages.Add((int)MenuItemType.Bible, new NavigationPage(new BibleMainPage()));
            MenuPages.Add((int)MenuItemType.Memorized, new NavigationPage(new VersesMemorizedPage()));
            MenuPages.Add((int)MenuItemType.LastRecited, new NavigationPage(new LastRecitedPage()));
            MenuPages.Add((int)MenuItemType.ImportExport, new NavigationPage(new ImportExport(new ImportExportViewModel())));
            MenuPages.Add((int)MenuItemType.BookGroupMemorized, new NavigationPage(new BookVersesMemorizedPage(new BooksGroupViewModel())));
            MenuPages.Add((int)MenuItemType.About, new NavigationPage(new AboutPage()));
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Bible:
                        MenuPages.Add(id, new NavigationPage(new BibleMainPage()));
                        break;
                    case (int)MenuItemType.Memorized:
                        MenuPages.Add(id, new NavigationPage(new VersesMemorizedPage()));
                        break;
                    case (int)MenuItemType.LastRecited:
                        MenuPages.Add(id, new NavigationPage(new LastRecitedPage()));
                        break;
                    case (int)MenuItemType.ImportExport:
                        MenuPages.Add(id, new NavigationPage(new ImportExport(new ImportExportViewModel())));
                        break;
                    case (int)MenuItemType.BookGroupMemorized:
                        MenuPages.Add(id, new NavigationPage(new BookVersesMemorizedPage(new BooksGroupViewModel())));
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}