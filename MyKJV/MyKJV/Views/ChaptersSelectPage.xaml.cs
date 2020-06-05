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
    public partial class ChaptersSelectPage : ContentPage
    {
        public ChaptersSelectPage()
        {
            InitializeComponent();
        }

        private void OnSLItemTapped(object sender, EventArgs e)
        {

        }
    }
}