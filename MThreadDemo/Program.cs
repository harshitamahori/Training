// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;
using System.Threading;

class Program
{
    static void thrd1()
    {
        for(int x = 0; x <= 10; x++)
        {
            Console.WriteLine("Thread1 is: {0}", x);
            if (x == 5)
            {
                Thread.Sleep(6000);
            }
        }

    }
    static void thrd2()
    {
        for (int y = 0; y <= 10; y++)
        {
            Console.WriteLine("Thread2 is: {0}", y);
        }

    }
    //,Multi threading execution
    public static void Main()
    {
        Thread t1 = new Thread(thrd1);
        Thread t2 = new Thread(thrd2);
        t1.Start();
        t2.Start();

    }
    //Single thread execution
    //public static void Main()
    //{
    //    thrd1();
    //    thrd2();

    //}
}
