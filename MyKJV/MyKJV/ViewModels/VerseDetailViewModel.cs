using System;

using MyKJV.Models;

namespace MyKJV.ViewModels
{
    public class VerseDetailViewModel : BaseViewModel
    {
        public Verse Verse { get; set; }
        bool isMemorized;
        public bool IsMemorized
        {
            get
            {
                return isMemorized;
            }
            set
            {
                SetProperty(ref isMemorized, value);
                SetMemorized(Verse, value);
            }
        }
        public VerseDetailViewModel(Verse item = null)
        {
            Title = item?.ToString();
            Verse = item;
            IsMemorized = item != null ? item.IsMemorized : false;
        }
        internal async void SetMemorized(Verse v1, bool v2)
        {
            IsBusy = true;
            if (v2 != v1.IsMemorized)
                await DataStore.SetVerseMemorized(v1, v2);
            IsBusy = false;
        }
    }
}
