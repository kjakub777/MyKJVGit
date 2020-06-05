using System;
using SQLite;
using System.ComponentModel;
using SQLiteNetExtensions.Attributes;

namespace MyKJV.Models
{
    [Table("Verse")]
    public class Verse
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        [MaxLength(500)]
        public string Text { get; set; }
        public int VerseNumber { get; set; }
        public int ChapterNumber { get; set; }
        public string BookName { get; set; }
        public string Testament { get; set; }
        //public Book Book { get; set; }
        public bool IsMemorized { get; set; }
        public int BookPosition { get; set; }
        public DateTime LastRecited { get; set; }
        public string FullTitle => $"{BookName} {ChapterNumber}:{VerseNumber}";
        public override string ToString() => $"{BookName} {ChapterNumber}:{VerseNumber}";

    }
}