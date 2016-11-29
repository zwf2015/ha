using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UrlStatus
{
    public partial class Main : Form
    {
        /// <summary>
        /// Threadsafe dictionary.
        /// </summary>
        ConcurrentDictionary<string, List<string>> dic = new ConcurrentDictionary<string, List<string>>((Environment.ProcessorCount * 2), 8);

        #region Input Data

        List<string> http = new List<string>();
        List<string> https = new List<string>();
        List<string> freeProtcol = new List<string>();

        List<string> input = new List<string>();

        #endregion

        /// <summary>
        /// Auto add http urls to https's data list.
        /// </summary>
        bool AutoCheckHttps = true;

        /// <summary>
        /// Timer for button animation.
        /// </summary>
        System.Windows.Forms.Timer t1 = new System.Windows.Forms.Timer();

        /// <summary>
        /// The window thread.
        /// </summary>
        TaskScheduler windowTaskScheduler = null;

        public Main()
        {
            ServicePointManager.DefaultConnectionLimit = 512;
            InitializeComponent();
            windowTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            // Todo: Add ignore list in config.
            string ignoreUrls = System.Configuration.ConfigurationSettings.AppSettings.Get("ignores");

        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.btn_Go.Enabled = false;
            this.tbx_url.Enabled = false;
            this.lab_Saveas.Visible = false;

            t1.Interval = 100;
            t1.Tick += T1_Tick;
        }

        /// <summary>
        /// Init input data, then do reqeusts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Go_Click(object sender, EventArgs e)
        {
            this.btn_Go.Enabled = false;
            this.splitContainer2.Enabled = false;

            bool fromUrl = this.rdoBtn_url.Checked;
            string url = this.tbx_url.Text;

            t1.Start();

            Task task = new Task(() =>
            {
                if (fromUrl)
                {
                    SetRtbText(string.Format("Reading html page: {0}...{1}", url, Environment.NewLine));
                    InitUrlDatas(HtmlReader.GetHtmlByUrl(url));
                }
                SetRtbText(Environment.NewLine);
            });

            task.Start();

            // Begin checking, after input data ready.
            Task tc = task.ContinueWith(t =>
            {
                DoCheck(windowTaskScheduler);
            });
        }


        private void DoCheck(TaskScheduler windowTaskScheduler)
        {
            SetRtbText(string.Format("Start checking...{0}{1}", DateTime.Now, Environment.NewLine));
            Stopwatch so = new Stopwatch();
            so.Start();
            CancellationTokenSource cts = new CancellationTokenSource();
            Task<OutPutList> task_http = new Task<OutPutList>(() =>
            {
                OutPutList result = new OutPutList();
                result.HttpTotalCount = http.Count;
                result.HttpsTotalCount = https.Count;
                result.Output_OK = new List<RequestResult>();
                result.OutPut_Error = new List<RequestResult>();

                new Task(() =>
                {
                    SetRtbText("HTTP Begin:");
                    foreach (var url in http)
                    {
                        var res = HtmlReader.DoRequest(url);
                        if (HttpStatusCode.OK == res.HttpStatusCode)
                        {
                            result.Output_OK.Add(res);
                            SetRtbText(string.Format("\t{0}: {1}", (int)res.HttpStatusCode, url));
                        }
                        else
                        {
                            result.OutPut_Error.Add(res);
                            SetRtbText(string.Format("\tRequest of {0} is Error: {1}.", url, res.Message));
                        }
                        SetLabText(result.GetStatus);
                    }
                }, TaskCreationOptions.AttachedToParent).Start();

                new Task(() =>
                {
                    SetRtbText("HTTPS Begin:");
                    foreach (var url in https)
                    {
                        var res = HtmlReader.DoRequest(url);
                        if (HttpStatusCode.OK == res.HttpStatusCode)
                        {
                            result.Output_OK.Add(res);
                            SetRtbText(string.Format("\t{0}: {1}", (int)res.HttpStatusCode, url));
                        }
                        else
                        {
                            result.OutPut_Error.Add(res);
                            SetRtbText(string.Format("\tRequest of {0} is Error: {1}.", url, res.Message));
                        }
                        SetLabText(result.GetStatus);
                    }
                }, TaskCreationOptions.AttachedToParent).Start();
                return result;
            });
            task_http.Start();

            // Output reports.
            Task task_Report = task_http.ContinueWith(t =>
            {
                var output = t.Result;
                SetRtbText(Environment.NewLine);
                SetRtbText(output.GetReport);
                SetRtbText(Environment.NewLine);
                SetRtbText("All task done!");
                SetRtbText(Environment.NewLine);
                SetRtbText(output.GetErrorUrls);
                SetRtbText(Environment.NewLine);
                SetRtbText(output.GetHttpsErrorUrls);
                return output;
            });

            Task t3 = task_Report.ContinueWith(t =>
            {
                so.Stop();
                SetRtbText(string.Format("EX Time: {0} s.", so.ElapsedMilliseconds / 1000));
                this.splitContainer2.Enabled = true;
                this.btn_Go.Enabled = true;
                this.lab_Saveas.Visible = true;
                t1.Stop();
            }, cts.Token, TaskContinuationOptions.ExecuteSynchronously, windowTaskScheduler);

        }
        
        /// <summary>
        /// Read urls from file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                InitUrlDatas(ReadFile(oFD.FileName));
            }
        }

        private string ReadFile(string path)
        {
            var content = string.Empty;
            try
            {
                content = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                SetRtbText(string.Format("ERROR: Can not read file {0}: {1}!", path, ex.Message));
            }
            return content;
        }

        /// <summary>
        /// Init the input urls from text.
        /// </summary>
        private void InitUrlDatas(string text)
        {
            // Regex urls.
            Task task = new Task(() =>
            {
                input = Resource.GetUrls(text);
            });
            task.Start();

            // init the data list for reqeust.
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
                string inputData = string.Format("\tEffective urls: {0} http, {1} https.", http.Count, https.Count);
                SetRtbText(inputData);
            });

            // output init report.
            Task cwt = tc.ContinueWith(t =>
            {
                SetLabText(string.Format("Status: http0/{0}, https0/{1}.", http.Count, https.Count));
                this.btn_Go.Enabled = true;
            }, windowTaskScheduler);
        }

        #region winform control event

        /// <summary>
        /// Read urls from file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoBtn_file_CheckedChanged(object sender, EventArgs e)
        {
            this.btn_Go.Enabled = false;
            this.btn_Openfile.Enabled = this.rdoBtn_file.Checked;
        }

        /// <summary>
        /// Read urls from html.
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

        private void chb_AutoHttps_CheckedChanged(object sender, EventArgs e)
        {
            this.AutoCheckHttps = this.chb_AutoHttps.Checked;
        }

        /// <summary>
        /// Save report to file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lab_Saveas_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = Resource.OpenFileDialog_Filter;
            sfd.FileName = "Report_" + DateTime.Now;
            sfd.FilterIndex = 1;
            sfd.AddExtension = true;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    sw.Write(this.rtb_output.Text);
                }
            }
        }

        #endregion

        #region winfrom control delegate

        private void SetRtbText(string text)
        {
            if (this.rtb_output.InvokeRequired)
            {
                this.Invoke(new SetTextCallback(SetRtbText), text);
            }
            else
            {
                this.rtb_output.AppendText(text);
                this.rtb_output.AppendText(Environment.NewLine);
                this.rtb_output.Focus();
            }
        }

        private void SetLabText(string text)
        {
            if (this.lab_Status.InvokeRequired)
            {
                this.Invoke(new SetTextCallback(SetLabText), text);
            }
            else
            {
                this.lab_Status.Text = text;
            }
        }

        #endregion

        #region Button animation

        private bool goBackX = false;
        private bool goBackY = false;
        private void T1_Tick(object sender, EventArgs e)
        {
            var innerX = this.btn_Go.Location.X;
            var innerY = this.btn_Go.Location.Y;

            var outerX = this.splitContainer2.Size.Width - this.splitContainer2.SplitterDistance - this.btn_Go.Width - 5;
            var outerY = this.splitContainer2.Size.Height - this.btn_Go.Height;

            if (goBackX)
            {
                innerX--;
                if (innerX < 1)
                {
                    goBackX = false;
                }
            }
            else
            {
                innerX++;
                if (innerX > outerX)
                {
                    goBackX = true;
                }
            }

            if (goBackY)
            {
                innerY--;
                if (innerY < 1)
                {
                    goBackY = false;
                }
            }
            else
            {
                innerY++;
                if (innerY > outerY)
                {
                    goBackY = true;
                }
            }
            var newPoint = new System.Drawing.Point(innerX, innerY);
            this.btn_Go.Location = newPoint;
        }

        #endregion

    }
}
