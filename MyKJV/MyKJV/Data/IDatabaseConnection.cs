using System;
using System.Collections.Generic;
using System.Text;

namespace MyKJV.Data
{
    public interface IDatabaseConnection
    {
        SQLite.SQLiteConnection DbConnection();

        string DbPath();

        /// <summary>
        /// oublic files
        /// </summary>
        /// <returns></returns>
        string ExternalStoragePath();

        /// <summary>
        /// privat files
        /// </summary>
        /// <returns></returns>
        string ExternalStoragePath2();
         
    }
}
