using System;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            ProductService.ShowAll();

            Console.WriteLine("quan ly san pham:");
            Console.WriteLine("1. them san pham");
            Console.WriteLine("2. sua san pham");
            Console.WriteLine("3. xoa san pham");
            Console.WriteLine("4. thoat");
            Console.Write("chon: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ProductService.Add();
                    break;
                case "2":
                    ProductService.Edit();
                    break;
                case "3":
                    ProductService.Delete();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("lua chon khong hop le!");
                    break;
            }

            ValidationHelper.Pause();
        }
    }
}
