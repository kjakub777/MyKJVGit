using Acr.UserDialogs;
//using MvvmHelpers.Commands;
using MyKJV.Models;
using MyKJV.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyKJV.ViewModels
{
  public  class BooksGroupViewModel:_BaseViewModel
    {
        private BookViewModel _oldBook;

        private ObservableCollection<BookViewModel> items;
        public ObservableCollection<BookViewModel> Items
        {
            get => items;

            set => SetProperty(ref items, value);
        }
         
        public Xamarin.Forms.Command LoadBooksCommand { get; set; }
        public Xamarin.Forms.Command<BookViewModel> RefreshItemsCommand { get; set; }

        public BooksGroupViewModel()
        { 
            Items = new ObservableCollection<BookViewModel>();
            LoadBooksCommand = new Command(async () => await ExecuteLoadItemsCommandAsync());
            RefreshItemsCommand = new Command<BookViewModel>((item) => ExecuteRefreshItemsCommand(item));
        }
         
        private void ExecuteRefreshItemsCommand(BookViewModel item)
        {
            if (_oldBook == item)
            {
                // click twice on the same item will hide it
                item.Expanded = !item.Expanded;
            }
            else
            {
                if (_oldBook != null)
                {
                    // hide previous selected item
                    _oldBook.Expanded = false;
                }
                // show selected item
                item.Expanded = true;
            }

            _oldBook = item;
        }
        async System.Threading.Tasks.Task ExecuteLoadItemsCommandAsync()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
                Items.Clear();
                var booksdata = await DataStore.GetMemorizedBooks("" );
                List<Book> booklist = new List<Book>();
                int i = 0;
                foreach (var b in booksdata)
                {
                    Book newb = new Book() { Name=b.BookName ,Position=++i};
                    booklist.Add(newb);
                }

                if (booklist != null && booklist.Count > 0)
                    {  try
                  
                        {
                            foreach (var b in booklist)
                                Items.Add(new BookViewModel(b, false));
                        }
                catch (Exception ex)
                    {

                        UserDialogs.Instance.Toast($"{ex}");
                    }
                UserDialogs.Instance.Toast($"Loaded bookviewmodels.");
                }
                else { IsEmpty = true; }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        internal async Task UpdateRecited(Verse item)
        {
            await DataStore.UpdateRecited(item);
        }
    
}
    public class _BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        bool isEmpty = false;
        public bool IsEmpty
        {
            get { return isEmpty; }
            set
            {
                isEmpty = value;
                OnEmptyChanged(this, new PropertyChangedEventArgs("IsEmpty"));
            }
        }

        private void OnEmptyChanged(_BaseViewModel baseViewModel, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            UserDialogs.Instance.Toast("No Data Found" ); 
        }

        string busyText = string.Empty;
        string title = string.Empty;



        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string BusyText
        {
            get => busyText;
            set => SetProperty(ref busyText, value);
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
