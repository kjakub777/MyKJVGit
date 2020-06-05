using System;
using System.Linq;

namespace MyKJV.Models
{
    public class BookData
    {
        public string BookName { get; set; }
        public int Postition { get; set; }
    }
    public class ChapterData
    { 
        public int ChapterNumber { get; set; }
        public string ChapterNumberStr { get=>$"{ChapterNumber}"; }
    }
}