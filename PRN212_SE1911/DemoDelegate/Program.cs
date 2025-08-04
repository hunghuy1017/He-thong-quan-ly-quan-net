using CourseManager;
using DemoDelegate;

internal class Program
{
    private static void Main(string[] args)
    {
        //demoBasicDelegate();
        //demoMulticastDelegate();
        demoUsingComparisonDelegate();
    }
    public static int Add(int a, int b)
    {
        Console.WriteLine("Add method called");
        return a+b;
    }
    public static int Sub(int a, int b)
    {
        Console.WriteLine("Subtract method called");
        return a-b;
    }
    public static int compute(Caculation caculation, int x, int y)
    {
        return caculation(x, y);//Noi quyet dinh goi ham
    }
    public static void demoBasicDelegate()
    {
        //Caculation caculation = new Caculation(Add);
        //caculation = new Caculation(Sub);
        Caculation caculation = Add;
        //Console.WriteLine($"Output: {caculation(9, 4)}");
        caculation = Sub;//Noi quyet ham nao duoc goi
        //Console.WriteLine($"Output: {caculation(9,4)}");
        // Using the compute method
        Console.WriteLine($"Output compute: {compute(caculation, 9, 4)}");
    }
    public static void demoMulticastDelegate()
    {
        Caculation caculation = Add;
        caculation += Sub; // Add Sub to the delegate chain
        caculation += Add;
        caculation -= Add;
        caculation -= Add;
        caculation -= Add;
        caculation -= Sub;//Runtime error because caculation is null
        Console.WriteLine($"Output compute: {compute(caculation, 9, 4)}");
    }
    public static void demoUsingComparisonDelegate() {
        CourseList courseList = new CourseList();
        courseList.initData();
        courseList.SortCourseTitle();
        Console.WriteLine("List of couse after sort by Title:");
        courseList.ShowListCourse();
    }
}