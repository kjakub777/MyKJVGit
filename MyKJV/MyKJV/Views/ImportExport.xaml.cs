using MyKJV.Tools;

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
    public partial class ImportExport : ContentPage
    {
        public ImportExport()
        {
            InitializeComponent();
        }

        
        
        private async void ImportDB_Clicked(object sender, EventArgs e)
        {
           await Task.Run(()=> StarterData.ImportDb(null));
        }

        private async void ExportDB_Clicked(object sender, EventArgs e)
        {
            await Task.Run(() => StarterData.ExportDb(null));
        }

        private async void ClearDB_Clicked(object sender, EventArgs e)
        {
            await Task.Run(() => StarterData.ClearDb(null));
        }
         
        private async void ImportDBOrig_Clicked(object sender, EventArgs e)
        {
            await Task.Run(() => StarterData.ImportDBOrigCSV( ));
        }
    }
}