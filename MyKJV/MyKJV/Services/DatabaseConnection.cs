


using MyKJV.Data;

using SQLite;

using System;
using System.IO;

namespace MyKJV.Services
{public class Helper
    {
        public static string ExternalStoragePath
        {
            get
            {
                var dbName = "MyKJVDb.db3";
                /*#if __IOS__
                    string folder = Environment.GetFolderPath
                      (Environment.SpecialFolder.Personal);
                    folder = Path.Combine (folder, "..", "Library");
                    var databasePath = Path.Combine(folder, dbName);
                #else
                #if __ANDROID__
                    string folder = Environment.GetFolderPath
                      (Environment.SpecialFolder.Personal);
                    var databasePath = Path.Combine(folder, dbName);
                #else  // WinPhone
                                var databasePath =
                                  Path.Combine(Xamarin.Forms.PlatformConfiguration.Windows.Storage.ApplicationData.Current.
                                  LocalFolder.Path, dbName);
                #endif
                #endif
                                return databasePath;*/
                string folder = Environment.GetFolderPath
                     (Environment.SpecialFolder.Personal);
                var databasePath = Path.Combine(folder, dbName);
                return databasePath;
            }
        }

    }
    //public class DatabaseConnection :IDatabaseConnection
    //{
    //    public virtual string databasePath
    //    {
    //        get
    //        {
    //            var dbName = "NOTRIGHTMyKJVDb.db3";
    //            /*#if __IOS__
    //                string folder = Environment.GetFolderPath
    //                  (Environment.SpecialFolder.Personal);
    //                folder = Path.Combine (folder, "..", "Library");
    //                var databasePath = Path.Combine(folder, dbName);
    //            #else
    //            #if __ANDROID__
    //                string folder = Environment.GetFolderPath
    //                  (Environment.SpecialFolder.Personal);
    //                var databasePath = Path.Combine(folder, dbName);
    //            #else  // WinPhone
    //                            var databasePath =
    //                              Path.Combine(Xamarin.Forms.PlatformConfiguration.Windows.Storage.ApplicationData.Current.
    //                              LocalFolder.Path, dbName);
    //            #endif
    //            #endif
    //                            return databasePath;*/
    //            string folder = Environment.GetFolderPath
    //                 (Environment.SpecialFolder.Personal);
    //            var databasePath = Path.Combine(folder, dbName);
    //            return databasePath;
    //        }
    //    }

    //    public   SQLiteConnection DbConnection()
    //    {
    //      return  new SQLiteConnection(databasePath);
    //    }

    //    public string DbPath()
    //    {
    //        return "";
    //    }

    //    public string ExternalStoragePath()
    //    {
    //        return" Xamarin.Forms.PlatformConfiguration.Windows.Storage.ApplicationData.Current.";
    //    }

    //    public string ExternalStoragePathPrivate()
    //    {
    //        return "";
    //    }
    //}


}