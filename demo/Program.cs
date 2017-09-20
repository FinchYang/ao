using System;

namespace demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var ss= DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            var dd=DateTime.Parse(ss);
            Console.WriteLine(dd+"Hello World!"+ss);
        }
    }
}
