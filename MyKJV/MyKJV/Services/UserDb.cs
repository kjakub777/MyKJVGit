using Acr.UserDialogs;
using MyKJV.Data;

using SQLite;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Xamarin.Forms;

namespace MyKJV.Services
{
    public class UserDB
    {
        public IDatabaseConnection DbWrapper;
        public SQLiteConnection DbConn;
        public UserDB()
        {
            DbWrapper = DependencyService.Get<IDatabaseConnection>();
            DbConn = DbWrapper.DbConnection();
        }
        public byte[] Database()
        {
            byte[] buffer = null;
            try
            {
                FileStream myfilestream = new FileStream(DbPath, FileMode.Open, FileAccess.Read);
                buffer = new byte[myfilestream.Length];
                myfilestream.Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert($"Error reading Db:\n{ex}", "Info", "Ugh");
            }
            return buffer;
        }
        public string ExternalStoragePath => DbWrapper.ExternalStoragePath();
        public string ExternalStoragePathPrivate => DbWrapper.ExternalStoragePath2();
        public string DbPath => DbWrapper.DbPath();
    }
}
