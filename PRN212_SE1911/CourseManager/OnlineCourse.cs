using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager
{
    internal class OnlineCourse : Course
    {
        public OnlineCourse()
        {
        }
        public OnlineCourse(int id, string title, DateTime startDate, string linkMeet) : base(id, title, startDate)
        {
            LinkMeet = linkMeet;
        }
        public string LinkMeet { get; set; }
        public override void Input()
        {
            base.Input();
            //Console.WriteLine("Enter url: ");
            //LinkMeet = Console.ReadLine();
            LinkMeet = Validation.getString(5, 100, "Enter Online Course link meet: ");
        }
        public override void readFromLine(string line)
        {
            int index = line.LastIndexOf("|");
            base.readFromLine(line.Substring(0,index));
           
            LinkMeet = line.Substring(index+1);
        }
        public override string ToString()
        {
            return base.ToString() + $" - {LinkMeet}";
        }
    }
    
}
