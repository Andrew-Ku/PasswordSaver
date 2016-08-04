using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.InputMethodServices;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;
using Newtonsoft.Json;
using SavePassword.Consts;

namespace SavePassword
{
    [Activity(Label = "SavePassword", Icon = "@drawable/icon", MainLauncher = true)]
    public class MainActivity : Activity
    {
        RelativeLayout mRelativeLayout;
        Button passwordBtn;
        TextView title;
        EditText passwordText;
        EditText passswordRepeatText;
        const string PASSWORD_REFERENCES = "masterPassword";
        int mode;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            mRelativeLayout = FindViewById<RelativeLayout>(Resource.Id.mainView);
            title = FindViewById<TextView>(Resource.Id.txtTitle);
            passwordText = FindViewById<EditText>(Resource.Id.txtPassword);
            passswordRepeatText = FindViewById<EditText>(Resource.Id.txtRepeatPassword);
            passwordBtn = FindViewById<Button>(Resource.Id.btnPassword);
            passwordBtn.SetTypeface(Typeface.Default, TypefaceStyle.Normal);

            var settings = Application.Context.GetSharedPreferences(PASSWORD_REFERENCES, FileCreationMode.Private);
            var password = settings.GetString("Password", string.Empty);

            mode = Intent.GetIntExtra("mode", 0);

            if (mode != (int)Mode.ChangeMasterPassword)
            {
                if (password == string.Empty)
                {
                    mode = (int)Mode.Create;
                    title.Text = "Создание мастер-пароля";
                    passswordRepeatText.Visibility = ViewStates.Visible;
                }
                else
                {
                    mode = (int)Mode.Confirm;
                    passswordRepeatText.Visibility = ViewStates.Gone;
                    title.Text = "Ввод мастер-пароля";
                    passwordBtn.Text = "Вход";

                }

            }
            else
            {
                passswordRepeatText.Visibility = ViewStates.Visible;
                title.Text = "Смена мастер-пароля";
                passwordBtn.Text = "Сохранить";
                passswordRepeatText.Hint = "Новый пароль";
            }


            passwordBtn.Click += mButton_Click;
            mRelativeLayout.Click += MRelativeLayoutOnClick;


        }

        private void MRelativeLayoutOnClick(object sender, EventArgs eventArgs)
        {
            InputMethodManager manager = (InputMethodManager)GetSystemService(InputMethodService);
            manager.HideSoftInputFromWindow(passwordText.WindowToken, 0);
            manager.HideSoftInputFromWindow(passswordRepeatText.WindowToken, 0);
        }

        void mButton_Click(object sender, EventArgs e)
        {
            var settings = Application.Context.GetSharedPreferences(PASSWORD_REFERENCES, FileCreationMode.Private);
            var passwordPref = settings.GetString("Password", string.Empty);


            switch (mode)
            {


                case (int)Mode.Create:
                    if (passwordText.Text == passswordRepeatText.Text)
                    {
                        var editSettings = settings.Edit();
                        editSettings.PutString("Password", passwordText.Text.Trim());
                        editSettings.Apply();
                        this.StartActivity(new Intent(this, typeof(Activity2)));
                        this.Finish();
                    }
                    else
                    {
                        passswordRepeatText.Text = string.Empty;
                        return;
                    }
                    break;

                case (int)Mode.Confirm:

                     if (passwordPref == passwordText.Text)
                    {
                        this.StartActivity(new Intent(this, typeof(Activity2)));
                        this.Finish();
                    }
                    else
                    {
                        passswordRepeatText.Text = string.Empty;
                        return;
                    }
                    break;

                case (int)Mode.ChangeMasterPassword:

                     if (passwordPref == passwordText.Text && passswordRepeatText.Text != string.Empty)
                    {
                        var editSettings = settings.Edit();
                        editSettings.PutString("Password", passswordRepeatText.Text.Trim());
                        editSettings.Apply();
                        this.StartActivity(new Intent(this, typeof(Activity2)));
                        this.Finish();
                    }
                    else
                    {
                        passswordRepeatText.Text = string.Empty;
                        return;
                    }
                    break;
            }



        }
    }
}

