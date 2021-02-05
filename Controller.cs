using System;

namespace ATM
{
    class AtmController {
        public static void start() {
            Data.LoadData();

            if (Data.users.Count == 0)
                Data.users.Add(new User("Admin11", "admin", "1234", 0));

            while (true) {
                Console.Clear();
            Console.WriteLine("Welcome to SWISS Bank ATM");
            Console.WriteLine("Please login to continue");

                Data.currentUser = AtmController.login();

                if (Data.currentUser == null)
                {
                    Console.WriteLine("Login failed. Username or password is incorrect!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    continue;
                }
                
                Console.WriteLine($"Welcome {Data.currentUser.name}!");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

                if (Data.currentUser.type == 0) // admin
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("1. Create New Account");
            Console.WriteLine("2. Delete Existing Account");
            Console.WriteLine("3. Update Account Information");
            Console.WriteLine("4. Search for Account");
            Console.WriteLine("5. View Reports");
            Console.WriteLine("6. Exit");

                        //ask for input
                        int temp = Int32.Parse(Console.ReadLine());
                        if (temp == 1)
                            AdminController.CreateNewAccount();
                        else if (temp == 2)
                            AdminController.DeleteAccount();
                        else if (temp == 3)
                            AdminController.UpdateAccountInfo();
                        else if (temp == 4)
                            AdminController.SearchAccount();
                        else if (temp == 5)
                            AdminController.ViewReports();
                        else if (temp == 6)
                        {
                            AtmController.logout();
                            break;
                        }
                    }
                }
                else if (Data.currentUser.type == 1) // customer
                {
                    while (true)
                    {
                        Console.WriteLine("1. Withdraw Cash");
            Console.WriteLine("2. Cash Transfer");
            Console.WriteLine("3. Deposit Cash");
            Console.WriteLine("4. Display Balance");
            Console.WriteLine("5. Exit");

                        int temp = Int32.Parse(Console.ReadLine());
                        if (temp == 1)
                            CustomerController.WithdrawCash();
                        else if (temp == 2)
                            CustomerController.CashTransfer();
                        else if (temp == 3)
                            CustomerController.DepositCash();
                        else if (temp == 4)
                            CustomerController.DisplayBalance();
                        else if (temp == 5) {
                            AtmController.logout();
                            break;
                        }
                    }
                }

                if (Data.currentUser == null)
                    break;
            }
        }

        static User login() {
            String username, password;

            Console.WriteLine("Enter your username");
            username = Console.ReadLine();

            Console.WriteLine("Enter your password");
            password = Console.ReadLine();

            User temp = Data.users.Find(delegate(User u)
            {
                return u.username == username;
            });

            return temp;
        }

        static void logout()
        {
            Data.SaveData();
            Data.currentUser = null;
        }
    }

     class AdminController {
        public static void CreateNewAccount()
        {
            String login, pincode, name, type, temp;
            Boolean status;
            Double balance;

            Console.WriteLine("Enter New Customer's Login:");
            login = Console.ReadLine();

            Console.WriteLine("Enter New Customer's Pin Code:");
            pincode = Console.ReadLine();
            
            Console.WriteLine("Enter Holder's Name:");
            name = Console.ReadLine();

            Console.WriteLine("Enter Type (Savings, Current):");
            type = Console.ReadLine();

            Console.WriteLine("Enter Starting Balance:");
            balance = Double.Parse(Console.ReadLine());

            Console.WriteLine("Enter Status (Active/Inactive):");
            temp = Console.ReadLine();
            if (temp.ToLower() == "active")
                status = true;
            else
                status = false;

            User user= new User(name, login, pincode, 1);
            Account account = new Account(login, type, balance, status);

            Data.users.Add(user);
            Data.accounts.Add(account);

            Console.WriteLine("New Account Created!");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        public static void DeleteAccount()
        {
            Console.WriteLine("Enter the account number which you want to delete: ");
            int acc_number = Int32.Parse(Console.ReadLine());

            Account acc = Data.accounts.Find(delegate(Account acc)
            {
                return acc.acc_number == acc_number;
            });

            if (acc == null)
            {
                Console.WriteLine("No such account found");
                return;
            }

            User u = Data.users.Find(delegate(User u)
            {
                return u.username == acc.username;
            });

            Console.WriteLine($"You wish to delete the account held by {u.name}; If this information is correct please re-enter the account number: ");
            int confirm_acc_number = Int32.Parse(Console.ReadLine());

            if (acc_number == confirm_acc_number)
            {
                Data.accounts.Remove(acc);
            }

            Console.WriteLine("Account deleted!");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        public static void UpdateAccountInfo()
        {
            String login, pincode, name, type, temp;
            Boolean status;
            Double balance;

            Console.WriteLine("Enter the Account Number to update: ");
            int acc_number = Int32.Parse(Console.ReadLine());

            Account acc = Data.accounts.Find(delegate(Account acc)
            {
                return acc.acc_number == acc_number;
            });

            if (acc == null)
            {
                Console.WriteLine("Account not found!");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                return;
            }

            User user = Data.users.Find(delegate(User u)
            {
                return u.username == acc.username;
            });

            Console.WriteLine($"Enter {user.name}'s Login:");
            login = Console.ReadLine();

            Console.WriteLine($"Enter {user.name}'s Pin Code:");
            pincode = Console.ReadLine();
            
            Console.WriteLine("Enter new holder's Name:");
            name = Console.ReadLine();

            Console.WriteLine("Enter new type (Savings, Current):");
            type = Console.ReadLine();

            Console.WriteLine($"Enter Balance (Current balance is {acc.balance}):");
            balance = Double.Parse(Console.ReadLine());

            Console.WriteLine("Enter Status (Active/Inactive):");
            temp = Console.ReadLine();
            if (temp.ToLower() == "active")
                status = true;
            else
                status = false;
            
            user.username = login;
            acc.username  = login;
            user.pincode  = pincode;
            user.name     = name;
            acc.type      = type;
            acc.status    = status;
            acc.balance   = balance;

            Console.WriteLine("Account updated!");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        // search and display info if account is found
        public static void SearchAccount()
        {
            Console.WriteLine("Enter account number to search: ");
            int acc_number = Int32.Parse(Console.ReadLine());

            Account acc = Data.accounts.Find(delegate(Account acc)
            {
                return acc.acc_number == acc_number;
            });

            if (acc == null)
            {
                Console.WriteLine("Account not found");
            }
            else
            {
                Console.WriteLine($"Username: {acc.username}");
            Console.WriteLine($"Type: {acc.type}");
            Console.WriteLine($"Balance: {acc.balance}");
            Console.WriteLine($"Status: {(acc.status ? "Active" : "Inactive")}");
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        public static void ViewReports()
        {
            Console.WriteLine("1. Accounts by Amount");
            Console.WriteLine("2. Accounts by Date");
        }
    }

    class CustomerController
    {
        public static void WithdrawCash()
        {
            Console.WriteLine("a. Fast cash");
            Console.WriteLine("a. Normal cash");
            Console.WriteLine("Please select a mode of withdrawal");
            char mode = Console.ReadKey().KeyChar;

            double amount = 0;

            if (mode == 'a')
            {
                Console.WriteLine("1. 500");
            Console.WriteLine("2. 1000");
            Console.WriteLine("3. 2000");
            Console.WriteLine("4. 5000");
            Console.WriteLine("5. 10,000");
            Console.WriteLine("6. 15,000");
            Console.WriteLine("7. 20,000");

            Console.WriteLine("Select one of the above");

                int temp = Int32.Parse(Console.ReadLine());

                if (temp < 1 || temp > 7)
                {
                    Console.WriteLine("Enter between 1 and 7");
                    return;
                }

                switch(temp)
                {
                    case 1:
                        amount = 500;
                        break;
                    case 2:
                        amount = 1000;
                        break;
                    case 3:
                        amount = 2000;
                        break;
                    case 4:
                        amount = 5000;
                        break;
                    case 5:
                        amount = 10000;
                        break;
                    case 6:
                        amount = 15000;
                        break;
                    case 7:
                        amount = 20000;
                        break;
                }
            }
            else if (mode == 'b')
            {
                Console.WriteLine("Enter amount: ");
                amount = Double.Parse(Console.ReadLine());
            }

            Account acc = Data.accounts.Find(delegate(Account acc)
            {
                return acc.username == Data.currentUser.username;
            });

            // check balance
            if (acc.balance - amount < 0)
            {
                Console.WriteLine("You dont have enough balance");
                return;
            }

            acc.balance = acc.balance - amount;
            Transaction trx = new Transaction(0, acc.acc_number, amount, DateTime.Now);
            Data.transactions.Add(trx);
        }

        public static void CashTransfer()
        {

        }

        public static void DepositCash()
        {

        }

        public static void DisplayBalance()
        {
            Account acc = Data.accounts.Find(delegate(Account acc)
            {
                return acc.username == Data.currentUser.username;
            });

            Console.WriteLine($"Account # {acc.acc_number}");
            Console.WriteLine($"Balance: {acc.balance}");
            Console.WriteLine($"Data: {DateTime.Now.Date}");
        }
    }
}