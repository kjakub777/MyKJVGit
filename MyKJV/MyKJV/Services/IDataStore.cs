using MyKJV.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyKJV.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);

        Task<bool> AddItemAsync(Verse item);


        Task<bool> UpdateItemAsync(T item);

        Task<bool> DeleteItemAsync(string id);
        Task<bool> UpdateRecited(Verse verse);

        Task<T> GetItemAsync(string id);

        Task<bool> SetVerseMemorized(Verse v,bool val);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

        Task<IEnumerable<Verse>> GetVersesAsync(bool forceRefresh = false);

        Task<IEnumerable<Verse>> GetVersesAsync(string bookName, int chapter);
        Task<Bible> GetMemorizedAsync(bool forceRefresh = false);

        Task<IEnumerable<Verse>> GetVersesAsync(string bookName, bool memorized);

        Task<IEnumerable<Verse>> GetMemoryVersesAsync(string testament);
        Task<IEnumerable<BookData>> GetMemorizedBooks(string testament  , bool memorized);
        Task<IEnumerable<BookData>> GetBooksAsync(string testament="");
        Task<IEnumerable<int>> GetChaptersAsync(string bookName);
        Task<IEnumerable<Verse>> GetLastRecitedAsync();
    }
}
