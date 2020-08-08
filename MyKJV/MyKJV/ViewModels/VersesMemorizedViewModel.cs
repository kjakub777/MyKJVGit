using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using MyKJV.Models;
using MyKJV.Views;
using System.Linq;
using Xamarin.Forms.Internals;
using System.Collections.Generic;

namespace MyKJV.ViewModels
{
    public class VersesMemorizedViewModel : BaseViewModel
    {

        BookData currentBookData;


        Verse selectedVerse;
        string testamentName;

        public VersesMemorizedViewModel()
        {
            Title = "Memorized";
            BookDatas = new ObservableCollection<BookData>();
            MemoryBooks = new ObservableCollection<MemoryBookGroup>();
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
                // if (CurrentBookData != null)
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

        internal async Task UpdateRecited()
        {
            if (SelectedVerse != null)
            {
                await DataStore.UpdateRecited(SelectedVerse);
                IsBusy = true;
            }
        }

        internal async Task UpdateRecited(Verse item)
        {
            await DataStore.UpdateRecited(item);
            IsBusy = true;
        }

        public async Task ExecuteLoadBooksCommand()
        {
            IsBusy = true;
            try
            {
                //BookDatas.Clear();
                //var bs = await DataStore.GetMemorizedBooks(TestamentName == "Both" ? "" : TestamentName, true);
                //BookDatas.Add(new BookData() { BookName = "All" });
                //bs.ForEach((b) => BookDatas.Add(b));
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

        public async Task ExecuteLoadVersesCommand()
        {
            IsBusy = true;
            try
            {
                //Items.Clear();
                //if(currentBookData != null)
                //{
                MemoryBooks.Clear();
                IEnumerable<Verse> vs;
                if (!string.IsNullOrEmpty(TestamentName))
                {
                    vs = await DataStore.GetMemoryVersesAsync(TestamentName);
                    vs.GroupBy(x => x.BookName)
                        .ForEach((v) => MemoryBooks.Add(new MemoryBookGroup(v.Key, v.ToList())));
                }
                else vs = await DataStore.GetMemoryVersesAsync("Both");
                vs.GroupBy(x => x.BookName)
                  .ForEach((v) => MemoryBooks.Add(new MemoryBookGroup(v.Key, v.ToList())));

                // }

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

        public ObservableCollection<BookData> BookDatas { get; set; }
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
        //public bool Expanded
        //{
        //    get { return _expanded; }
        //    set
        //    {
        //        if (_expanded != value)
        //        {
        //            _expanded = value;
        //           SetProperty(ref this._expanded, value);
        //             if (_expanded)
        //            {
        //                this.AddRange(hotelRooms);
        //            }
        //            else
        //            {
        //                this.Clear();
        //            }
        //        }
        //    }
        //}
        public ObservableCollection<Verse> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public ObservableCollection<MemoryBookGroup> MemoryBooks { get; set; }
        public Verse SelectedVerse
        {
            get => this.selectedVerse;
            set => SetProperty(ref selectedVerse, value);
        }
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
    }
}


/*using System;
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
    public class VersesMemorizedViewModel : BaseViewModel
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
        public VersesMemorizedViewModel()
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
*/
