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
    public partial class VersesPage : ContentPage
    {
        VersesViewModel viewModel;

        public VersesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new VersesViewModel();
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (Verse)layout.BindingContext;
            await Navigation.PushAsync(new VerseDetailPage(new VerseDetailViewModel(item)));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            //  await Navigation.PushModalAsync(new NavigationPage(new NewVersePage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Items.Count == 0)
            {
                viewModel.IsBusy = true;
            }
            //if (viewModel.Items.Count == 0)
            //    viewModel.IsBusy = true;
        }

        private async void TestamentPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            await viewModel.ExecuteLoadBooksCommand();
        }

        private async void BookPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

            await viewModel.ExecuteLoadVersesCommand();
        }

        private async void ChapterPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //   await viewModel.ExecuteLoadVersesCommand();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var item = (Verse)layout.BindingContext; 
            if (item != null)
                await viewModel.UpdateRecited(item);
           // await viewModel.UpdateRecited( );
        }
    }
}