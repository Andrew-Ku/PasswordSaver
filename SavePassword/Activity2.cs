using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Newtonsoft.Json;
using SavePassword.Adapters;
using SavePassword.Animations;
using SavePassword.Consts;
using SavePassword.Extension;
using SavePassword.Infrastructure.Impl;
using SavePassword.Models;
using SavePassword.Services;
using ClipboardManager = Android.Text.ClipboardManager;

namespace SavePassword
{

    [Activity(Label = "Activity2")]
    public class Activity2 : Activity
    {
        private List<Account> mAccounts;
        private ListView mListView;
        private EditText mSearch;
        private LinearLayout mContainer;
        private bool mAnimatedDown;
        private bool mIsAnimating;
        AccountsAdapter mAdapter;

        private TextView nameHeader;
        private TextView passwordHeader;
        private TextView createDateHeader;

        private bool nameAscending = true;
        private bool passwordAscending;
        private bool createDateAscending;

        private int sortType = (int)SortType.Name;

        private AccountRepository _repository;
        private CryptService _cryptService;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _repository = new AccountRepository("accounts");
            _cryptService = new CryptService();
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Activity2);
            mListView = FindViewById<ListView>(Resource.Id.listView);
            mSearch = FindViewById<EditText>(Resource.Id.etSearch);
            mContainer = FindViewById<LinearLayout>(Resource.Id.llContainer);

            nameHeader = FindViewById<TextView>(Resource.Id.txtNameHeader);
            passwordHeader = FindViewById<TextView>(Resource.Id.txtPasswordHeader);
            createDateHeader = FindViewById<TextView>(Resource.Id.txtCreateDateHeader);

            nameHeader.Click += NameHeaderOnClick;
            passwordHeader.Click += PasswordHeaderOnClick;
            createDateHeader.Click += CreateDateHeaderOnClick;

            mSearch.Alpha = 0;
            mContainer.BringToFront();
            mSearch.TextChanged += mSearch_TextChanged;

            mAccounts = new List<Account>();
            //mAccounts.Add(new Account { Name = "Mail", Password = "12345", CreateDate = DateTime.Now.AddDays(12) });
            //mAccounts.Add(new Account { Name = "Yandex", Password = "fafjd", CreateDate = DateTime.Now });
            //mAccounts.Add(new Account { Name = "GitHub", Password = "75scx", CreateDate = DateTime.Now.AddDays(4) });
            //mAccounts.Add(new Account { Name = "Vk.com", Password = "kcv33", CreateDate = DateTime.Now.AddDays(-9) });
            //mAccounts.Add(new Account { Name = "Compane", Password = "03vjye", CreateDate = DateTime.Now.AddDays(-6) });
            //mAccounts.Add(new Account { Name = "NGTU", Password = "03vjye", CreateDate = DateTime.Now.AddDays(7) });
            //mAccounts.Add(new Account { Name = "Пропуск", Password = "03vjye", CreateDate = DateTime.Now });
            //mAccounts.Add(new Account { Name = "Сигнализация", Password = "sdasd123", CreateDate = DateTime.Now.AddDays(1) });
            //mAccounts.Add(new Account { Name = "Апаче", Password = "Тест1", CreateDate = DateTime.Now.AddDays(3) });
            //mAccounts.Add(new Account { Name = "Тест", Password = "Тест2", CreateDate = DateTime.Now.AddDays(-12) });

            mAccounts = _repository.GetItems().ToList();
            foreach (var mAccount in mAccounts)
            {
                mAccount.Password = _cryptService.DecryptText(mAccount.Password, "test");
            }


            mAdapter = new AccountsAdapter(this, Resource.Layout.row_account, mAccounts.OrderBy(a => a.Name).ToList());
            mListView.Adapter = mAdapter;

            RegisterForContextMenu(mListView);
        }

        private void CreateDateHeaderOnClick(object sender, EventArgs eventArgs)
        {
            List<Account> filteredAccount;
            sortType = (int)SortType.CreateDate;
            if (createDateAscending)
            {
                filteredAccount = mAccounts.Where(a => a.Name.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
             || a.Password.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
             || a.CreateDate.ToLongDateString().Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)).OrderByDescending(a => a.CreateDate).ToList();
            }
            else
            {
                filteredAccount = mAccounts.Where(a => a.Name.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
             || a.Password.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
             || a.CreateDate.ToLongDateString().Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)).OrderBy(a => a.CreateDate).ToList();
            }

            mAdapter = new AccountsAdapter(this, Resource.Layout.row_account, filteredAccount);
            mListView.Adapter = mAdapter;
            createDateAscending = !createDateAscending;
        }

        private void PasswordHeaderOnClick(object sender, EventArgs eventArgs)
        {
            List<Account> filteredAccount;
            sortType = (int)SortType.Password;
            if (passwordAscending)
            {
                filteredAccount = mAccounts.Where(a => a.Name.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
             || a.Password.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
             || a.CreateDate.ToLongDateString().Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)).OrderByDescending(a => a.Password).ToList();
            }
            else
            {
                filteredAccount = mAccounts.Where(a => a.Name.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
             || a.Password.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
             || a.CreateDate.ToLongDateString().Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)).OrderBy(a => a.Password).ToList();
            }

            mAdapter = new AccountsAdapter(this, Resource.Layout.row_account, filteredAccount);
            mListView.Adapter = mAdapter;
            passwordAscending = !passwordAscending;
        }

        private void NameHeaderOnClick(object sender, EventArgs eventArgs)
        {
            List<Account> filteredAccount;
            sortType = (int)SortType.Name;
            if (nameAscending)
            {
                filteredAccount = mAccounts.Where(a => a.Name.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
             || a.Password.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
             || a.CreateDate.ToLongDateString().Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)).OrderByDescending(a => a.Name).ToList();
            }
            else
            {
                filteredAccount = mAccounts.Where(a => a.Name.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
             || a.Password.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
             || a.CreateDate.ToLongDateString().Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)).OrderBy(a => a.Name).ToList();
            }

            mAdapter = new AccountsAdapter(this, Resource.Layout.row_account, filteredAccount);
            mListView.Adapter = mAdapter;

            nameAscending = !nameAscending;
        }

        private void mSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchedAccounts = mAccounts.Where(a => a.Name.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
             || a.Password.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
             || a.CreateDate.ToLongDateString().Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList<Account>();


            mAdapter = new AccountsAdapter(this, Resource.Layout.row_account, searchedAccounts);
            mListView.Adapter = mAdapter;

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {

                case Resource.Id.search:
                    mSearch.Visibility = ViewStates.Visible; ;
                    if (mIsAnimating)
                    {
                        return true;
                    }
                    if (!mAnimatedDown)
                    {
                        //Listview is up
                        MyAnimation anim = new MyAnimation(mListView, mListView.Height - mSearch.Height);
                        anim.Duration = 200;
                        mListView.StartAnimation(anim);
                        anim.AnimationStart += anim_AnimationStartDown;
                        anim.AnimationEnd += anim_AnimationEndDown;
                        mContainer.Animate().TranslationYBy(mSearch.Height).SetDuration(200).Start();
                    }

                    else
                    {
                        //Listview is down
                        MyAnimation anim = new MyAnimation(mListView, mListView.Height + mSearch.Height);
                        anim.Duration = 200;
                        mListView.StartAnimation(anim);
                        anim.AnimationStart += anim_AnimationStartUp;
                        anim.AnimationEnd += anim_AnimationEndUp;
                        mContainer.Animate().TranslationYBy(-mSearch.Height).SetDuration(200).Start();
                    }

                    mAnimatedDown = !mAnimatedDown;
                    return true;

                case Resource.Id.newAccount:
                    this.StartActivity(new Intent(this, typeof(CreateAccount)));
                    Finish();
                    return true;

                case Resource.Id.newPasswordAction:
                    var intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("mode", (int)Mode.ChangeMasterPassword);
                    this.StartActivity(intent);
                    Finish();
                    return true;


                default:
                    return base.OnOptionsItemSelected(item);
            }
        }


        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.listView)
            {
                var info = (AdapterView.AdapterContextMenuInfo)menuInfo;

                var item = SelectedMyAccount(info.Position);

                menu.Add(Menu.None, 1, 1, item.Name);
                menu.Add(Menu.None, 2, 1, item.Password);

                base.OnCreateContextMenu(menu, v, menuInfo);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            var itemAccount = SelectedMyAccount(info.Position);

            var clipboard = (ClipboardManager)GetSystemService(ClipboardService);
            var entity = string.Empty;
            if (item.ItemId == 1)
            {
                clipboard.Text = itemAccount.Name;
                entity = "Логин";
            }
            else if (item.ItemId == 2)
            {
                clipboard.Text = itemAccount.Password;
                entity = "Пароль";
            }

            Toast.MakeText(this, $"{entity} скопирован в буфер", ToastLength.Short).Show();
            return true;
        }



        #region Анимация
        void anim_AnimationEndUp(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
        {
            mIsAnimating = false;
            mSearch.ClearFocus();
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(mSearch.WindowToken, HideSoftInputFlags.None);


        }

        void anim_AnimationEndDown(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
        {
            mSearch.RequestFocus();
            InputMethodManager inputMethodManager = this.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.ShowSoftInput(mSearch, ShowFlags.Forced);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
            mIsAnimating = false;
        }

        void anim_AnimationStartDown(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
        {
            mIsAnimating = true;
            mSearch.Animate().AlphaBy(1.0f).SetDuration(200).Start();
        }

        void anim_AnimationStartUp(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
        {
            mIsAnimating = true;
            mSearch.Animate().AlphaBy(-1.0f).SetDuration(200).Start();
        }

        #endregion

        protected override void OnPause()
        {
            if (CurrentFocus != null)
            {
                InputMethodManager inputMethodManager = (InputMethodManager)this.GetSystemService(InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
            }

            base.OnPause();
        }

        private Account SelectedMyAccount(int position)
        {
            var items = new List<Account>();

            switch (sortType)
            {
                case (int)SortType.Password:
                    items = passwordAscending
                        ? mAccounts.OrderBy(a => a.Password).ToList()
                        : mAccounts.OrderByDescending(a => a.Password).ToList();

                    break;

                case (int)SortType.Name:
                    items = nameAscending
                       ? mAccounts.OrderBy(a => a.Name).ToList()
                       : mAccounts.OrderByDescending(a => a.Name).ToList();

                    break;
                case (int)SortType.CreateDate:
                    items = createDateAscending
                       ? mAccounts.OrderBy(a => a.CreateDate).ToList()
                       : mAccounts.OrderByDescending(a => a.CreateDate).ToList();

                    break;
                default:
                    break;
            }

            return items[position];
        }
    }
}

