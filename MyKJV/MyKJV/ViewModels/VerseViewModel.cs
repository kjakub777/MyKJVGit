using MyKJV.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyKJV.ViewModels
{
 public   class VerseViewModel
    {
        
        private Verse _verse;

        public VerseViewModel(Verse verse)
        {
            this._verse = verse;
        }

        public string sLastRecited => _verse?. LastRecited.ToShortDateString();

        public string VerseName { get { return _verse.ChapVerseText; } }
    //    public int TypeID { get { return _verse.IdTypeID; } }

        public Verse Verse
        {
            get => _verse;
        }
    }
}
