using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.ComponentModel;
using SQLiteNetExtensions.Attributes;

namespace MyKJV.Models
{
    [Table("Book")]
    public class Book
    {
        public Book()
        {
            //Id = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid Id { get; set; }

        List<Chapter> chapters;
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Chapter> Chapters
        {
            get
            {
                if(chapters == null)
                    chapters = new List<Chapter>();
                return chapters;
            }
            set { this.chapters = value; }
        }

        [ForeignKey(typeof(Testament))]
        public Guid TestamentId { get; set; }

        [ManyToOne]
        public Testament Testament { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }
    }
    //public class AnimalGroup : List<Animal>
    //{
    //    public string Name { get; private set; }

    //    public AnimalGroup(string name, List<Animal> animals) : base(animals)
    //    {
    //        Name = name;
    //    }
    //}
    public class MemoryBookGroup:List<Verse>
    {
        public MemoryBookGroup(string name, List<Verse> vs):base (vs)
        {           
            Name = name;
            //Id = Guid.NewGuid();
        }


        //List<Verse> verses;

        //public List<Verse> Verses
        //{
        //    get
        //    {
        //        if(verses == null)
        //            verses = new List<Verse>();
        //        return verses;
        //    }
        //    set { this.verses = value; }
        //}

        public string Name { get; set; }

        //public int Position { get; set; }
    }
}