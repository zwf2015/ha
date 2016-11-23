namespace WindowsFormsApplication1
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbx_url = new System.Windows.Forms.TextBox();
            this.rdoBtn_url = new System.Windows.Forms.RadioButton();
            this.rdoBtn_file = new System.Windows.Forms.RadioButton();
            this.btn_Openfile = new System.Windows.Forms.Button();
            this.btn_Go = new System.Windows.Forms.Button();
            this.rtb_output = new System.Windows.Forms.RichTextBox();
            this.chb_AutoHttps = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rtb_output);
            this.splitContainer1.Size = new System.Drawing.Size(676, 478);
            this.splitContainer1.SplitterDistance = 133;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.chb_AutoHttps);
            this.splitContainer2.Panel2.Controls.Add(this.btn_Go);
            this.splitContainer2.Size = new System.Drawing.Size(676, 133);
            this.splitContainer2.SplitterDistance = 387;
            this.splitContainer2.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbx_url);
            this.groupBox1.Controls.Add(this.rdoBtn_url);
            this.groupBox1.Controls.Add(this.rdoBtn_file);
            this.groupBox1.Controls.Add(this.btn_Openfile);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(387, 133);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input";
            // 
            // tbx_url
            // 
            this.tbx_url.Location = new System.Drawing.Point(94, 60);
            this.tbx_url.Name = "tbx_url";
            this.tbx_url.Size = new System.Drawing.Size(100, 20);
            this.tbx_url.TabIndex = 4;
            this.tbx_url.Text = "http://www.newegg.com";
            this.tbx_url.TextChanged += new System.EventHandler(this.tbx_url_TextChanged);
            // 
            // rdoBtn_url
            // 
            this.rdoBtn_url.AutoSize = true;
            this.rdoBtn_url.Location = new System.Drawing.Point(24, 60);
            this.rdoBtn_url.Name = "rdoBtn_url";
            this.rdoBtn_url.Size = new System.Drawing.Size(64, 17);
            this.rdoBtn_url.TabIndex = 3;
            this.rdoBtn_url.Text = "From Url";
            this.rdoBtn_url.UseVisualStyleBackColor = true;
            this.rdoBtn_url.CheckedChanged += new System.EventHandler(this.rdoBtn_url_CheckedChanged);
            // 
            // rdoBtn_file
            // 
            this.rdoBtn_file.AutoSize = true;
            this.rdoBtn_file.Checked = true;
            this.rdoBtn_file.Location = new System.Drawing.Point(24, 25);
            this.rdoBtn_file.Name = "rdoBtn_file";
            this.rdoBtn_file.Size = new System.Drawing.Size(67, 17);
            this.rdoBtn_file.TabIndex = 2;
            this.rdoBtn_file.TabStop = true;
            this.rdoBtn_file.Text = "From File";
            this.rdoBtn_file.UseVisualStyleBackColor = true;
            this.rdoBtn_file.CheckedChanged += new System.EventHandler(this.rdoBtn_file_CheckedChanged);
            // 
            // btn_Openfile
            // 
            this.btn_Openfile.Location = new System.Drawing.Point(94, 23);
            this.btn_Openfile.Name = "btn_Openfile";
            this.btn_Openfile.Size = new System.Drawing.Size(100, 21);
            this.btn_Openfile.TabIndex = 1;
            this.btn_Openfile.Text = "Openfile";
            this.btn_Openfile.UseVisualStyleBackColor = true;
            this.btn_Openfile.Click += new System.EventHandler(this.btn_Openfile_Click);
            // 
            // btn_Go
            // 
            this.btn_Go.Location = new System.Drawing.Point(64, 40);
            this.btn_Go.Name = "btn_Go";
            this.btn_Go.Size = new System.Drawing.Size(76, 36);
            this.btn_Go.TabIndex = 0;
            this.btn_Go.Text = "go";
            this.btn_Go.UseVisualStyleBackColor = true;
            this.btn_Go.Click += new System.EventHandler(this.btn_Go_Click);
            // 
            // rtb_output
            // 
            this.rtb_output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_output.Location = new System.Drawing.Point(0, 0);
            this.rtb_output.Name = "rtb_output";
            this.rtb_output.Size = new System.Drawing.Size(676, 341);
            this.rtb_output.TabIndex = 0;
            this.rtb_output.Text = "";
            // 
            // chb_AutoHttps
            // 
            this.chb_AutoHttps.AutoSize = true;
            this.chb_AutoHttps.Checked = true;
            this.chb_AutoHttps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_AutoHttps.Location = new System.Drawing.Point(13, 85);
            this.chb_AutoHttps.Name = "chb_AutoHttps";
            this.chb_AutoHttps.Size = new System.Drawing.Size(104, 17);
            this.chb_AutoHttps.TabIndex = 1;
            this.chb_AutoHttps.Text = "AutoCheckHttps";
            this.chb_AutoHttps.UseVisualStyleBackColor = true;
            this.chb_AutoHttps.CheckedChanged += new System.EventHandler(this.chb_AutoHttps_CheckedChanged);
            // 
            // Main
            // 
            this.AcceptButton = this.btn_Go;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 478);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HttpsSpider";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox rtb_output;
        private System.Windows.Forms.Button btn_Go;
        private System.Windows.Forms.Button btn_Openfile;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoBtn_url;
        private System.Windows.Forms.RadioButton rdoBtn_file;
        private System.Windows.Forms.TextBox tbx_url;
        private System.Windows.Forms.CheckBox chb_AutoHttps;
    }
}

