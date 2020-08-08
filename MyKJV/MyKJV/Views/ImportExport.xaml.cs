using MyKJV.Tools;
using MyKJV.ViewModels;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace MyKJV.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImportExport : ContentPage
    {
        Action<double, uint> animatePbar;
        public ImportExportViewModel viewmodel;
        ///public string importPath = "/storage/emulated/0/db.txt", exportPath = "/storage/emulated/0/";
        public bool upftp = false;
        public ImportExport(ImportExportViewModel vm)
        {
            InitializeComponent();

            viewmodel = vm;
            this.BindingContext = viewmodel;
            Title = "Import and Export";
            viewmodel.Init();

            //EntryImp
            //cbFtp
            pbar.IsVisible = pbar.Progress != 0d && pbar.Progress < .999d;
            animatePbar = new Action<double, uint>((dval, uimax) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    pbar.Progress = dval / (double)uimax;
                    pbar.IsVisible = pbar.Progress != 0d && pbar.Progress < .999d;
                });
            });
        }



        private async void ImportDB_Clicked(object sender, EventArgs e)
        {
            await viewmodel.ImportDb(animatePbar);
            //await Task.Factory
            //    .StartNew(async () =>
            //      {
            //                      });
        }

        private async void ExportDB_Clicked(object sender, EventArgs e)
        {
            await viewmodel.ExportDb(animatePbar);
        }

        private async void ClearDB_Clicked(object sender, EventArgs e)
        {
            await viewmodel.ClearDb();
        }

        private async void ImportDBOrig_Clicked(object sender, EventArgs e)
        {
            await StarterData.ImportDBOrigCSV(animatePbar);
        }

        private void EntryExp_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewmodel.ExportPath = e.NewTextValue;
        }

        private void EntryImp_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewmodel.ImportPath = e.NewTextValue;
        }

        private void cbFtp_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            viewmodel.UpFtp = e.Value;
        }
    }
}