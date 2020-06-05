using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.ComponentModel;
using SQLiteNetExtensions.Attributes;

namespace MyKJV.Models
{
    [Table("Chapter")]
    public class Chapter
    {
        public Chapter()
        {
            //Id = Guid.NewGuid();
        }
        [PrimaryKey]
        public Guid Id { get; set; }
        List<Verse> verses;

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Verse> Verses
        {
            get
            {
                if (verses == null)
                    verses = new List<Verse>();
                return this.verses;
            }
            set
            {
                this.verses = value;
            }
        }
        [ForeignKey(typeof(Book))]
        public Guid BookId { get; set; }
        [ManyToOne]
        public Book Book { get; set; }
        public int Number { get; set; }
    }
}