using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class StudentManager
    {
        // Contains a list of students
        private List<Student> students;

        public StudentManager()
        {
            students = new List<Student>();
        }

        // Public Method: AddStudent(Student student)
        public void AddStudent(Student student)
        {
            students.Add(student);
        }

        // Public Method: DisplayStudents()
        public void DisplayStudents()
        {
            if (!students.Any())
            {
                Console.WriteLine("No students to display.");
                return;
            }

            Console.WriteLine("\n--- All Students ---");
            foreach (var student in students)
            {
                student.Display();
            }
            Console.WriteLine("--------------------");
        }

        // Public Method: PromoteStudents(IsPromotable isPromotable)
        public void PromoteStudents(IsPromotable isPromotable)
        {
            Console.WriteLine("\n--- Promotable Students ---");
            bool foundPromotable = false;
            foreach (var student in students)
            {
                if (isPromotable(student))
                {
                    student.Display();
                    foundPromotable = true;
                }
            }

            if (!foundPromotable)
            {
                Console.WriteLine("No students meet the promotion criteria.");
            }
            Console.WriteLine("---------------------------");
        }

        // 4. Create the InputStudent() method (static)
        public static Student InputStudent()
        {
            Student newStudent = new Student();

            Console.WriteLine("\nEnter Student Information:");

            while (true)
            {
                Console.Write("Enter Student ID: ");
                string idInput = Console.ReadLine();
                try
                {
                    newStudent.Id = int.Parse(idInput);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: Invalid ID format. Please enter an integer.");
                }
            }

            Console.Write("Enter Student Name: ");
            newStudent.Name = Console.ReadLine();

            while (true)
            {
                Console.Write("Enter Student Age: ");
                string ageInput = Console.ReadLine();
                try
                {
                    newStudent.Age = int.Parse(ageInput);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: Invalid Age format. Please enter an integer.");
                }
            }

            return newStudent;
        }
    }
}
