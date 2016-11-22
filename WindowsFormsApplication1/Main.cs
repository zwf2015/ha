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

        List<string> input = new List<string>();

        public Main()
        {
            ServicePointManager.DefaultConnectionLimit = 512;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.btn_Go.Enabled = false;
        }

        private void Test()
        {
            for (int i = 0; i < 20; i++)
            {
                input.Add(Resource.Test_Url);
            }

            Task task = new Task(() =>
            {
                HtmlReader.DoRequest(Resource.Test_Url);
            });

            task.Start();
        }

        

        private void btn_Go_Click(object sender, EventArgs e)
        {
            TaskScheduler syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Task<int> task = new Task<int>(() =>
            {
                return (int)HtmlReader.DoRequest(Resource.Test_Url);
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

                // 读取文件到内存
                Task task = new Task(() =>
                {
                    ReadFile(oFD.FileName);
                });
                task.Start();

                // 分类
                Task tc = task.ContinueWith(t =>
                {
                    var http = input.Where(a => a.StartsWith(Resource.Http_Protocol)).ToList();
                    var freeProtcol = input.Where(a => a.StartsWith(Resource.Free_Protocol)).ToList();
                    var https = input.Where(a => a.StartsWith(Resource.Https_Protocol)).ToList();


                });
                tc.Start();

                // 输出分类结果
                TaskScheduler syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
                Task cwt = tc.ContinueWith(t =>
                {
                    this.rtb_output.AppendText(string.Format("Read {0} url(s) form file.{1}", this.input.Count, Environment.NewLine));
                }, syncContextTaskScheduler);

                this.btn_Go.Enabled = true;
            }
        }

        private void ReadFile(string path)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                input = lines
                    .Where(a => StringExtension.IsUrl(a))
                    // .Distinct()
                    .ToList();
            }
            catch (Exception ex)
            {
                SetText(string.Format("ERROR: Can not read file {0}: {1}!", path, ex.Message));
            }
        }

        private void rdoBtn_file_CheckedChanged(object sender, EventArgs e)
        {
            this.btn_Openfile.Visible = this.rdoBtn_file.Checked;
        }

        private void rdoBtn_url_CheckedChanged(object sender, EventArgs e)
        {
            this.tbx_url.Visible = this.rdoBtn_url.Checked;
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

    }

    delegate void SetTextCallback(string text);

}
