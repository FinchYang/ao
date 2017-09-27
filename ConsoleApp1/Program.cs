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
           
             //   int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            int[] numbers = { 5, 4, 5, 3, 2, 5, 3, 7, 2, 0 };
            var numberGroups =
                    from num in numbers
                  //  group num by num % 5 into numGroup
                    group num by num into numGroup
                    select new { Remainder = numGroup.Key, Numbers = numGroup.Count() };

                foreach (var grp in numberGroups)
                {
                    Console.WriteLine("Numbers with a remainder of {0} when divided by 5:{1}", 
                        grp.Remainder,grp.Numbers);
                    //foreach (var n in grp.Numbers)
                    //{
                    //    Console.WriteLine(n);
                    //}
                }
            

        }
    }
}
