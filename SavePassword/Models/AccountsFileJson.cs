using System;
using System.Collections.Generic;


namespace SavePassword.Models
{
    public class AccountsFileJson
    {
        public DateTime DocDate { get; set; }

        public List<Account> Accounts { get; set; } = new List<Account>();

        public string resultMes { get; set; }
    }
}