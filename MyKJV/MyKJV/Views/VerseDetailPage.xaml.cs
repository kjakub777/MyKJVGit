using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyKJV.Models;
using MyKJV.ViewModels;

namespace MyKJV.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class VerseDetailPage : ContentPage
    {
        VerseDetailViewModel viewModel;

        public VerseDetailPage(VerseDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var chkBox = sender as CheckBox;
            viewModel.IsMemorized = chkBox.IsChecked;
        }

        //public VerseDetailPage()
        //{
        //    InitializeComponent();

        //    var item = new Verse
        //    {
        //        Text = "Item 1"               
        //    };

        //    viewModel = new VerseDetailViewModel(item);
        //    BindingContext = viewModel;
        //}
    }
}