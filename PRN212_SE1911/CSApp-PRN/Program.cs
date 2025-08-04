using System;

namespace CSApp_PRN
{
    class Program
    {
        static void Main()
        {
            var dao = new ProductDAO();
            while (true)
            {
                Console.WriteLine("\n== PRODUCT MANAGEMENT ==");
                Console.WriteLine("1. View All Products");
                Console.WriteLine("2. Add Product");
                Console.WriteLine("3. Update Product");
                Console.WriteLine("4. Delete Product");
                Console.WriteLine("5. Search Product");
                Console.WriteLine("0. Exit");
                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var products = dao.GetAll();
                        foreach (var p in products)
                        {
                            Console.WriteLine($"{p.ProductID} | {p.ProductName} | {p.Price:C} | {(p.IsAvailable ? "Available" : "Unavailable")}");
                        }
                        break;

                    case "2":
                        Console.Write("Name: ");
                        var name = Console.ReadLine();
                        Console.Write("Price: ");
                        var price = decimal.Parse(Console.ReadLine());
                        Console.Write("Is Available (true/false): ");
                        var available = bool.Parse(Console.ReadLine());
                        dao.Add(new Product { ProductName = name, Price = price, IsAvailable = available });
                        Console.WriteLine("Product added.");
                        break;

                    case "3":
                        Console.Write("Product ID to update: ");
                        int idToUpdate = int.Parse(Console.ReadLine());
                        Console.Write("New Name: ");
                        var newName = Console.ReadLine();
                        Console.Write("New Price: ");
                        var newPrice = decimal.Parse(Console.ReadLine());
                        Console.Write("Is Available (true/false): ");
                        var newAvailable = bool.Parse(Console.ReadLine());
                        dao.Update(new Product { ProductID = idToUpdate, ProductName = newName, Price = newPrice, IsAvailable = newAvailable });
                        Console.WriteLine("Product updated.");
                        break;

                    case "4":
                        Console.Write("Product ID to delete: ");
                        int idToDelete = int.Parse(Console.ReadLine());
                        dao.Delete(idToDelete);
                        Console.WriteLine("Product deleted.");
                        break;

                    case "5":
                        Console.Write("Keyword: ");
                        var keyword = Console.ReadLine();
                        var results = dao.Search(keyword);
                        foreach (var p in results)
                        {
                            Console.WriteLine($"{p.ProductID} | {p.ProductName} | {p.Price:C} | {(p.IsAvailable ? "Available" : "Unavailable")}");
                        }
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}
