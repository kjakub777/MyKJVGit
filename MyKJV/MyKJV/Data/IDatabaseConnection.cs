using System;
using System.Collections.Generic;
using System.Text;

namespace MyKJV.Data
{
    public interface IDatabaseConnection
    {        SQLite.SQLiteConnection DbConnection();
    }
}
