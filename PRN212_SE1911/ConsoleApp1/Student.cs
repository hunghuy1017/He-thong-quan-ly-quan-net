using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    // 1. Create a Student class
    public class Student
    {
        // Public Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        // Public Method: Display()
        public void Display()
        {
            Console.WriteLine($"Id: {Id}, Name: {Name}, Age: {Age}");
        }
    }

}
