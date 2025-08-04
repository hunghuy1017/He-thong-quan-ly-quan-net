using ConsoleApp2;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!"); // Fixed typo: Changed 'WriLine' to 'WriteLine'  
        Student s = new Student();
        s.Name = "John Doe";
        //s.login("admin", "1234");
    }
    static bool isPrime(int n)
    {
        if (n <= 1) return false;
        for (int i = 2; i <= Math.Sqrt(n); i++)
        {
            if (n % i == 0) return false;
        }
        return true;
    }
}
class Course
{
    string courseName;
    string courseCode;
    int credits;
    public Course(string courseName, string courseCode, int credits)
    {
        this.courseName = courseName;
        this.courseCode = courseCode;
        this.credits = credits;
    }
}
