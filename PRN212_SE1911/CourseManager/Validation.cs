namespace CourseManager
{
    internal class Validation
    {
        public static int getInt(int min, int max, string mess)
        {
            int value;
            while (true)
            {
                try
                {
                    Console.WriteLine(mess);
                    value = int.Parse(Console.ReadLine());
                    if (value < min || value > max) throw new OverflowException("Number is Overflow!");
                    return value;
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Number is wrong format. Please enter a valid integer.");
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine("" + ex.Message + " Please enter a number between " + min + " and " + max + ".");
                }
            }
        }
        public static string getString(int minLenth, int maxLength, string mess)
        {
            string value;
            while (true) {
                try
                {
                    Console.WriteLine(mess);
                    value = Console.ReadLine().Trim();
                    if (value.Length < minLenth || value.Length > maxLength) throw new OverflowException("String is Overflow!");
                    return value;
                }
                catch (FormatException e)
                {
                    Console.WriteLine("String is wrong format. Please enter a valid string.");
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine("" + ex.Message + " Please enter a string between " + minLenth + " and " + maxLength + " characters.");
                }
            }
        }
        public static DateTime getDate(DateTime start, DateTime end,string parten, string mess)
        {
            DateTime value;
            while (true)
            {
                try
                {
                    Console.WriteLine(mess);
                    value = DateTime.ParseExact(Console.ReadLine(), parten, null);
                    if (value < start || value > end) throw new OverflowException("Date is Overflow!");
                    return value;
                }
                catch (FormatException e)
                {
                    Console.WriteLine($"Date is wrong format. Please enter a valid date {parten} format.");
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine("" + ex.Message + " Please enter a date between " + start.ToString(parten) + " and " + end.ToString(parten) + ".");
                }
            }
        }

    }
}
