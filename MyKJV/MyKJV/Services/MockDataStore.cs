using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MyKJV.Models;
using MyKJV.Tools;

namespace MyKJV.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {

        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }
        public async Task<bool> AddItemAsync(Verse item)
        {
            //items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            //var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            //items.Remove(oldItem);
            //items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            //var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            //items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<Verse>> GetVersesAsync(bool memorized = false)
        {
            var db = new DatabaseConnection().DbConnection();
            if (memorized)
                return await Task.FromResult(db.Table<Verse>().Where(x => x.IsMemorized));
            else
                return await Task.FromResult(db.Table<Verse>());
        }

        public async Task<Bible> GetMemorizedAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(StarterData.GetMemorized());
        }


        public async Task<IEnumerable<BookData>> GetBooksAsync(string testament = "")
        {
            var db = new DatabaseConnection().DbConnection();
            if (string.IsNullOrEmpty(testament) || testament == "Both")
                return await Task.FromResult(db.Query<Verse>("select distinct bookname from verse ")
                .Select(x => new BookData() { BookName = x.BookName }));
            else
                return await Task.FromResult(db.Query<Verse>("select distinct bookname from verse where testament=?", testament)
                .Select(x => new BookData() { BookName = x.BookName }));

        }
        public async Task<IEnumerable<int>> GetChaptersAsync(string bookName)
        {
            var db = new DatabaseConnection().DbConnection();
            if (string.IsNullOrEmpty(bookName))
                return new List<int>();
            else
                return await Task.FromResult(db.Query<Verse>("select distinct ChapterNumber from verse where BookName=?", bookName)
                .OrderBy(x => x.ChapterNumber).Select(x => x.ChapterNumber));

        }
        public async Task<IEnumerable<Verse>> GetVersesAsync(string bookName, int chapter)
        {
            var db = new DatabaseConnection().DbConnection();
            if (string.IsNullOrEmpty(bookName) || chapter == 0)
                return new List<Verse>();
            return await Task.FromResult(db.Table<Verse>()
                .Where(x => x.BookName == bookName && x.ChapterNumber == chapter)
                .OrderBy(x => x.VerseNumber));

        }
        public async Task<IEnumerable<BookData>> GetMemorizedBooks(string testament, bool memorized)
        {
            var db = new DatabaseConnection().DbConnection();
            if (string.IsNullOrEmpty(testament) || testament == "Both")
                return await Task.FromResult(db.Query<Verse>("select distinct bookname from verse where IsMemorized=?", memorized)
                .Select(x => new BookData() { BookName = x.BookName }));
            else
                return await Task.FromResult(db.Query<Verse>("select distinct bookname from verse where testament=? and IsMemorized=?", testament, memorized)
                .Select(x => new BookData() { BookName = x.BookName }));

        }
        public async Task<IEnumerable<Verse>> GetVersesAsync(string bookName, bool memorized)
        {
            var db = new DatabaseConnection().DbConnection();
            if (string.IsNullOrEmpty(bookName))
                return new List<Verse>();
            if (bookName == "All")
                return await Task.FromResult(db.Table<Verse>()
               .Where(x => x.IsMemorized)
               .OrderBy(x => x.BookPosition).ThenBy(x => x.BookName).ThenBy(x => x.ChapterNumber).ThenBy(x => x.VerseNumber));
            if (memorized)
                return await Task.FromResult(db.Table<Verse>()
                    .Where(x => x.BookName == bookName && x.IsMemorized)
                    .OrderBy(x => x.ChapterNumber).ThenBy(x => x.VerseNumber));
            else return await Task.FromResult(db.Table<Verse>()
               .Where(x => x.BookName == bookName)
               .OrderBy(x => x.ChapterNumber).ThenBy(x => x.VerseNumber));

        }

        public async Task<bool> SetVerseMemorized(Verse v, bool val)
        {
            var db = new DatabaseConnection().DbConnection();
            v.IsMemorized = val;
            return await Task.FromResult(db.Update(v) == 1);
        }

        public Task<bool> UpdateRecited(Verse verse)
        {
            var db = new DatabaseConnection().DbConnection();
            if (verse.LastRecited >= DateTime.Now.AddDays(-1))
                verse.LastRecited = DateTime.Now.AddDays(-60);
            else
                verse.LastRecited = DateTime.Today;
            return Task.FromResult(1 == db.Update(verse));
        }

        public async Task<IEnumerable<Verse>> GetLastRecitedAsync()
        {
            var db = new DatabaseConnection().DbConnection();
            return await Task.FromResult(db.Table<Verse>()
                .Where(x => x.IsMemorized)
                .OrderBy(x => x.LastRecited)
                .ThenBy(x => x.BookPosition)
                .ThenBy(x => x.ChapterNumber)
                .ThenBy(x => x.VerseNumber));
        }
    }
}