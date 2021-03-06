﻿using System;
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
            ServicePointManager.DefaultConnectionLimit = 50;

            const string url = "http://baidu.com";

            List<string> output_OK = new List<string>();
            List<string> output_ERROR = new List<string>();

            List<string> input = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                input.Add(url);
            }


            Task task = new Task(() =>
            {
                DoRequest(url);
            });

            task.Start();

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

        static List<string> GetHref(string url)
        {
            var urls = new List<string>();
            if (!string.IsNullOrWhiteSpace(url))
            {
                // const string Regex_Href = "href=\".*? \"";
                const string Regex_Src = "src=\".*? \"";


            }

            return urls;
        }
    }
}
