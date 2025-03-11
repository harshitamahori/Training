// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;

public class cardHolder
{
    //Bank user - fisrtname,lastname,card no,pin,balance
    //class variables
    String cardNum;
    int pin;
    String firstName;
    String lastName;
    double balance;

    public cardHolder(string cardNum,int pin, string firstName, string lastName,double balance)
    {
        this.cardNum = cardNum;
        this.pin = pin;
        this.firstName = firstName;
        this.lastName = lastName;
        this.balance = balance;

    }

    //create getters
    public string getNum()
    {
        return cardNum;
    }
    public int getPin()
    {
        return pin;
    }
    public string getFirstName()
    {
        return firstName;
    }
    public string getLastName()
    {
        return lastName;
    }
    public double getBalance()
    {
        return balance;
    }

    //create setters
    public void setNum(String newCardNum)
    {
        cardNum = newCardNum;
    }
    public void setPin(int newPin)
    {
        pin = newPin;
    }
    public void setFirstName(string newFirstName)
    {
        firstName = newFirstName;
    }
    public void setLastName(string newLastName)
    {
        lastName = newLastName;
    }
    public void setBalance(double newBalance)
    {
        balance = newBalance;
    }

    public static void Main(string[] args)
    {
        void printOptions()
        {
            Console.WriteLine("Please choose from one of the following options....");
            Console.WriteLine("1. Deposit");
            Console.WriteLine("2. Withdraw");
            Console.WriteLine("3. Show Balance");
            Console.WriteLine("4. Exit");
        }
        void deposit(cardHolder currentUser)
        {
            Console.WriteLine("How much money would you like to deposit? ");
            double deposit = Double.Parse(Console.ReadLine()); //returns a string so parse to double
            currentUser.setBalance(currentUser.getBalance() + deposit);
            Console.WriteLine("Thank you for your money !! . Your new balance is: "+currentUser.getBalance());
        }
        void withdraw(cardHolder currentUser)
        {
            Console.WriteLine("How much money would you like to withdraw? ");
            double withdrawal = Double.Parse(Console.ReadLine());
            //check if user have enough money
            if(currentUser.getBalance() < withdrawal)
            {
                Console.WriteLine("Insufficient balance :(");
            }
            else
            {
                currentUser.setBalance(currentUser.getBalance() - withdrawal);

                Console.WriteLine("You are good to go! Thank you");
            }
        }
        void balance(cardHolder currentUser)
        {
            Console.WriteLine("Current balance :" + currentUser.getBalance());
        }

        List<cardHolder> cardHolders = new List<cardHolder>();
        cardHolders.Add(new cardHolder("1234567890", 1234, "Harshita", "Mahori", 50000));
        cardHolders.Add(new cardHolder("1234509876", 1122, "Nandini", "Bisht", 100000));
        cardHolders.Add(new cardHolder("9876543210", 2233, "Tushar", "Singh", 35000));
        cardHolders.Add(new cardHolder("9871234560", 4455, "Shipra", "Vashist", 45000));

        //PROMPT USER
        Console.WriteLine("Welcome to Harshi'sATM");
        Console.WriteLine("Please enter your debit card: ");
        String debitCradNum = "";
        cardHolder currentUser;

        while (true)
        {
            try
            {
                debitCradNum = Console.ReadLine();
                currentUser = cardHolders.FirstOrDefault(a => a.cardNum == debitCradNum);   // searc and return whole record
                if(currentUser != null)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Card not recognized . Please try again");
                }

            }
            catch
            {
                Console.WriteLine("Card not recognized . Please try again");

            }
        }
        Console.WriteLine("Please enter your pin: ");
        int userPin = 0;
        while (true)
        {
            try
            {
                userPin = int.Parse(Console.ReadLine());
                if (currentUser.getPin() == userPin)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Incorrect Pin. Please try again");
                }

            }
            catch
            {
                Console.WriteLine("Incorrect Pin. Please try again");

            }
        }
        Console.WriteLine("Welcome " + currentUser.getFirstName()+" !!!");
        int option = 0;
        do
        {
            printOptions();
            try
            {
                option = int.Parse(Console.ReadLine());

            }
            catch { }

            if (option == 1){
                deposit(currentUser);
            }
            else if(option == 2)
            {
                withdraw(currentUser);
            }
            else if(option == 3)
            {
                balance(currentUser);
            }
            else if(option == 4)
            {
                break;
            }
            else
            {
                option = 0;
            }

        } while (option != 4);
        Console.WriteLine("Thank you! Have a nice day !!");

    }
}
