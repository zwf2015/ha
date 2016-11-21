using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            const string url = "http://baidu.com";

            DoRequest(url);

            Console.ReadKey();
        }

        static void DoRequest(string url)
        {
            WebRequest req = null;
            HttpWebResponse res = null;
            try
            {
                req= WebRequest.Create(url);
                res = (HttpWebResponse)req.GetResponse();
                Console.WriteLine(res.StatusDescription);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                res.Close();
                req.Abort();
                res = null;
                req = null;
            }
        }
    }
}
