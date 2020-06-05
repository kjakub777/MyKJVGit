using System;
using System.Collections.Generic;
using System.Text;

namespace MyKJV.Models
{
    public enum MenuItemType
    {
        Bible,
        Memorized,
        LastRecited,
        ImportExport,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
