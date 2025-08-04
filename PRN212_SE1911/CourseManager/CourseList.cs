using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager
{
    internal class CourseList
    {
        List<Course> courses;

        public CourseList()
        {
            courses = new List<Course>();
        }
        public void AddListCourse()
        {
            while (true)
            {
                Console.WriteLine("Enter course type: Course (C)? OnlineCourse (O)? Exit (E):");
                string type = Console.ReadLine();
                Course c = null;
                if (type.ToUpper().Equals("C"))
                {
                    c = new Course();
                    c.Input();
                    courses.Add(c);
                }
                else if (type.ToUpper().Equals("O"))
                {
                    c = new OnlineCourse();
                    c.Input();
                    courses.Add(c);
                }
                else if (type.ToUpper().Equals("E"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid type");
                    continue;
                }
            }
        }
        public void ShowListCourse()
        {
            foreach (Course c in courses)
            {
                Console.WriteLine(c);
            }
        }
        public void searchCourseByDate(DateTime start, DateTime end)
        {
            foreach (Course c in courses)
            {
                if (c.StartDate >= start && c.StartDate <= end)
                {
                    Console.WriteLine(c);
                }
            }
        }
        public void initData()
        {
            courses.Add(new Course(3, "Python for Data Science", new DateTime(2023, 3, 10)));
            courses.Add(new OnlineCourse(4, "Web Development", new DateTime(2023, 4, 5), "https://zoom.us/j/123456789"));
            courses.Add(new Course(1, "C# Basics", new DateTime(2023, 1, 15)));
            courses.Add(new OnlineCourse(2, "Java Advanced", new DateTime(2023, 2, 20), "https://meet.google.com/abc-defg-hij"));

        }
        public void searchCourse()
        {
            //Console.WriteLine("Enter start date: ");
            //DateTime start = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);
            DateTime start = Validation.getDate(new DateTime(2025, 1, 1), new DateTime(2025, 12, 31), "dd/MM/yyyy", "Enter start date (dd/MM/yyyy): ");
            //Console.WriteLine("Enter end date: ");
            //DateTime end = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);
            DateTime end = Validation.getDate(new DateTime(2025, 1, 1), new DateTime(2025, 12, 31), "dd/MM/yyyy", "Enter end date (dd/MM/yyyy): ");
            Console.WriteLine("List of courses you have searched: ");
            searchCourseByDate(start, end);
        }
        public void SortCourse()
        {
            courses.Sort();
        }
        public void SortCourseById()
        {
            courses.Sort(new IdComparers());
        }
        public void SortCourseByStartDate()
        {
            courses.Sort(new SatStartDateComparers());
        }
        public void readCourseFromFile(string fileName)
        {
            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Course c = null;
                        if (line[0] == 'C')
                        {
                            c = new Course();
                        }
                        else
                        {
                            c = new OnlineCourse();
                        }
                        c.readFromLine(line);
                        courses.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
            }
        }
    }
}
