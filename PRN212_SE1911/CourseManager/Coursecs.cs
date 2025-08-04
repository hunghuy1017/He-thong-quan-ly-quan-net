using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager
{
    internal class Course:IComparable<Course>
    {
        
        public Course()
        {
        }

        public Course(int id, string title, DateTime startDate)
        {
            Id = id;
            Title = title;
            StartDate = startDate;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }

        public int CompareTo(Course other)
        {
            return Title.CompareTo(other.Title);
        }
       

        public virtual void Input()
        {
            //Console.WriteLine("Enter id: ");
            //Id = int.Parse(Console.ReadLine());
            Id = Validation.getInt(1, 100, "Enter Course id: ");

            //Console.WriteLine("Enter title: ");
            //Title = Console.ReadLine();
            Title = Validation.getString(5, 10, "Enter Course title:");

            //Console.WriteLine("Enter start date: ");
            //StartDate = DateTime.ParseExact(Console.ReadLine(),"dd/MM/yyyy",null);
            StartDate = Validation.getDate(new DateTime(2025, 1, 1), new DateTime(2025,12,31), "dd/MM/yyyy", "Enter Course start date (dd/MM/yyyy): ");
        }
        public virtual void readFromLine(string line)
        {
            string[] items= line.Split("|");
          
            Id = int.Parse(items[1]);
            Title = items[2];
            StartDate = DateTime.ParseExact(items[3], "dd/MM/yyyy", null);
        }
        public override string ToString()
        {
            return $"{Id} - {Title} - {StartDate}";
        }
    }
}
