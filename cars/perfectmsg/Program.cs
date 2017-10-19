using carshare;
using perfectmsg.msg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace perfectmsg
{
    class Program
    {
        public static string msgfile="/home/carbusiness/ftp/get/cars/carover.txt";

        static void Main(string[] args)
        {
            //import msg data
            if (!File.Exists(msgfile))
            {
                Console.WriteLine("file {0} doesn't exist,{1}", msgfile, DateTime.Now);
            }
            else
            {
                var msgs = File.ReadAllLines(msgfile);

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
                        var ordinal = 0;
                        if (!int.TryParse(fields[0], out ordinal))
                        {
                            Console.WriteLine("bad ordinal line: {0}", msg);
                            continue;
                        }
                        short busitype = 0;
                        short.TryParse(fields[1], out busitype);

                        var success = true;
                        if (fields[2] == "2") success = false;

                        var phone = fields[3];
                        var content = fields[4];
                        var dbmsg = db.Carmsg.FirstOrDefault(c =>c.Ordinal==ordinal);
                        if (dbmsg == null)
                        {
                            count++;
                            db.Carmsg.Add(new Carmsg
                            {
                                Ordinal=ordinal,
                                Content=content,Busiflag= success,
                                Busitype=busitype,
                                Timestamp=DateTime.Now,
                                Phone=phone,
                            });
                            db.SaveChanges();
                        }
                    }
                }
                Console.WriteLine("{0} of {2} message added this time,{1}",count, DateTime.Now,msgs.Count());
            }

            //send message
            using(var msgdb=new messageContext())
            {
                var sendcount = 0;
                var needsend = msgdb.Carmsg.Where(c => c.Sendflag == false
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
                Console.WriteLine("{0} of {2} messages sended this time,{1}", sendcount, DateTime.Now,needsend.Count());
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
