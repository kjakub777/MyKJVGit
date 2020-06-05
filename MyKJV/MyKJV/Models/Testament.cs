using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.ComponentModel;
using SQLiteNetExtensions.Attributes;

namespace MyKJV.Models
{[Table ("Testament")]
    public class Testament
    {
        public Testament()
        {
            //Id = Guid.NewGuid();   
        }
        [PrimaryKey]
        public Guid Id { get; set; }
        [OneToMany(CascadeOperations =CascadeOperation.All)]
        public List<Book> Books { get; set; }
        public string Name { get; set; } 
    }
}