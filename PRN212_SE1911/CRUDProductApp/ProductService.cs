using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ProductService
{
    public static void ShowAll()
    {
        var list = ProductManager.GetAllProducts();
        Console.WriteLine("danh sach san pham:");
        foreach (var p in list)
        {
            Console.WriteLine($"id: {p.ProductID} | ten: {p.ProductName} | gia: {p.Price} | {(p.IsAvailable ? "con hang" : "het hang")}");
        }
        Console.WriteLine();
    }

    public static void Add()
    {
        var prod = new Product
        {
            ProductName = ValidationHelper.ReadNonEmptyString("ten san pham: "),
            Price = ValidationHelper.ReadPositiveDecimal("gia: "),
            IsAvailable = ValidationHelper.ReadBoolean("co san hang khong")
        };
        ProductManager.AddProduct(prod);
        Console.WriteLine("them thanh cong!");
    }

    public static void Edit()
    {
        int id = ValidationHelper.ReadPositiveInt("nhap id can sua: ");
        var existing = ProductManager.GetById(id);
        if (existing == null)
        {
            Console.WriteLine("khong tim thay san pham.");
            return;
        }

        existing.ProductName = ValidationHelper.ReadNonEmptyString("ten moi: ");
        existing.Price = ValidationHelper.ReadPositiveDecimal("gia moi: ");
        existing.IsAvailable = ValidationHelper.ReadBoolean("co san hang khong");
        ProductManager.UpdateProduct(existing);
        Console.WriteLine("cap nhat thanh cong!");
    }

    public static void Delete()
    {
        int deleteId = ValidationHelper.ReadPositiveInt("nhap id can xoa: ");
        var product = ProductManager.GetById(deleteId);
        if (product == null)
        {
            Console.WriteLine("khong tim thay san pham.");
            return;
        }

        ProductManager.DeleteProduct(deleteId);
        Console.WriteLine("da xoa!");
    }
}

