using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SavePassword.Infrastructure.Impl;
using SQLite;
using Environment = System.Environment;

//[assembly: Dependency(typeof(SqLite))]
namespace SavePassword.Infrastructure.Impl
{

    public class SqLite:ISQLite 
    {
        public SqLite() { }
        public SQLiteConnection GetConnection(string sqliteFilename)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            // создаем подключение
            var conn = new SQLiteConnection(path);

            return conn;
        }
    }
}