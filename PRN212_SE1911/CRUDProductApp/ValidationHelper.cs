using System;

public static class ValidationHelper
{
    public static string ReadNonEmptyString(string prompt)
    {
        string input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(input))
                Console.WriteLine("khong duoc de trong");
        } while (string.IsNullOrWhiteSpace(input));
        return input;
    }

    public static decimal ReadPositiveDecimal(string prompt)
    {
        decimal result;
        do
        {
            Console.Write(prompt);
            if (!decimal.TryParse(Console.ReadLine(), out result) || result < 0)
                Console.WriteLine("gia phai la so duong");
        } while (result < 0);
        return result;
    }

    public static bool ReadBoolean(string prompt)
    {
        string input;
        do
        {
            Console.Write(prompt + " (y/n): ");
            input = Console.ReadLine()?.Trim().ToLower();
        } while (input != "y" && input != "n");

        return input == "y";
    }

    public static int ReadPositiveInt(string prompt)
    {
        int result;
        do
        {
            Console.Write(prompt);
            if (!int.TryParse(Console.ReadLine(), out result) || result <= 0)
                Console.WriteLine("phai nhap so nguyen duong");
        } while (result <= 0);
        return result;
    }

    public static void Pause()
    {
        Console.WriteLine("\nnhan phim bat ky de tiep tuc...");
        Console.ReadKey();
        Console.Clear();
    }
}
