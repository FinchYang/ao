

using abmsg.msg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace perfectmsg
{
    class Program
    {
        public static string msgfile= "/home/inspect/ftp/get/studyover.txt";
        public static string msgfilenew = "/home/inspect/ftp/get/contentPhone.txt";
        static void impfile(string file, string tag)
        {
            var msgs = File.ReadAllLines(file);
            var count = 0;
            using (var db = new messageContext())
            {
                foreach (var msg in msgs)
                {
                    var fields = msg.Split('|');
                    if (fields.Length < 5)
                    {
                        Console.WriteLine("bad line: {0}", msg);
                        continue;
                    }
                    var ordinal = fields[0];
                   
                    short busitype = 0;
                    short.TryParse(fields[1], out busitype);

                    var success = true;
                    if (fields[2] == "2") success = false;

                    var phone = fields[3];
                    var content = fields[4];
                    var dbmsg = db.Abmsg.FirstOrDefault(c => c.Ordinal == ordinal);
                    if (dbmsg == null)
                    {
                        count++;
                        db.Abmsg.Add(new Abmsg
                        {
                            Ordinal = ordinal,
                            Content = content,
                            Busiflag = success,
                            Busitype = busitype,
                            Timestamp = DateTime.Now,
                            Phone = phone,
                        });
                        db.SaveChanges();
                    }
                }
            }
            Console.WriteLine("{2},{0} message added this time,{1}", count, DateTime.Now,tag);
        }
        static void Main(string[] args)
        {
            //import msg data
            if (!File.Exists(msgfile))
            {
                Console.WriteLine("file {0} doesn't exist,{1}", msgfile, DateTime.Now);
            }
            else
            {
                impfile(msgfile, "over");
            }
            if (!File.Exists(msgfilenew))
            {
                Console.WriteLine("file {0} doesn't exist,{1}", msgfile, DateTime.Now);
            }
            else
            {
                impfile(msgfilenew, "new");
            }
            //send message
            using (var msgdb=new messageContext())
            {
                var sendcount = 0;
                var needsend = msgdb.Abmsg.Where(c => c.Sendflag == false
                  && c.Count < 100
                  && c.Timestamp.CompareTo(DateTime.Now.AddMonths(-1))>=0
                  );
                foreach(var m in needsend)
                {
                    try {
                        send(m.Phone, m.Content);
                        m.Sendflag = true;
                        sendcount++;
                    }
                   catch(Exception ex)
                    {
                        Console.WriteLine("{2},send msg {0} error, {1}", m.Content, ex, m.Phone);
                        m.Count++;
                    }
                    msgdb.SaveChanges();

                }
                Console.WriteLine("{0} messages sended this time,{1}", sendcount, DateTime.Now);
            }
        }
      
        public static void send(string phone, string content)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("account", "mfxx");
            dict.Add("password", "59c0f4720b89f36c90fd785d2cde1e04");
            dict.Add("smsType", "");
            dict.Add("message", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><com.ctc.ema.server.jwsserver.sms.MtMessage><content>" + content + "</content><phoneNumber>" + phone + "</phoneNumber><sendTime>" + System.DateTime.Now.AddMinutes(1).ToLocalTime().ToString() + "</sendTime><smsId>4acadda1-5806-4492-9a82-b7ab3f1c8ec0</smsId><subCode></subCode></com.ctc.ema.server.jwsserver.sms.MtMessage>");

            using (var client = new HttpClient())
            using (var formData = new FormUrlEncodedContent(dict))
            {
                var ret = client.PostAsync("http://192.168.10.16:9090/ema/httpSms/sendSms", formData);
                var result = ret.Result.Content.ReadAsStringAsync().Result;
                Console.WriteLine(result);
            }
        }
    }
}
