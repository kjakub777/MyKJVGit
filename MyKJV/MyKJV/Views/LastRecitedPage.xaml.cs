using MyKJV.Models;
using MyKJV.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyKJV.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LastRecitedPage : ContentPage
    {
        LastRecitedViewModel viewModel;
        public LastRecitedPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new LastRecitedViewModel();
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (Verse)layout.BindingContext;
            await Navigation.PushAsync(new VerseDetailPage(new VerseDetailViewModel(item)));
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
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var item = (Verse)layout.BindingContext;
            if (item != null)
                await viewModel.UpdateRecited(item);
        }
    }
}