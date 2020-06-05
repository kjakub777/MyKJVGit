using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using MyKJV.Models;
using MyKJV.Views;
using System.Linq;
using Xamarin.Forms.Internals;

namespace MyKJV.ViewModels
{
    public class VersesViewModel : BaseViewModel
    {
        Verse selectedVerse;
        public Verse SelectedVerse
        {
            get => this.selectedVerse;
            set=>SetProperty(ref selectedVerse, value);
        }
        string testamentName;
        public string TestamentName
        {
            get
            {
                return this.testamentName;
            }
            set
            {
                SetProperty(ref this.testamentName, value);
            }
        }
        BookData currentBookData;
        public BookData CurrentBookData
        {
            get
            {
                return this.currentBookData;
            }
            set
            {
                SetProperty(ref this.currentBookData, value);
            }
        }
        public ObservableCollection<BookData> BookDatas { get; set; }
        public ObservableCollection<Verse> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public VersesViewModel()
        {
            Title = "Memorized";
            BookDatas = new ObservableCollection<BookData>();
            Items = new ObservableCollection<Verse>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Verse>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Verse;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                if (CurrentBookData != null)
                    await ExecuteLoadVersesCommand();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task ExecuteLoadBooksCommand()
        {
            IsBusy = true;
            try
            {
                BookDatas.Clear();
                var bs = await DataStore.GetMemorizedBooks(TestamentName == "Both" ? "" : TestamentName, true);
                BookDatas.Add(new BookData() { BookName = "All" });
                bs.ForEach((b) => BookDatas.Add(b));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                IsBusy = false;
            }
        }

        internal async Task UpdateRecited(Verse item)
        {
            await DataStore.UpdateRecited(item);
            IsBusy = true;
        }
        internal async Task UpdateRecited( )
        {
            if(SelectedVerse != null)
            {
                await DataStore.UpdateRecited(SelectedVerse);
                IsBusy = true;
            }
        }

        public async Task ExecuteLoadVersesCommand()
        {
            IsBusy = true;
            try
            {
                Items.Clear();
                if(currentBookData != null)
                {
                    var vs = await DataStore.GetVersesAsync(currentBookData.BookName, true);
                    vs.ForEach((v) => Items.Add(v));
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}