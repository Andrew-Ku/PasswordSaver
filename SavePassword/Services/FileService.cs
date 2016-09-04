using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SavePassword.Consts;
using SavePassword.Infrastructure.Impl;
using SavePassword.Models;
using Environment = Android.OS.Environment;


namespace SavePassword.Services
{
    public class FileService
    {
        private readonly AccountRepository _accountRepository;
        private readonly CryptService _cryptService;

        public FileService()
        {
            _accountRepository = new AccountRepository("accounts");
            _cryptService = new CryptService();
        }

        public bool ExportToExcel()
        {
            var path = System.IO.Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath,FileApp.FileDataDirectory);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            var model = new AccountsFileJson
            {
                Accounts = _accountRepository.GetItems().ToList(),
                DocDate = DateTime.Now
            };

            foreach (var account in model.Accounts)
            {
                account.Password = _cryptService.DecryptText(account.Password, "test");
            }

            try
            {
                var modelJson = JsonConvert.SerializeObject(model);
                System.IO.File.WriteAllText(System.IO.Path.Combine(path, 
                    $"{"Passwords "}{model.DocDate.ToShortDateString()}.{"json"}"), modelJson);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;

        }

        public AccountsFileJson ImportFromExcel(string path)
        {
            var model = new AccountsFileJson();

            try
            {
                var file = File.ReadAllText(path);
                 model = JsonConvert.DeserializeObject<AccountsFileJson>(file);

                foreach (var account in model.Accounts)
                {
                    account.Password = _cryptService.DecryptText(account.Password, "test");

                }

                return model;
            }
            catch (Exception e)
            {
                model.resultMes = "Ошибка чтения файла";
                return model;
            }
        }
    }
}