using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Collections;


namespace Account
{
    class BankAccount
    {
        private long accNo;
        private decimal accBal;
        private AccountType accType;
        private string holder;
        private Queue tranQueue = new Queue();

        public long Number
        {
            get { return accNo; }
        }

        public decimal Balance
        {
            get { return accBal; }
        }

        public AccountType Type
        {
            get { return accType; }
        }

        public string Holder
        {
            get { return holder; }
            set { holder = value; }
        }

        public void Populate(long number, decimal balance)
        {
            accNo = number;
            accBal = balance;
            accType = AccountType.Checking;
        }

        public void Deposit(decimal amount)
        {
            accBal += amount;
            tranQueue.Enqueue(new BankTransaction(amount, DateTime.Now));
        }

        public void Withdraw(decimal amount)
        {
            accBal -= amount;
            tranQueue.Enqueue(new BankTransaction(-amount, DateTime.Now));
        }

        public IEnumerator GetEnumerator()
        {
            return tranQueue.GetEnumerator();
        }

        public BankTransaction this[int index]
        {
            get
            {
                if (index < 0 || index >= tranQueue.Count)
                {
                    return null;
                }
                IEnumerator ie = tranQueue.GetEnumerator();
                BankTransaction tran = null;
                for (int i = 0; i <= index; i++)
                {
                    ie.MoveNext();
                    tran = (BankTransaction)ie.Current;
                }
                return tran;
            }
        }

        public override String ToString()
        {
            return ("Account's holder name: " + holder + "\nAccount Number: " + accNo + "\nAccount Type: " + accType + "\nAccount Balance: " + accBal);
        }
    }

    public enum AccountType
    {
        Checking,
        Savings
    }

    class BankTransaction
    {
        private decimal amount;
        private DateTime when;

        public decimal Amount
        {
            get { return amount; }
        }

        public DateTime When
        {
            get { return when; }
        }

        public BankTransaction(decimal tranAmount, DateTime tranWhen)
        {
            amount = tranAmount;
            when = tranWhen;
        }
    }

    class CreateAccount
    {
        public static void Main(string[] args)
        {
            BankAccount acc1 = new BankAccount();
            acc1.Populate(123, 500);
            acc1.Holder = "Japhet";
            for (int i = 0; i < 5; i++)
            {
                acc1.Deposit(100);
                acc1.Withdraw(100);
            }
            Console.WriteLine(acc1);
            IEnumerator enumerator = acc1.GetEnumerator();
            int counter = 0;
            while (enumerator.MoveNext())
            {
                BankTransaction tran = (BankTransaction)enumerator.Current;
                Console.WriteLine("Date: {0} \tAmount: {1}", tran.When, tran.Amount);
                counter++;
            }
            Console.ReadLine();
        }
    }
}