using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQDemo
{
    internal class Employee{
        public int ID { get; set; }
        public string? FirstName { get; set;}
        public string? lastName { get; set; }
        public string? Department { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }

    }
    internal static class DataManager
    {
        internal static List<Employee> Data
        {
            get
            {
                return new List<Employee>
                {
                    new Employee {ID =1,FirstName="John",lastName="Smith",Age=30,Department="HR",Salary=25000},
                    new Employee {ID =2,FirstName="Harshita",lastName="Singh",Age=22,Department="DEV",Salary=27500},
                    new Employee {ID =3,FirstName="Arvind",lastName="Aswal",Age=33,Department="QA",Salary=33000},
                    new Employee {ID =4,FirstName="Harsh",lastName="Mahori",Age=22,Department="DEV",Salary=297000},
                    new Employee {ID =5,FirstName="Aastha",lastName="Bisht",Age=25,Department="Management",Salary=44000},
                    new Employee {ID =6,FirstName="Tushar",lastName="Singh",Age=24,Department="DEV",Salary=55000},
                    new Employee {ID =7,FirstName="Piyush",lastName="Singh",Age=28,Department="Management",Salary=150000},
                    new Employee {ID =8,FirstName="Shipra",lastName="Vashist",Age=35,Department="QA",Salary=76000},
                    new Employee {ID =9,FirstName="Nandini",lastName="Bisht",Age=29,Department="HR",Salary=45500},
                    new Employee {ID =10,FirstName="Udit",lastName="Jain",Age=38,Department="DevOps",Salary=105000},
                    new Employee {ID =11,FirstName="Tvisha",lastName="Tulli",Age=43,Department="Management",Salary=67000},
                    new Employee {ID =12,FirstName="Harshita",lastName="Mahori",Age=33,Department="DevOps",Salary=89500}
                };

            }
        }

        internal static List<Employee> GetData()
        {
            return Data;
        }
    }
}
