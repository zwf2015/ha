using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Main : Form
    {
        ConcurrentDictionary<string, List<string>> dic = new ConcurrentDictionary<string, List<string>>((Environment.ProcessorCount * 2), 8);

        List<string> output_OK = new List<string>();
        List<string> output_ERROR = new List<string>();

        List<string> http = new List<string>();
        List<string> https = new List<string>();
        List<string> freeProtcol = new List<string>();

        List<string> input = new List<string>();

        bool AutoCheckHttps = true;

        public Main()
        {
            ServicePointManager.DefaultConnectionLimit = 512;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.btn_Go.Enabled = false;
            this.tbx_url.Enabled = false;
        }

        private void btn_Go_Click(object sender, EventArgs e)
        {
            this.btn_Go.Enabled = false;

            TaskScheduler syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            CancellationTokenSource cts = new CancellationTokenSource();

            Task<OutPutList> task = new Task<OutPutList>(state =>
            {
                OutPutList result = new OutPutList();
                result.Output_OK = new List<string>();
                result.OutPut_Error = new List<string>();
                SetText(state.ToString());
                new Task(() =>
                {
                    foreach (var url in http)
                    {
                        if (HttpStatusCode.OK == DoRequest(url))
                        {
                            result.Output_OK.Add(url);
                        }
                        else
                        {
                            result.OutPut_Error.Add(url);
                        }
                    }
                }).Start();
                new Task(() =>
                {
                    foreach (var url in https)
                    {
                        if (HttpStatusCode.OK == DoRequest(url))
                        {
                            result.Output_OK.Add(url);
                        }
                        else
                        {
                            result.OutPut_Error.Add(url);
                        }
                    }
                }).Start();
                return result;
            }, "Begin Do Request...");

            task.ContinueWith(t =>
            {
                SetText(string.Format("Parent continue: {0}.", t.Result.Http_Ok_Count));
                SetText(Environment.NewLine);
            });

            task.ContinueWith(t =>
                    {
                        var output = t.Result;
                        this.rtb_output.AppendText("All task done!");
                        this.rtb_output.AppendText(Environment.NewLine);
                        this.rtb_output.AppendText(output.GetReport);
                        this.btn_Go.Enabled = true;
                    }, cts.Token, TaskContinuationOptions.ExecuteSynchronously, syncContextTaskScheduler);

            task.Start();
        }

        private void test()
        {
            TaskScheduler syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            string url = this.tbx_url.Text.Trim();
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

        private void btn_Openfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFD = new OpenFileDialog();
            oFD.Title = Resource.OpenFileDialog_Title;
            oFD.ShowHelp = true;
            oFD.Filter = Resource.OpenFileDialog_Filter;
            oFD.FilterIndex = 1;
            oFD.RestoreDirectory = false;
            oFD.Multiselect = false;
            if (oFD.ShowDialog() == DialogResult.OK)
            {
                this.rtb_output.AppendText(string.Format("Reading file: {0}...{1}", oFD.FileName, Environment.NewLine));

                // 读取文件
                Task task = new Task(() =>
                {
                    ReadFile(oFD.FileName);
                });
                task.Start();

                // 分类
                Task tc = task.ContinueWith(t =>
                {
                    http = input.Where(a => a.ToLower().StartsWith(Resource.Http_Protocol)).Distinct().ToList();
                    freeProtcol = input.Where(a => a.ToLower().StartsWith(Resource.Free_Protocol)).Distinct().ToList();
                    https = input.Where(a => a.ToLower().StartsWith(Resource.Https_Protocol)).Distinct().ToList();

                    if (AutoCheckHttps)
                    {
                        http.AddRange(freeProtcol.Select(a => a.TrimStart('/').Insert(0, Resource.Http_Protocol)));
                        http = http.Distinct().ToList();

                        https.AddRange(http.Select(a => a.Insert(4, "s")));
                        https = https.Distinct().ToList();
                    }
                    string inputData = string.Format("Effective urls: {0} http, {1} https.", http.Count, https.Count);
                    SetText(inputData);
                });

                // 输出分类结果
                TaskScheduler syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
                Task cwt = tc.ContinueWith(t =>
                {
                    this.rtb_output.AppendText(string.Format("Read {0} url(s) form file.{1}", this.input.Count, Environment.NewLine));
                }, syncContextTaskScheduler);

                this.btn_Go.Enabled = true;
            }
        }

        private HttpStatusCode DoRequest(string url)
        {
            WebRequest req = null;
            HttpWebResponse res = null;
            HttpStatusCode hsc = HttpStatusCode.BadRequest;
            try
            {
                req = WebRequest.Create(url);
                req.Timeout = 1000 * 10;

                res = (HttpWebResponse)req.GetResponse();
                hsc = res.StatusCode;
                SetText(string.Format("{0}: {1}.", (int)hsc, url));
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("Do Request of {0} Error: {1}.", url, ex.Message);
                SetText(errMsg);
                LogManager.WriteLog(errMsg, ex);
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
                if (req != null)
                {
                    req.Abort();
                }
                res = null;
                req = null;
            }
            return hsc;
        }

        private void ReadFile(string path)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                input = lines
                    .Where(a => StringExtension.IsUrl(a))
                    .Distinct()
                    .ToList();
            }
            catch (Exception ex)
            {
                SetText(string.Format("ERROR: Can not read file {0}: {1}!", path, ex.Message));
            }
        }

        /// <summary>
        /// 从文件读取Urls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoBtn_file_CheckedChanged(object sender, EventArgs e)
        {
            this.btn_Go.Enabled = false;
            this.btn_Openfile.Enabled = this.rdoBtn_file.Checked;
        }

        /// <summary>
        /// 从给定的页面读取Urls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoBtn_url_CheckedChanged(object sender, EventArgs e)
        {
            this.tbx_url.Enabled = this.rdoBtn_url.Checked;
            this.btn_Go.Enabled = StringExtension.IsUrl(this.tbx_url.Text);
        }

        private void tbx_url_TextChanged(object sender, EventArgs e)
        {
            this.btn_Go.Enabled = StringExtension.IsUrl(this.tbx_url.Text);
        }

        private void SetText(string text)
        {
            if (this.rtb_output.InvokeRequired)
            {
                this.Invoke(new SetTextCallback(SetText), text);
            }
            else
            {
                this.rtb_output.AppendText(text);
                this.rtb_output.AppendText(Environment.NewLine);
                this.rtb_output.Focus();
            }
        }

        private void chb_AutoHttps_CheckedChanged(object sender, EventArgs e)
        {
            this.AutoCheckHttps = this.chb_AutoHttps.Checked;
        }
    }

    public class OutPutList
    {
        public List<string> Output_OK { get; set; }
        public List<string> OutPut_Error { get; set; }
        public int Http_Ok_Count { get { return Output_OK.Count(x => x.StartsWith(Resource.Http_Protocol)); } }
        public int Https_Ok_Count { get { return Output_OK.Count(x => x.StartsWith(Resource.Https_Protocol)); } }
        public int Http_Error_Count { get { return OutPut_Error.Count(x => x.StartsWith(Resource.Http_Protocol)); } }
        public int Https_Error_Count { get { return OutPut_Error.Count(x => x.StartsWith(Resource.Https_Protocol)); } }
        public string GetReport
        {
            get
            {
                string report = @"
Report:
\tType\tTotal\tOk\tError
\thttp\t{0}\t{1}\t{2}
\thttps\t{3}\t{4}\t{5}
";

                return string.Format(report
                    , Output_OK.Count, Http_Ok_Count, Http_Error_Count
                    , OutPut_Error.Count, Https_Ok_Count, Https_Error_Count
                    );
            }
        }
    }

    delegate void SetTextCallback(string text);

}
