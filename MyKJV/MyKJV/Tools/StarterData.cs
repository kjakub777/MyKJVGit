using Acr.UserDialogs;

using MyKJV.Models;
using MyKJV.Services;

using SQLite;

using SQLiteNetExtensions.Extensions;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyKJV.Tools
{

    public class StarterData
    {

        public static Bible GetMemorized()
        {
            return GetData("MyKJV.Data.MostRecentVerses.csv");
        }

        internal static async Task EnsureDataExists()
        {
            int count = -1;
            try
            {
                using (var db = new DatabaseConnection().DbConnection())
                    count = db.Table<Verse>().Count();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Toast($"{ex.Message}.");
            }
            UserDialogs.Instance.Toast($"{count} rows in DB...");
            if (count < 30000)
            {
                UserDialogs.Instance.Toast($"Importing...");
                await Task.Run(() => ImportDBOrigCSV());
                UserDialogs.Instance.Toast("Imported DB from CSV.");
            }
        }

        public static void SetMemorizedDB(string resource)
        {
            var db = new DatabaseConnection().DbConnection();

            int lineNum = 0;
            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Verse)).Assembly;

                //foreach (var res in assembly.GetManifestResourceNames())
                //    info += "found resource: " + res;
                //Debug.WriteLine(info);
                Stream stream = assembly.GetManifestResourceStream(resource);
                //Stream stream = assembly.GetManifestResourceStream("BibleMemoryAssistant.Data.KJVWHOLE.csv");
                string text = "";
                if (stream != null)
                {
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        text = reader.ReadToEnd();
                    }
                }
                else
                    Debug.WriteLine("SHIT");

                var csv = text.Split('\n');

                Regex regexBook = new Regex(@"[\d]* ?[A-z]+\b");
                Regex regexChaptVerse = new Regex(@"\b[\d]+:[\d]+");
                string book = ""; int ch = 0, vn = 0;
                for (int i = 0; i < csv.Length; i++)
                {
                    lineNum++;
                    string line = csv[i];
                    book = regexBook.Match(line).Value;
                    var arr = regexChaptVerse.Match(line).Value.Split(':');
                    ch = int.Parse(arr[0]);
                    vn = int.Parse(arr[1]);
                    // string strVerseContent = line.Substring(line.IndexOf(',') + 1);
                    var v = db.Find<Verse>(x => x.BookName == book && x.ChapterNumber == ch && x.VerseNumber == vn);
                    if (v != null)
                    {
                        v.IsMemorized = true;
                        db.Update(v);
                    }
                    else
                        Debug.WriteLine("NULL1\n" + line);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("THIS LINE " + lineNum + "\n" + ex.ToString());
            }

        }
        public static Bible GetData(string resource)
        {

            Testament oldTestament = new Testament() { Id = Guid.NewGuid(), Name = "Old Testament", Books = new List<Book>() };
            Testament newTestament = new Testament() { Id = Guid.NewGuid(), Name = "New Testament", Books = new List<Book>() };
            Bible bible = new Bible()
            {
                Id = Guid.NewGuid(),
                OldTestamentId = oldTestament.Id,
                NewTestamentId = newTestament.Id,
                Name = "KJV",
                OldTestament = oldTestament,
                NewTestament = newTestament
            };
            int lineNum = 0;
            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Verse)).Assembly;

                //foreach (var res in assembly.GetManifestResourceNames())
                //    info += "found resource: " + res;
                //Debug.WriteLine(info);
                Stream stream = assembly.GetManifestResourceStream(resource);
                //Stream stream = assembly.GetManifestResourceStream("BibleMemoryAssistant.Data.KJVWHOLE.csv");
                string text = "";
                if (stream != null)
                {
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        text = reader.ReadToEnd();
                    }
                }
                else
                    Debug.WriteLine("SHIT");

                var csv = text.Split('\n');



                Book currentBook = null;//new Book() { Name = "Genesis", Position = 1 };
                string strCurrentBook = "";
                int intCurrentChapter = 0;
                Chapter currentChapter = null;//new Chapter() { Number = 1 };

                Regex regexBook = new Regex(@"[\d]* ?[A-z]+\b");
                Regex regexChaptVerse = new Regex(@"\b[\d]+:[\d]+");
                bool isOldTestament = true;
                int bookIndex = 1;
                for (int i = 0; i < csv.Length; i++)
                {
                    lineNum++;
                    string line = csv[i];
                    string strRowBook = regexBook.Match(line).Value;
                    if (isOldTestament && strRowBook == "Matthew")
                        isOldTestament = false;
                    var arrChapterVerse = regexChaptVerse.Match(line).Value.Split(':');
                    int intRowChapter = int.Parse(arrChapterVerse[0].Trim());
                    int intVerseNum = int.Parse(arrChapterVerse[1].Trim());
                    string strVerseContent = line.Substring(line.IndexOf(',') + 1);

                    if (strCurrentBook != strRowBook)
                    {
                        //need to create new
                        currentBook = new Book { Id = Guid.NewGuid(), Name = strRowBook, Position = bookIndex++, Chapters = new List<Chapter>() };
                        strCurrentBook = strRowBook;
                        currentChapter = new Chapter() { Id = Guid.NewGuid(), Number = intRowChapter, Book = currentBook, BookId = currentBook.Id };
                        intCurrentChapter = intRowChapter;
                        currentBook.Chapters.Add(currentChapter);
                        if (isOldTestament)
                        {
                            currentBook.Testament = oldTestament;
                            oldTestament.Books.Add(currentBook);
                        }
                        else
                        {
                            currentBook.Testament = newTestament;
                            newTestament.Books.Add(currentBook);
                        }
                    }
                    else if (intCurrentChapter != intRowChapter)
                    {
                        //new chapter
                        currentChapter = new Chapter() { BookId = currentBook.Id, Number = intRowChapter, Book = currentBook };
                        intCurrentChapter = intRowChapter;
                        currentBook.Chapters.Add(currentChapter);
                    }

                    currentChapter.Verses.Add(new Verse()
                    {
                        Text = strVerseContent,
                        //Chapter = currentChapter,
                        //ChapterId = currentChapter.Id,
                        //Id = Guid.NewGuid(),
                        IsMemorized = false,
                        VerseNumber = intVerseNum
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("THIS LINE " + lineNum + "\n" + ex.ToString());
            }


            return bible;
        }
        /*  public static Bible GetData(string resource)
        {

            Testament oldTestament = new Testament() { Id = Guid.NewGuid(), Name = "Old Testament", Books = new List<Book>() };
            Testament newTestament = new Testament() { Id = Guid.NewGuid(), Name = "New Testament", Books = new List<Book>() };
            Bible bible = new Bible()
            {
                Id = Guid.NewGuid(),
                OldTestamentId = oldTestament.Id,
                NewTestamentId = newTestament.Id,
                Name = "KJV",
                OldTestament = oldTestament,
                NewTestament = newTestament
            };
            int lineNum = 0;
            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Verse)).Assembly;

                //foreach (var res in assembly.GetManifestResourceNames())
                //    info += "found resource: " + res;
                //Debug.WriteLine(info);
                Stream stream = assembly.GetManifestResourceStream(resource);
                //Stream stream = assembly.GetManifestResourceStream("BibleMemoryAssistant.Data.KJVWHOLE.csv");
                string text = "";
                if (stream != null)
                {
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        text = reader.ReadToEnd();
                    }
                }
                else
                    Debug.WriteLine("SHIT");

                var csv = text.Split('\n');



                Book currentBook = null;//new Book() { Name = "Genesis", Position = 1 };
                string strCurrentBook = "";
                int intCurrentChapter = 0;
                Chapter currentChapter = null;//new Chapter() { Number = 1 };

                Regex regexBook = new Regex(@"[\d]* ?[A-z]+\b");
                Regex regexChaptVerse = new Regex(@"\b[\d]+:[\d]+");
                bool isOldTestament = true;
                int bookIndex = 1;
                for (int i = 0; i < csv.Length; i++)
                {
                    lineNum++;
                    string line = csv[i];
                    string strRowBook = regexBook.Match(line).Value;
                    if (isOldTestament && strRowBook == "Matthew")
                        isOldTestament = false;
                    var arrChapterVerse = regexChaptVerse.Match(line).Value.Split(':');
                    int intRowChapter = int.Parse(arrChapterVerse[0].Trim());
                    int intVerseNum = int.Parse(arrChapterVerse[1].Trim());
                    string strVerseContent = line.Substring(line.IndexOf(',') + 1);

                    if (strCurrentBook != strRowBook)
                    {
                        //need to create new
                        currentBook = new Book { Id = Guid.NewGuid(), Name = strRowBook, Position = bookIndex++, Chapters = new List<Chapter>() };
                        strCurrentBook = strRowBook;
                        currentChapter = new Chapter() { Id = Guid.NewGuid(), Number = intRowChapter, Book = currentBook, BookId = currentBook.Id };
                        intCurrentChapter = intRowChapter;
                        currentBook.Chapters.Add(currentChapter);
                        if (isOldTestament)
                        {
                            currentBook.Testament = oldTestament;
                            oldTestament.Books.Add(currentBook);
                        }
                        else
                        {
                            currentBook.Testament = newTestament;
                            newTestament.Books.Add(currentBook);
                        }
                    }
                    else if (intCurrentChapter != intRowChapter)
                    {
                        //new chapter
                        currentChapter = new Chapter() { BookId = currentBook.Id, Number = intRowChapter, Book = currentBook };
                        intCurrentChapter = intRowChapter;
                        currentBook.Chapters.Add(currentChapter);
                    }

                    currentChapter.Verses.Add(new Verse()
                    {
                        Text = strVerseContent,
                        Chapter = currentChapter,
                        ChapterId = currentChapter.Id,
                        Id = Guid.NewGuid(),
                        IsMemorized = false,
                        VerseNumber = intVerseNum
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("THIS LINE " + lineNum + "\n" + ex.ToString());
            }


            return bible;
        }*/

        public static void ExportDb(SQLiteConnection db)
        {
            if (db == null)
                db = new DatabaseConnection().DbConnection();
            var filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DBBack.txt");
            if (File.Exists(filepath))
                File.Delete(filepath);
            FileStream fs = new FileStream(filepath, FileMode.CreateNew);

            var vs = db.Table<Verse>();

            foreach (var v in vs)
            {
                string str = $"insert into verse(Id, Testament, BookPosition, BookName, ChapterNumber, VerseNumber, Text, LastRecited, IsMemorized) VALUES " +
                $"('{v.Id}', '{v.Testament}', '{v.BookPosition}', '{v.BookName}', '{v.ChapterNumber}', '{v.VerseNumber}', '{v.Text.Replace("\r", "").Replace("'", "''")}'," +
                $"'{v.LastRecited.Ticks}', '{(v.IsMemorized ? "1" : "0")}');\n";
                fs.Write(ASCIIEncoding.ASCII.GetBytes(str), 0, ASCIIEncoding.ASCII.GetByteCount(str));
            }
            fs.Flush();
            fs.Close();
            UserDialogs.Instance.Toast($"DB exported {vs.Count()} records.");
        }
        public static void ImportDb(SQLiteConnection db)
        {
            if (db == null)
                db = new DatabaseConnection().DbConnection();
            db.DropTable<Verse>();
            db.CreateTable<Verse>();
            var filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DBBack.txt");
            if (File.Exists(filepath))
            {
                var vs = File.ReadAllLines(filepath);

                foreach (var row in vs)
                {
                    try
                    {

                        db.Execute(row);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("" + ex);
                    }
                }
                var count = db.Table<Verse>().Count();
                UserDialogs.Instance.Toast($"DB imported {count} records.");
            }
        }

        public static void GetDataInsertDB(string resource)
        {

            CreateTables();
            var db = new DatabaseConnection().DbConnection();

            int lineNum = 0;
            try

            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Verse)).Assembly;

                //foreach (var res in assembly.GetManifestResourceNames())
                //    info += "found resource: " + res;
                //Debug.WriteLine(info);
                Stream stream = assembly.GetManifestResourceStream(resource);
                string text = "";
                if (stream != null)
                {
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        text = reader.ReadToEnd();
                    }
                }
                else
                    Debug.WriteLine("SHIT");

                var csv = text.Split('\n');



                string strCurrentBook = "";

                Regex regexBook = new Regex(@"[\d]* ?[A-z]+\b");
                Regex regexChaptVerse = new Regex(@"\b[\d]+:[\d]+");
                bool isOldTestament = true;
                int bookIndex = 0;
                for (int i = 0; i < csv.Length; i++)
                {
                    lineNum++;
                    string line = csv[i];
                    string strRowBook = regexBook.Match(line).Value;
                    if (isOldTestament && strRowBook == "Matthew")
                        isOldTestament = false;
                    var arrChapterVerse = regexChaptVerse.Match(line).Value.Split(':');
                    int intRowChapter = int.Parse(arrChapterVerse[0].Trim());
                    int intVerseNum = int.Parse(arrChapterVerse[1].Trim());
                    string strVerseContent = line.Substring(line.IndexOf(',') + 1);

                    if (strCurrentBook != strRowBook)
                    {
                        strCurrentBook = strRowBook;
                        bookIndex++;

                    }
                    var v = new Verse()
                    {
                        Text = strVerseContent,
                        ChapterNumber = intRowChapter,
                        BookName = strRowBook,
                        Testament = isOldTestament ? "Old" : "New",
                        //Id = i+1,
                        LastRecited = DateTime.Now.AddDays(-30),
                        IsMemorized = false,
                        VerseNumber = intVerseNum,
                        BookPosition = bookIndex
                    };
                    db.Insert(v);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("THIS LINE " + lineNum + "\n" + ex.ToString());
            }

            var vs = db.Table<Verse>().Count();

        }
        private static void CreateTables()
        {
            try
            {
                using (var db = new DatabaseConnection().DbConnection())
                {
                    try
                    {

                        db.DropTable<Verse>();
                    }
                    catch (Exception ex)
                    {

                    }
                    db.CreateTable<Verse>(SQLite.CreateFlags.None);
                    var nd = db.DeleteAll<Verse>();
                }
            }
            catch (Exception ex)
            {

            }
        }

        internal static void ClearDb(object p)
        {
            try
            {
                var db = new DatabaseConnection().DbConnection();
                db.DeleteAll<Verse>();
                UserDialogs.Instance.Toast("DB cleared");
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Toast($"{ex.Message}");
            }
        }
        internal static void ImportDBOrigCSV()
        {
            GetDataInsertDB("MyKJV.Data.KJVWHOLE.csv");
            SetMemorizedDB("MyKJV.Data.MostRecentVerses.csv");
            UserDialogs.Instance.Toast("Imported DB from CSV.");
        }
    }


}