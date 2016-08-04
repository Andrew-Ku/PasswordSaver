using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SavePassword.Models;
using SQLite;

namespace SavePassword.Infrastructure.Impl
{
    public class AccountRepository
    {
        SQLiteConnection database;
        public AccountRepository(string filename)
        {
            database = new SqLite().GetConnection(filename);
            database.CreateTable<Account>();
        }
        public IEnumerable<Account> GetItems()
        {
            return (from i in database.Table<Account>() select i).ToList();
        }
        public Account GetItem(int id)
        {
            return database.Get<Account>(id);
        }
        public int DeleteItem(int id)
        {
            return database.Delete<Account>(id);
        }
        public int SaveItem(Account item)
        {
            if (item.Id != 0)
            {
                database.Update(item);
                return item.Id;
            }
            else
            {
                return database.Insert(item);
            }
        }
    }
}