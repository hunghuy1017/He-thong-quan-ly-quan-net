using CourseManager;

internal class Program
{
    private static void Main(string[] args)
    {
        CourseList courseList = new CourseList();
        //courseList.AddListCourse();
        //courseList.initData();
        courseList.readCourseFromFile("TextFile1.txt");
        Console.WriteLine("List of courses: ");
        courseList.ShowListCourse();
        
        courseList.searchCourse();
        
        courseList.SortCourse();
        Console.WriteLine("List of courses you have Sorted: ");
        courseList.ShowListCourse();

    }
}