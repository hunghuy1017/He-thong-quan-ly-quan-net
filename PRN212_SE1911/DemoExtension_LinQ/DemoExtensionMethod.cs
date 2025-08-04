using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoExtension_LinQ
{
    static class DemoExtensionMethod
    {
        public static void Display(this List<Course> courses)
        {          
            foreach (Course c in courses)
            {
                Console.WriteLine(c);
            }
        }
    }
}
