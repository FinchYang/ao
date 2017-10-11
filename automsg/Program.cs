using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;
using automsg.aaa;

namespace exportdb
{
    class Program
    {
        static string dbtofilePath = "dbtofile";

        static void Main(string[] args)
        {
            var quit = false;
            do
            {
                var now = DateTime.Now;
                try
                {
                    using (var abdb = new studyinContext())
                    {
                        var req = abdb.Request.OrderBy(a => a.Ordinal).LastOrDefault();
                        if (req == null)
                        {
                            sendautomsg("db-error");
                        }
                        else
                        {
                            if (now.Hour >= 6)
                                if (req.Time < now.AddMinutes(-10))
                                {
                                    sendautomsg(string.Format("last-request-time-{0}", getdate(req.Time)));
                                }

                        }
                        // if (now.Minute < 10)//report per hour
                        // {
                        //     var cc = abdb.Request.Where(b => b.Time > now.AddHours(-1)).Count();
                        //     sendautomsg(string.Format("last-hour-had-{0}-requests", cc));
                        // }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(now + "something happened ... " + ex.Message);
                }
                Thread.Sleep(1000 * 60 * 10);
            } while (!quit);
        }
        private static string getdate(DateTime dt)
        {
            return string.Format("last-request-time-{0}-{1}-{2}-{3}-{4}-{5}",
                             dt.Year, dt.Month, dt.Day,
                            dt.Hour, dt.Minute, dt.Second);
        }
        private static void sendautomsgone(string v, string phone)
        {
            var cmd =
            string.Format(" {0} \"{1}\"", phone, v );
            Console.WriteLine(cmd);
            var a = new System.Diagnostics.Process();
            a.StartInfo.FileName = "/home/driverbusiness/bin/sendmsg";
            a.StartInfo.UseShellExecute = true;
            a.StartInfo.Arguments = cmd;
            a.Start();
            a.WaitForExit();
        }
        private static void sendautomsg(string v)
        {
            sendautomsgone(v, "13255533751");
            sendautomsgone(v, "13256622325");
        }
    }
}
