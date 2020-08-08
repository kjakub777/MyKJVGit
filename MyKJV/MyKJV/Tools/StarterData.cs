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
using System.Runtime.CompilerServices;
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



        public static async Task SetMemorizedDB(string resource, Action<double, uint> animatePbar)
        {
            UserDB dcon = new UserDB();

            var db = dcon.DbConn;

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
                        text = await reader.ReadToEndAsync();
                    }
                }
                else
                    Debug.WriteLine("SHIT");

                var csv = text.Split('\n');
                double dval = 0;
                uint length = (uint)csv.Length;
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
                    animatePbar?.Invoke(++dval, length);
                    //else
                    //    Debug.WriteLine("NULL1\n" + line);
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
        public static async Task<bool> ImportDb(string importPath, Action<double, uint> action)
        {

            UserDB dcon = new UserDB();
            var db = dcon.DbConn;

            var filepath = importPath ?? throw new ArgumentNullException("ImportPath"); //string.IsNullOrEmpty(importPath)? Path.Combine(path, "DBBack.txt"):importPath;

            db.DropTable<Verse>();
            db.CreateTable<Verse>();

            if (File.Exists(filepath))
            {
                var vs = await Task.FromResult(File.ReadAllLines(filepath));
                double val = 0d;
                uint max = (uint)vs.Length;
                await Task.Factory.StartNew(() =>
                {
                    foreach (var row in vs)
                    {
                        try
                        {
                            db.Execute(row);

                            action?.Invoke(++val, max);//progress bar invoke on main
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("" + ex);
                        }
                    }

                    UserDialogs.Instance.Alert($"DB imported {val} records.");
                    return val > 0;
                });
            }
            return false;
        }

        public static async Task<bool> ExportDb(string exportPath, Action<double, uint> action, bool toFtp)
        {
            StringBuilder builder = new StringBuilder();

            try
            {
                //try to upload it to ftp
                UserDB dcon = new UserDB();
                var db = dcon.DbConn;

                var filepath = exportPath ?? throw new ArgumentNullException("ExportPath"); //string.IsNullOrEmpty(importPath)? Path.Combine(path, "DBBack.txt"):importPath;

                //var filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DBBack.txt");
                if (!Directory.Exists(Path.GetDirectoryName(filepath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filepath));
                if (File.Exists(filepath))
                    File.Delete(filepath);

                var vs = db.Table<Verse>();
                double val = 0d;
                uint max = (uint)vs.Count();
                foreach (var v in vs)
                {
                    string str = $"insert into verse(Id, Testament, BookPosition, BookName, ChapterNumber, VerseNumber, Text, LastRecited, IsMemorized) VALUES " +
                    $"('{v.Id}', '{v.Testament}', '{v.BookPosition}', '{v.BookName}', '{v.ChapterNumber}', '{v.VerseNumber}', '{v.Text.Replace("\r", "").Replace("'", "''")}'," +
                    $"'{v.LastRecited.Ticks}', '{(v.IsMemorized ? "1" : "0")}');\n";
                    builder.Append(str);
                    action?.Invoke(++val, max);//progress bar invoke on main
                }

                FileStream fs = new FileStream(filepath, FileMode.CreateNew);
                fs.Flush();
                fs.Close();
                UserDialogs.Instance.Alert($"DB exported {vs.Count()} records.");
                return true;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert($"Error writing text file.{ex}");
            }
            try
            {
                if (toFtp)
                {
                    await FtpHandler.SendDbToFTP("db.Txt", file: ASCIIEncoding.ASCII.GetBytes(builder.ToString()));

                    await FtpHandler.SendDbToFTP();
                }

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert($"Error sending to ftp{ex}");
            }
            return false;

        }
        public static async Task GetDataInsertDB(string resource, Action<double, uint> animatePbar)
        {

            CreateTables();
            UserDB dcon = new UserDB();
            var db = dcon.DbConn;


            int lineNum = 0;
            try

            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Verse)).Assembly;

                //foreach (var res in assembly.GetManifestResourceNames())
                //    info += "found resource: " + res;
                //Debug.WriteLine(info);
                Stream stream = assembly.GetManifestResourceStream(resource);
                List<string> csv = new List<string>();
                if (stream != null)
                {
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        while (-1 < reader.Peek())
                        {
                            csv.Add(await reader.ReadLineAsync());
                        }
                    }
                }
                else
                    Debug.WriteLine("SHIT");

                string strCurrentBook = "";
                double dval = 0;
                uint length = (uint)csv.Count;
                Regex regexBook = new Regex(@"[\d]* ?[A-z]+\b");
                Regex regexChaptVerse = new Regex(@"\b[\d]+:[\d]+");
                bool isOldTestament = true;
                int bookIndex = 0;
                for (int i = 0; i < length; i++)
                {
                    //if (i == max)
                    //    break;
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
                        LastRecited = DateTime.Now.AddDays(-3000),
                        IsMemorized = false,
                        VerseNumber = intVerseNum,
                        BookPosition = bookIndex
                    };
                    db.Insert(v);
                    animatePbar?.Invoke(++dval, length);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("THIS LINE " + lineNum + "\n" + ex.ToString());
            }

            var vs = db.Table<Verse>().Count();

        }
        private static void CreateTables()
        {
            try
            {
                UserDB dcon = new UserDB();

                using (var db = dcon.DbConn)
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

        internal static async Task ClearDb(object p)
        {
            try
            {
                UserDB dcon = new UserDB();

                using (var db = dcon.DbConn)
                    db.DeleteAll<Verse>();
                UserDialogs.Instance.Toast("DB cleared");
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Toast($"{ex.Message}");
            }
        }
        public static async Task ImportDBOrigCSV(Action<double, uint> animatePbar)
        {
            int count = -1;
            UserDB dcon = new UserDB();
            bool createAndInsert = false;
            var db = dcon.DbConn;
            try
            {
                count = db.Table<Verse>().Count();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Toast($"{ex.Message}.");
            }
            UserDialogs.Instance.Confirm(new ConfirmConfig()
            {
                CancelText = "Forget it.",
                Message = $"There are {count} rows in DB, import from csv?",
                OkText = "Sounds like a plan!",
                Title = "Data Issue",
                OnAction = delegate (bool confirm) { createAndInsert = confirm; }
            });
            if (createAndInsert)
            {
                UserDialogs.Instance.Toast($"Data!..{count}");
                await GetDataInsertDB("MyKJV.Data.KJVWHOLE.csv", animatePbar);

                await SetMemorizedDB("MyKJV.Data.MostRecentVerses.csv", animatePbar);
                UserDialogs.Instance.Alert($"Created db records successfully.");
            }
        }
    }


}