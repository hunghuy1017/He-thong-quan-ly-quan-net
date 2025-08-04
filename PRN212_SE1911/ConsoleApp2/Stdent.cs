using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class Student:Object
    {
        //fields
        
        //properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public Student(int id, string name, string address)
        {
            Id = id;
            Name = name;
            Address = address;
        }

        public Student()
        {
        }

        public bool login(string id, string password)
        {
            if (id == "admin" && password == "1234")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void input()
        {
            Console.WriteLine("Enter Id: ");
            Id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Name: ");
            Name = Console.ReadLine();
            Console.WriteLine("Enter Address: ");
            Address = Console.ReadLine();
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Add: {Address}";
        }

        public virtual void display()
        {
            //Console.WriteLine("Id: " + Id+", Name: "+Name+", Adress: "+Address);
            //Console.WriteLine("Id: {0}, Name: {1}, Address: {2}", Id, Name, Address);
            Console.WriteLine($"Id: {Id}, Name: {Name}, Address: {Address}");

        }

        public override bool Equals(object obj)
        {
            return obj is Student student &&
                   Id == student.Id &&
                   Name == student.Name &&
                   Address == student.Address;
        }
    }
    class SEStudent : Student
    {
        public string Major { get; set; }

        public SEStudent(int id, string name, string address, string major):base(id, name, address)
        {
            //Id = id;
            //Name = name;
            //Address = address;
            Major = major;
        }

        public SEStudent()
        {
        }

        public void input()
        {
            base.input();
            Console.WriteLine("Enter Major: ");
            Major = Console.ReadLine();
        }
        public override void display()
        {
            base.display();
            Console.WriteLine($"Major: {Major}");
        }
        public override string ToString()
        {
            return base.ToString() + $", Major: {Major}";
        }

    }
}
