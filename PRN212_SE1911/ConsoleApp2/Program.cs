using System.Collections;
using ConsoleApp2;

internal class Program
{
    private static void Main(string[] args)
    {
        //DemoArrayList();
        DemoList();
        //Student s1 = new Student(1, "Hoang", "Ha Noi");
        //Student s2 = new Student(1, "Hoang", "Ha Noi");
        //s1 = s2;
        //if (s1.Equals(s2))
        //{
        //    Console.WriteLine("Bang nhau");
        //}
        //else
        //{
        //    Console.WriteLine("Khong bang nhau");
        //}
        //Student s = new SEStudent(1, "Hoang", "Ha Noi", "PRN212");


        //Console.WriteLine(s);

        //s.display();


        //Student s = new Student(1,"Trung Dung","Ha Noi");
        //s.display();
        //SEStudent s2 = new SEStudent(2,"Tuan Anh","Bac Ninh","C#");
        //s2.display();
        //Student s3 = new Student();
        //s3.input();
        //s3.display();
        //SEStudent s4 = new SEStudent();
        //s4.input();
        //s4.display();
        //s.Id = 12;
        //s.Name = "John Doe";
        //s.Id = 100;// Using property to set id
        //Console.WriteLine("Id = "+ s.Id);// Using property to get id
        //s.address = "123 Main St";//address is private
    }
    public static void DemoArrayList()
    {
        ArrayList list = new ArrayList();
        list.Add(1);
        list.Add(2);
        list.Add('A');
        list.Add(3.14);
        list.Add(new Student(1, "Hoang", "Ha Noi"));
        
        int sum = (int)list[0]+(int)list[1];
        int su2 = (int)((char)list[2]);
        //int s3 = (int)su2;
        //int sum1 = (int)list[0] + (int)list[2];//Loi runtime
        Console.WriteLine(su2);
    }
    public static void DemoList()
    {
        List<SEStudent> list = new List<SEStudent>();
        list.Add(new SEStudent(1, "Hoang", "Ha Noi","PRN212"));
        list.Add(new SEStudent(2, "Tuan", "Bac Ninh", "PRN212"));
        list.Add(new SEStudent(3, "Nam", "Hai Duong", "PRN212"));
        //list.Add(1);//Loi compile
        Student s = list[0];
        SEStudent s2 = new SEStudent(3, "Nam", "Hai Duong", "PRN");
        if(list.Contains(s2))
        {
            Console.WriteLine("Co ton tai");
        }
        else
        {
            Console.WriteLine("Khong ton tai");
        }
        //Console.WriteLine(s);
    }
}