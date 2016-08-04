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
using SavePassword.Infrastructure.Impl;
using SavePassword.Models;
using SavePassword.Services;

namespace SavePassword
{
    [Activity(Label = "CreateAccount")]
    public class CreateAccount : Activity
    {
        private  AccountRepository _repository;
        private  CryptService _cryptService;
       
        Button createButton;
        EditText nameEditText;
        EditText descriptionEditText;
        EditText passwordText;
        EditText passswordRepeatText;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _repository = new AccountRepository("accounts");
            _cryptService = new CryptService();

            SetContentView(Resource.Layout.CreateAccount);

            nameEditText = FindViewById<EditText>(Resource.Id.nameEditText);
            passwordText = FindViewById<EditText>(Resource.Id.passwordEditView);
            passswordRepeatText = FindViewById<EditText>(Resource.Id.passwordRepeatEditView);
            descriptionEditText = FindViewById<EditText>(Resource.Id.descriptionEditText);

            createButton = FindViewById<Button>(Resource.Id.CreateAccountButton);
           
            createButton.Click+=CreateButtonOnClick;
        }

        private void CreateButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (nameEditText.Text.Trim() == string.Empty
                || passwordText.Text.Trim() == string.Empty
                || passswordRepeatText.Text.Trim() == string.Empty)
            {
                return;
            }

            if (passwordText.Text != passswordRepeatText.Text)
            {
                return;
            }

            var account = new Account()
            {
                Name = nameEditText.Text,
                Password = _cryptService.EncryptText(passwordText.Text,"test"),
                Description = descriptionEditText.Text,
                CreateDate = DateTime.Now,
                GroupId = 1
            };

            _repository.SaveItem(account);

            this.StartActivity(new Intent(this, typeof(Activity2)));
            Finish();
        }

        public override void OnBackPressed()
        {
            this.StartActivity(new Intent(this, typeof(Activity2)));
            base.OnBackPressed();
        }
    }
}