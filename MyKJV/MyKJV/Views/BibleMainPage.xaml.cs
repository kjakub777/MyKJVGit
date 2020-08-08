using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyKJV.Models;
using MyKJV.Views;
using MyKJV.ViewModels;

namespace MyKJV.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class BibleMainPage : ContentPage
    {
        ItemsViewModel viewModel;

        public BibleMainPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();

        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (Verse)layout.BindingContext;
            await Navigation.PushAsync(new VerseDetailPage(new VerseDetailViewModel(item)));
            viewModel.IsBusy = true;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Items.Count == 0)
            {
                viewModel.IsBusy = true;
                //viewModel.LoadItemsCommand.Execute(null);
            }
        }

        private async void TestamentPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //   viewModel.LoadBooksCommand.Execute(((Picker)sender).SelectedItem as Testament);
            //viewModel.IsBusy = true;
            await viewModel.ExecuteLoadBooksCommand();
        }

        private async void BookPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //   viewModel.LoadChaptersCommand.Execute(((Picker)sender).SelectedItem as Book);
            //viewModel.IsBusy = true;
            await viewModel.ExecuteLoadChaptersCommand();
        }

        private async void ChapterPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  viewModel.LoadVersesCommand.Execute(((Picker)sender).SelectedItem as Chapter);
            //viewModel.IsBusy = true;
            await viewModel.ExecuteLoadVersesCommand();
        }

        private async void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        { 
            await viewModel.ExecuteSwipeCommand(e.Parameter);
        }
    }
}