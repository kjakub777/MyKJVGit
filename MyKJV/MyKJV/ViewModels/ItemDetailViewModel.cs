using System;

using MyKJV.Models;

namespace MyKJV.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Verse Item { get; set; }
        public bool IsMemorized { get; set; }
        public ItemDetailViewModel(Verse item = null)
        {
            Title = item?.FullTitle;
            Item = item;
            IsMemorized = item != null ? item.IsMemorized : false;
        }
    }
}
