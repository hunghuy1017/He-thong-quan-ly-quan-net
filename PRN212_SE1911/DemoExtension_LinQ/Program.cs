using DemoExtension_LinQ;

internal class Program
{
    private static void Main(string[] args)
    {
        //demoExtensionMethod();
        //demoCourseExtension();
        //demoLinQ();
        demoUsingEvent();
    }
    public static void demoExtensionMethod()
    {
        List<Course> courses = new List<Course>
        {
            new Course (1,"PRN212", new DateTime(2025,01,01)),
            new Course (2,"PRJ301", new DateTime(2025,02,01)),
            new Course (3,"PRO192", new DateTime(2022,03,01)),
            new Course (4,"DBI202", new DateTime(2025,04,01)),
        };
        courses.Display();
    }
    public static void demoCourseExtension()
    {
        Course c = new Course(1, "PRN212", new DateTime(2025, 01, 01));
        c.dispaly(3);
    }
    public static void demoLinQ()
    {
        DemoLinQ demo = new DemoLinQ();
        Console.WriteLine("All Courses:");
        demo.getAllCourses().Display();
       
        demo.getCourseByIdUsingMethod(2).dispaly(1);
        Console.WriteLine("Courses with title containing 'PRN':");
        demo.getCoursesByTitleUsingMethos("PRN").Display();
        Console.WriteLine("Courses starting between 01/01/2025 and 03/01/2025:");
        demo.getCoursesByStartDateUsingMethod(new DateTime(2025, 01, 01), new DateTime(2025, 03, 01));
        demo.getCourseByIdUsingQuery(2).dispaly();
        Console.WriteLine("Courses with title containing 'PRN':");
        demo.getCoursesByTitleUsingQuery("PRN").Display();
        Console.WriteLine("Courses starting between 01/01/2025 and 03/01/2025:");
        demo.getCoursesByStartDateUsingQuery(new DateTime(2025, 01, 01), new DateTime(2025, 03, 01));

    }
    public static void demoUsingEvent()
    {
        DemoEvent demo = new DemoEvent();
        demo.CourseAdded += onAdd;
        Course course = new Course(1, "PRN212", new DateTime(2025, 01, 01));
        demo.AddCourse(course);
    }
    public static void onAdd(object sender, Course e)
    {
        Console.WriteLine($"Course is added successfully!");
    }
}