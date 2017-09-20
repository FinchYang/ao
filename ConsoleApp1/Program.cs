using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var identify = "379009450819723";
           // var identify = "37900919550819723X";
            var idl = identify.Length;
            if (idl == 18)//379009 19750819 723X
            {
                var year =int.Parse( identify.Substring(6, 4));
                var month = int.Parse(identify.Substring(10, 2));
                var day = int.Parse(identify.Substring(12, 2));
                var birth = new DateTime(year, month, day);
                if (birth.AddYears(60) > DateTime.Now) Console.WriteLine("forbidden" + birth);
                else Console.WriteLine("old enough");
            }
            else if (idl == 15)
            {
                var year = int.Parse(identify.Substring(6, 2))+1900;
                var month = int.Parse(identify.Substring(8, 2));
                var day = int.Parse(identify.Substring(10, 2));
                var birth = new DateTime(year, month, day);
                if (birth.AddYears(60) > DateTime.Now) Console.WriteLine(" 15forbidden" + birth);
                else Console.WriteLine("15 old enough");
            }
        var tt=    System.IO.File.ReadAllText(@"e:\37900919750819723X");
            Console.WriteLine(DateTime.Now+ "-15 old enough"+tt);
        }
    }
}
