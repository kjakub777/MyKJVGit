//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using SQLite;
//using MyKJV.Droid;
//using System.IO;
//using MyKJV.Data;

//[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection))]
//namespace MyKJV.Droid
//{
//    public class DatabaseConnection : IDatabaseConnection
//    {
//        public SQLiteConnection DbConnection()
//        { 
//            return new SQLiteConnection(DbPath());
//        }
//        public string DbPath()
//        {
//            var dbName = "MyKJVDb.db3";
//            var path = Path.Combine( Android.OS.Environment.DataDirectory.AbsolutePath, dbName);
//            return path;
//        }
//        /// <summary>
//        /// oublic files
//        /// </summary>
//        /// <returns></returns>
//        public string ExternalStoragePath()
//        {
//            var path = Android.OS.Environment
//                .GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments)
//                .AbsolutePath;
//            return path;
//        }
//        /// <summary>
//        /// privat files
//        /// </summary>
//        /// <returns></returns>
//        public string ExternalStoragePathPrivate()
//        {
//            var path = Android.OS.Environment.DataDirectory.AbsolutePath;
//            return path;
//        }
//    }
//}