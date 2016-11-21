using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Main : Form
    {
        private const string url = "http://baidu.com";


        List<string> output_OK = new List<string>();
        List<string> output_ERROR = new List<string>();

        List<string> input = new List<string>();

        public Main()
        {
            ServicePointManager.DefaultConnectionLimit = 512;
            InitializeComponent();

            // Task: http://www.cnblogs.com/pengstone/archive/2012/12/23/2830238.html

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Test()
        {
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

        private void btn_Go_Click(object sender, EventArgs e)
        {
            TaskScheduler syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Task<int> task = new Task<int>(() =>
            {
                return (int)DoRequest(url);
            });

            var cts = new CancellationTokenSource();

            task.ContinueWith(
                t =>
                    {
                        this.rtb_output.AppendText(string.Format("Result is: {0}.{1}", task.Result.ToString(), Environment.NewLine));
                    },
                cts.Token,
                TaskContinuationOptions.AttachedToParent,
                syncContextTaskScheduler);

            task.Start();
        }

        private void openFile_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFD = new OpenFileDialog();
            oFD.Title = "Open file";
            oFD.ShowHelp = true;
            oFD.Filter = "Plain Text|*.txt|All Files|*.*";
            oFD.FilterIndex = 1;
            oFD.RestoreDirectory = false;
            oFD.Multiselect = false;
            if (oFD.ShowDialog() == DialogResult.OK)
            {
                this.rtb_output.AppendText(string.Format("Reading file: {0}...{1}", oFD.FileName, Environment.NewLine));

                Task task = new Task(() =>
                {
                    ReadFile(oFD.FileName);
                });

                task.Start();

                TaskScheduler syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

                Task cwt = task.ContinueWith(t =>
                {
                    this.rtb_output.AppendText(string.Format("Read {0} url(s) form file.{1}", this.input.Count, Environment.NewLine));
                }, syncContextTaskScheduler);
            }
        }

        private void ReadFile(string path)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                input = lines
                    .Where(a =>
                        !string.IsNullOrWhiteSpace(a)
                        &&
                        (a.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                          || a.StartsWith("https", StringComparison.OrdinalIgnoreCase)
                          || a.StartsWith("//", StringComparison.OrdinalIgnoreCase)
                        ))
                    // .Distinct()
                    .ToList();
            }
            catch (Exception ex)
            {
                this.rtb_output.AppendText(string.Format("Can not read file {0}: {1}!", path, ex.Message));
            }
        }
    }
}
