using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using MyKJV.Models;
using MyKJV.Views;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms.Internals;

namespace MyKJV.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        
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
        //Testament testament;
        //public Testament Testament
        //{
        //    get
        //    {
        //        return this.testament;
        //    }
        //    set
        //    {
        //        SetProperty(ref this.testament, value);
        //    }
        //}

        string bookName;
        public string BookName
        {
            get => bookName;
            set => SetProperty(ref bookName, value);
        }
        //Book book;
        //public Book Book
        //{
        //    get
        //    {
        //        return this.book;
        //    }
        //    set
        //    {
        //        SetProperty(ref this.book, value);
        //    }
        //}
        ChapterData currentChapter;
        public ChapterData CurrentChapter
        {
            get
            {
                return this.currentChapter;
            }
            set
            {
                SetProperty(ref this.currentChapter, value);
            }
        }
        //  public Testament Testament { get; set; } 
        //public ObservableCollection<Book> Books { get; set; }
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
        public ObservableCollection<ChapterData> Chapters { get; set; }
        public ObservableCollection<Verse> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command<object> SwipeCommand { get; set; }

        internal async void SetMemorized(Verse v1, bool v2)
        {
            IsBusy = await DataStore.SetVerseMemorized(v1, v2);
        }

        //public Command<Testament> LoadBooksCommand { get; set; }
        //public Command<Book> LoadChaptersCommand { get; set; }
        //public Command<Chapter> LoadVersesCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "KJV";
            //Testaments = new ObservableCollection<Testament>();
            //Books = new ObservableCollection<Book>();
            Chapters = new ObservableCollection<ChapterData>();
            Items = new ObservableCollection<Verse>();
            BookDatas = new ObservableCollection<BookData>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SwipeCommand = new Command<object>(async(obj) =>
            {
            });
            //LoadBooksCommand = new Command<Testament>(async (t) => await ExecuteLoadBooksCommand(t));
            //LoadChaptersCommand = new Command<Book>(async (b) => await ExecuteLoadChaptersCommand(b));
            //LoadVersesCommand = new Command<Chapter>(async (c) => await ExecuteLoadVersesCommand(c));

            MessagingCenter.Subscribe<NewItemPage, Verse>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Verse;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

     public   async Task ExecuteSwipeCommand(object obj)
        { 
            if (obj is Verse v)
            {
              SetMemorized(v, !v.IsMemorized);
            }
        }
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                if (CurrentBookData != null && CurrentChapter != null)
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
                var bs = await DataStore.GetBooksAsync(TestamentName);
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
        public async Task ExecuteLoadChaptersCommand()
        {
            IsBusy = true;
            try
            {
                Chapters.Clear();
                IEnumerable<int> ch = await DataStore.GetChaptersAsync(CurrentBookData.BookName);
                ch.ForEach((c) => Chapters.Add(new ChapterData() { ChapterNumber = c }));

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
        /* //else
                //{
                if(Testament == null)
                    Testament = Testaments.FirstOrDefault();
                Books.Clear();
                Testament.Books.ForEach(x => Books.Add(x));                  
                if (Book == null) 
                    Book = Books.FirstOrDefault();


                Chapters.Clear();
                Book.Chapters.ForEach(x => Chapters.Add(x));
                if (Chapter == null)
                    Chapter =  Chapters.FirstOrDefault();
               

                Items.Clear();
                Chapter.Verses.ForEach(x => Items.Add(x));
                //}
                // await Load*/
        public async Task ExecuteLoadVersesCommand()
        {
            IsBusy = true;
            try
            {
                Items.Clear();
                var vs = await DataStore.GetVersesAsync(currentBookData.BookName, CurrentChapter.ChapterNumber);
                vs.ForEach((v) => Items.Add(v));

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