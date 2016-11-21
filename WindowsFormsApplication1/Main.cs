using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Main : Form
    {
        public Main()
        {
            ServicePointManager.DefaultConnectionLimit = 512;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
        }


        static HttpStatusCode DoRequest(string url)
        {
            WebRequest req = null;
            HttpWebResponse res = null;
            HttpStatusCode hsc = HttpStatusCode.BadRequest;
            try
            {
                req = WebRequest.Create(url);
                res = (HttpWebResponse)req.GetResponse();
                hsc = res.StatusCode;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                res.Close();
                req.Abort();
                res = null;
                req = null;
            }
            return hsc;
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
