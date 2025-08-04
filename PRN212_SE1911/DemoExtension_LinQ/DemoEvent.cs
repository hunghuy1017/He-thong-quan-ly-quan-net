using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoExtension_LinQ
{
    public delegate void addEvent(object sender, Course c);
    internal class DemoEvent
    {
        List<Course> courses = new List<Course>();
        public event addEvent CourseAdded;
        public void AddCourse(Course c)
        {
            courses.Add(c);
            //kich hoat su kien
            onCourseAdded(c);
        }
        protected virtual void onCourseAdded(Course c)
        {
            CourseAdded?.Invoke(this, c);
        }
    }
}
