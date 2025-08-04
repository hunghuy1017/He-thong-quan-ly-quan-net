namespace CourseManager
{
    internal class CourseList
    {
        List<Course> courses;

        public CourseList()
        {
            courses = new List<Course>();
        }

        public void ShowListCourse()
        {
            foreach (Course c in courses)
            {
                Console.WriteLine(c);
            }
        }
        public void initData()
        {
            courses.Add(new Course(3, "Python", new DateTime(2023, 3, 10)));
            courses.Add(new Course(4, "Web Development", new DateTime(2023, 4, 5)));
            courses.Add(new Course(1, "C# Basics", new DateTime(2023, 1, 15)));
            courses.Add(new Course(2, "Java Advanced", new DateTime(2023, 2, 20)));

        }
        //public int titleComparison(Course c1, Course c2)
        //{
        //    return c1.Title.CompareTo(c2.Title);
        //}
        public void SortCourseTitle()
        {
            // Using a delegate to sort by title
            //courses.Sort(
            //    //Anonymous method
            //    delegate (Course c1, Course c2)
            //    {
            //        return c1.Title.CompareTo(c2.Title);
            //    }
            //    );

            // Using a lambda expression to sort by title
            courses.Sort((c1, c2) => c1.Title.CompareTo(c2.Title));
        }
        public void SortCourseById()
        {
            courses.Sort((c1, c2) => c1.Id.CompareTo(c2.Id));
        }
        public void SortCourseByStartDate()
        {
            courses.Sort((c1, c2) => c1.StartDate.CompareTo(c2.StartDate));
        }
    }
}
