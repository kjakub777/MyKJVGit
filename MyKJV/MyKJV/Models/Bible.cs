using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.ComponentModel;
using Xamarin.Forms;
using SQLiteNetExtensions.Attributes;
//https://bitbucket.org/twincoders/sqlite-net-extensions/src/master/
namespace MyKJV.Models
{
    [Table("Bible")]
    public class Bible
    {
        public Bible()
        {
            //Id = Guid.NewGuid();
        }
        [PrimaryKey ] 
        public Guid Id { get; set; }

        // [One]
        [ForeignKey(typeof(Testament))]
        public Guid OldTestamentId { get;set;}
        
        [ForeignKey(typeof(Testament))]
       public Guid NewTestamentId { get;set;}
        [OneToOne]
        public Testament OldTestament { get; set; }
        [OneToOne]
        public Testament NewTestament { get; set; }
        public string Name { get; set; }
    }
}