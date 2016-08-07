using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SavePassword.Models;
using SavePassword.Services;

namespace SavePassword.Adapters
{
    class AccountsAdapter: BaseAdapter<Account>
    {
        private Context mContext;
        private int mRowLayout;
        private List<Account> mAccounts;
        private int [] mAlternatingColors;

        private CryptService _cryptService;

        public AccountsAdapter(Context context, int rowLayout, List<Account> accounts)
        {
            mContext = context;
            mRowLayout = rowLayout;
            mAccounts = accounts;
            mAlternatingColors = new int[] { 0xF2F2F2, 0x6797C5 };
            _cryptService = new CryptService();
        }

        public override int Count
        {
            get { return mAccounts.Count; }
        }

        public override Account this[int position]
        {
            get { return mAccounts[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(mRowLayout, parent, false);
            }

            row.SetBackgroundColor(GetColorFromInteger(mAlternatingColors[position % mAlternatingColors.Length]));
           
            
            TextView name = row.FindViewById<TextView>(Resource.Id.txtName);
            name.Text = mAccounts[position].Name;

            TextView password = row.FindViewById<TextView>(Resource.Id.txtPasswordL);
            password.Text = mAccounts[position].Password;

            TextView createDate = row.FindViewById<TextView>(Resource.Id.txtCreateDate);
            createDate.Text = mAccounts[position].CreateDate.ToShortDateString();

            if ((position % 2) == 1)
            {
                //Green background, set text white
                name.SetTextColor(Color.White);
                password.SetTextColor(Color.White);
                createDate.SetTextColor(Color.White);
                
            }

            else
            {
                //White background, set text black
                name.SetTextColor(Color.Black);
                password.SetTextColor(Color.Black);
                createDate.SetTextColor(Color.Black);
            }

            return row;
        }

        private Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }
    }
}