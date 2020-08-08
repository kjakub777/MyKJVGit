using Acr.UserDialogs;
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
using MvvmHelpers;
using System.ComponentModel;
using MyKJV.Services;

namespace MyKJV.ViewModels
{
    public class BookViewModel : ObservableRangeCollection<VerseViewModel>, INotifyPropertyChanged//,BaseViewModel
    {
        public string Title;

        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        // It's a backup variable for storing CountryViewModel objects
        ObservableRangeCollection<VerseViewModel> verses = new ObservableRangeCollection<VerseViewModel>();
        Book book;
        public BookViewModel(string BookName, bool expanded)
        {
            Title = "Memorized";
            this._expanded = expanded;
            Book = new Book() { Name = BookName };
            LoadVerses();
        }
   public BookViewModel(Book book, bool expanded)
        {
            Title = "Memorized";
            this._expanded = expanded;
            this.Book = book;
            LoadVerses();
        }

        async void LoadVerses()
        {
            try
            {
                var vs = await DataStore.GetVersesAsync(Book.Name, true);
                foreach (var v in vs)
                {
                    verses.Add(new VerseViewModel(v));
                }
            }
            catch (Exception ex)
            {

                UserDialogs.Instance.Toast($"{ex}");
            }
            if (this._expanded)
                this.AddRange(verses);
        }
  public BookViewModel( )
        {
          
        }








        private bool _expanded;
       
        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Expanded")); 
                    if (_expanded)
                    {
                        this.AddRange(verses);
                    }
                    else
                    {
                        this.Clear();
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("StateIcon"));
                }
            }
        } 
        public Command LoadItemsCommand { get; set; }
        public string StateIcon
        {
            get
            {
                if (Expanded)
                {
                    return "arrow_b.png";
                }
                else
                { return "arrow_a.png"; }
            }
        }
        public string Name { get { return Book.Name; } }

        public Book Book { get => this.book; set => this.book = value; }
        public bool IsBusy { get; private set; }
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
