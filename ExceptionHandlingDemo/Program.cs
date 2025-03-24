using System;

class Program
{
    static void ValidateNumber(int num)
    {
        if (num < 0)
        {
            throw new ArgumentException("Number cannot be negative."); //manually throw exception
        }
        Console.WriteLine($"Valid number: {num}");
    }

    static void Main()
    {
        try
        {
            Console.Write("Enter a number: ");
            int num = Convert.ToInt32(Console.ReadLine());

            ValidateNumber(num);  // This may throw an exception
        }
        catch (FormatException )
        {
            Console.WriteLine("Error: Please enter a valid integer.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("General Exception: " + ex.Message);
        }
        finally
        {
            Console.WriteLine("Execution completed.");
        }
    }
}

