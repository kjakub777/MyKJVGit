 
using SQLite;
using MyKJV.Droid;
using System.IO;
using MyKJV.Data;

using Xamarin.Forms;
[assembly: Dependency(typeof(SQLiteDbConnection))]

namespace MyKJV.Droid
{
  public  class SQLiteDbConnection:IDatabaseConnection
    {
        const string dbNAME="MyKJVDb.db3";
        public SQLiteConnection DbConnection()
        {
            return new SQLiteConnection(DbPath());
        }

        public string DbPath()
        { 
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbNAME);
            return path;
        }
        /// <summary>
        /// oublic files
        /// </summary>
        /// <returns></returns>
        public string ExternalStoragePath()
        {
            //var p = Android.Support.V4.Content.ContextCompat
            //   .CheckSelfPermission(null, Android.Manifest.Permission.WriteExternalStorage);
            //if (p != (int)Android.Content.PM.Permission.Granted)
            //    System.Diagnostics.Debug.WriteLine("NO PERMISSION");
            //else
            //    System.Diagnostics.Debug.WriteLine("PERMISSION!!");
            var path = Android.OS.Environment
                .GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments)
                .AbsolutePath;
           // path = "/sdcard/";
            return path;
        }
        /// <summary>
        /// privat files
        /// </summary>
        /// <returns></returns>
        public string ExternalStoragePath2()
        {
        //var p=    Android.Support.V4.Content.ContextCompat
        //            .CheckSelfPermission(null, Android.Manifest.Permission.WriteExternalStorage);
        //    if(p != (int)Android.Content.PM.Permission.Granted)
        //        System.Diagnostics.Debug.WriteLine("NO PERMISSION");
        //    else  
        //        System.Diagnostics.Debug.WriteLine("PERMISSION!!");
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return path;
        }
   
    }
}