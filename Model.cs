using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ATM {
    [Serializable]
    class User {
        public String name;
        public String username;
        public String pincode;
        public int type; // 0 = admin, 1 = customer

        public User(String name, String username, String pincode, int type)
        {
            this.name     = name;
            this.username = username;
            this.pincode  = pincode;
            this.type     = type;
        }
    }

    [Serializable]
    class Account {
        public int acc_number;
        public String username;  // foreign key
        public String type; // savings, current
        public Double balance;
        public Boolean status; // active/inactive

        public Account(String username, String type, Double balance, Boolean status)
        {
            this.acc_number = Data.accounts.Count + 1;
            this.username   = username;
            this.type       = type;
            this.balance    = balance;
            this.status     = status;
        }
    }

    [Serializable]
    class Transaction
    {
        public int type; // 0 = debit, 1 = credit, 2 = transfer
        public int primary_acc_number;
        public double amount;
        public DateTime date;

        public Transaction(int type, int primary_acc_number, double amount, DateTime date)
        {
            this.type = type;
            this.primary_acc_number = primary_acc_number;
            this.amount = amount;
            this.date = date;
        }
    }

    class Data {
        public static List<User> users               = new List<User>();
        public static List<Account> accounts         = new List<Account>();
        public static List<Transaction> transactions = new List<Transaction>();

        public static User currentUser = null;

        public static void SaveData()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();

                Stream stream = new FileStream(@"users.dat", FileMode.OpenOrCreate, FileAccess.Write);
                formatter.Serialize(stream, Data.users);
                stream.Close();

                stream = new FileStream(@"accounts.dat", FileMode.OpenOrCreate, FileAccess.Write);
                formatter.Serialize(stream, Data.accounts);
                stream.Close();

                stream = new FileStream(@"transactions.dat", FileMode.OpenOrCreate, FileAccess.Write);
                formatter.Serialize(stream, Data.transactions);
                stream.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void LoadData()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                
                Stream stream = new FileStream(@"users.dat", FileMode.Open, FileAccess.Read);
                Data.users = (List<User>)formatter.Deserialize(stream);
                stream.Close();

                stream = new FileStream(@"accounts.dat", FileMode.Open, FileAccess.Read);
                Data.accounts = (List<Account>)formatter.Deserialize(stream);
                stream.Close();

                stream = new FileStream(@"transactions.dat", FileMode.Open, FileAccess.Read);
                Data.transactions = (List<Transaction>)formatter.Deserialize(stream);
                stream.Close();
            }
            catch(Exception e )
            {
                Console.WriteLine(e.Message);
            }
        }
      }
}