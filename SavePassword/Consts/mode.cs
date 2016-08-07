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

namespace SavePassword.Consts
{
    public enum Mode
    {
        Create = 1,
        Confirm = 2,
        ChangeMasterPassword = 3,
    }

    public enum SortType
    {
        Name = 1,
        Password = 2,
        CreateDate = 3
    }
}