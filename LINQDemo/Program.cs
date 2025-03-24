// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using LINQDemo;
using System.Linq;

class Program
{
    public static void Main(string[] args)
    {
        List<Employee> employeeList = DataManager.GetData();

        // Check if any employee is older than 30
        bool anyAbove30 = employeeList.Any(emp => emp.Age > 30);
        Console.WriteLine("Any employee above 30: " + anyAbove30);

        // Check if all employees are older than 30
        bool allAbove30 = employeeList.All(emp => emp.Age > 30);
        Console.WriteLine("All employees above 30: " + allAbove30);

        // Sum of all salaries
        decimal totalSalary = employeeList.Sum(emp => emp.Salary);
        Console.WriteLine("Total Salary Sum: " + totalSalary);

        // Count of employees older than 30
        int countAbove30 = employeeList.Count(emp => emp.Age > 30);
        Console.WriteLine("Count of employees above 30: " + countAbove30);

        // Average salary of employees
        double averageSalary = employeeList.Average(emp => emp.Salary);
        Console.WriteLine("Average Salary: " + averageSalary);

        // Minimum salary among employees
        decimal minSalary = employeeList.Min(emp => emp.Salary);
        Console.WriteLine("Minimum Salary: " + minSalary);

        // Maximum salary among employees
        decimal maxSalary = employeeList.Max(emp => emp.Salary);
        Console.WriteLine("Maximum Salary: " + maxSalary);

        // Employee with the minimum salary
        Employee lowestPaidEmployee = employeeList.MinBy(emp => emp.Salary);
        Console.WriteLine("Lowest Paid Employee -> ID:{0}, Name: {1} {2}, Department: {3}, Age: {4}, Salary: {5}",
            lowestPaidEmployee.ID, lowestPaidEmployee.FirstName, lowestPaidEmployee.lastName,
            lowestPaidEmployee.Department, lowestPaidEmployee.Age, lowestPaidEmployee.Salary);

        // Fetch first 2 employees
        var firstTwoEmployees = employeeList.Take(2).ToList();
        Console.WriteLine("First 2 Employees:");
        firstTwoEmployees.ForEach(emp => Console.WriteLine(emp.FirstName + " " + emp.lastName));

        // Skip first two employees and take next 5
        var skippedTwoNextFive = employeeList.Skip(2).Take(5).ToList();
        Console.WriteLine("Skipping first 2, next 5 Employees:");
        skippedTwoNextFive.ForEach(emp => Console.WriteLine(emp.FirstName + " " + emp.lastName));

        // Distinct employees by first name
        var uniqueEmployeesByName = employeeList.DistinctBy(emp => emp.FirstName).ToList();
        Console.WriteLine("Employees with unique first names:");
        uniqueEmployeesByName.ForEach(emp => Console.WriteLine(emp.FirstName));

        // Select distinct first names of employees
        IEnumerable<string> distinctNames = employeeList.Select(emp => emp.FirstName).Distinct();
        Console.WriteLine("Distinct first names:");
        foreach (string name in distinctNames)
        {
            Console.WriteLine(name);
        }

        // Ordering employees by first name, then last name
        var orderedEmployees = employeeList.OrderBy(emp => emp.FirstName).ThenBy(emp => emp.lastName).ToList();
        Console.WriteLine("Employees ordered by first and last name:");
        orderedEmployees.ForEach(emp => Console.WriteLine(emp.FirstName + " " + emp.lastName));

        // Ordering employees in descending order
        var descendingOrderEmployees = employeeList.OrderByDescending(emp => emp.FirstName).ThenByDescending(emp => emp.lastName).ToList();
        Console.WriteLine("Employees in descending order of first and last name:");
        descendingOrderEmployees.ForEach(emp => Console.WriteLine(emp.FirstName + " " + emp.lastName));

        // Filtering employees with age > 28 and salary > 50000 (where always return a collection)
        var filteredEmployees = employeeList.Where(emp => emp.Age > 28 && emp.Salary > 50000).ToList();
        Console.WriteLine("Employees with Age > 28 and Salary > 50000:");
        filteredEmployees.ForEach(emp => Console.WriteLine(emp.FirstName + " " + emp.lastName));

        // Fetch the first matching employee with a given name
        Employee? firstEmployee = employeeList.FirstOrDefault(emp => emp.FirstName == "Harshita");
        Console.WriteLine("First Employee named Harshita: " + (firstEmployee != null ? firstEmployee.FirstName : "Not Found"));

        // Fetch the last matching employee with a given name
        Employee? lastEmployee = employeeList.LastOrDefault(emp => emp.FirstName == "Harshita");
        Console.WriteLine("Last Employee named Harshita: " + (lastEmployee != null ? lastEmployee.FirstName : "Not Found"));

        // Fetch an employee with a unique first name (Single)
        try
        {
            Employee? singleName = employeeList.SingleOrDefault(emp => emp.FirstName == "Harshita");
            if (singleName != null)
                Console.WriteLine("Single Tushar -> {0} {1}", singleName.FirstName, singleName.lastName);
            else
                Console.WriteLine("No employee named Tushar found");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("More than one employee found");
        }

    }

}
