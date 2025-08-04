using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoExtension_LinQ
{
    internal class DemoLinQ
    {
        List<Course> courses;
        public DemoLinQ()
        {
            courses = new List<Course>
        {
            new Course (1,"PRN212", new DateTime(2025,01,01)),
            new Course (2,"PRJ301", new DateTime(2025,02,01)),
            new Course (3,"PRO192", new DateTime(2022,03,01)),
            new Course (4,"DBI202", new DateTime(2025,04,01)),
        };
        }
        public List<Course> getAllCourses()
        {
            return courses;
        }
        public Course getCourseByIdUsingMethod(int id)
        {
            return courses.First(x => x.Id == id);
        }
        public List<Course> getCoursesByTitleUsingMethos(string pattern)
        {
            return courses.Where(x => x.Title.Contains(pattern)).ToList();
        }
        //public List<Course> getCoursesByStartDateUsingMethod(DateTime start, DateTime end)
        //{
        //    return courses.Where(x => x.StartDate >= start && x.StartDate <= end).ToList();
        //}
        public void getCoursesByStartDateUsingMethod(DateTime start, DateTime end)
        {
            var result = courses.Where(x => x.StartDate >= start && x.StartDate <= end)
                                .Select(x=>(x.Title,x.StartDate));
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Title} - {item.StartDate.ToString("dd/MM/yyyy")}");
            }
        }
        public Course getCourseByIdUsingQuery(int id)
        {
            return (from c in courses
                   where c.Id==id
                   select c).First();
        }
        public List<Course> getCoursesByTitleUsingQuery(string pattern)
        {
            return (from c in courses
                   where c.Title.Contains(pattern)
                   select c).ToList();
        }       
        public void getCoursesByStartDateUsingQuery(DateTime start, DateTime end)
        {
            var result = from c in courses
                         where c.StartDate>=start&&c.StartDate<=end
                         select //c;
                               new {newTitle = c.Title, Date = c.StartDate };
            foreach (var item in result)
            {
                Console.WriteLine($"{item.newTitle} - {item.Date.ToString("dd/MM/yyyy")}");
            }
            
        }
    }
}
