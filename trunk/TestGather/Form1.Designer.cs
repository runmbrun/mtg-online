namespace TestMTGGatherer
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.t = new System.Windows.Forms.TabPage();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonFilter = new System.Windows.Forms.Button();
            this.comboBoxInstrument = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonCardSetSave = new System.Windows.Forms.Button();
            this.buttonCardSetLoad = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBoxCardImage = new System.Windows.Forms.PictureBox();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTimer = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxFetchPagesInParallel = new System.Windows.Forms.TextBox();
            this.comboBoxFetchMTGEdition = new System.Windows.Forms.ComboBox();
            this.buttonFetch = new System.Windows.Forms.Button();
            this.linkLabelDefault = new System.Windows.Forms.LinkLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonCreatePicturePack = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.t.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCardImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.t);
            this.tabControl1.Location = new System.Drawing.Point(1, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(791, 612);
            this.tabControl1.TabIndex = 0;
            // 
            // t
            // 
            this.t.Controls.Add(this.textBox2);
            this.t.Controls.Add(this.textBox1);
            this.t.Controls.Add(this.label3);
            this.t.Controls.Add(this.buttonFilter);
            this.t.Controls.Add(this.comboBoxInstrument);
            this.t.Location = new System.Drawing.Point(4, 22);
            this.t.Name = "t";
            this.t.Padding = new System.Windows.Forms.Padding(3);
            this.t.Size = new System.Drawing.Size(783, 586);
            this.t.TabIndex = 0;
            this.t.Text = "Card Set Results";
            this.t.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(371, 23);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 9;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(253, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Choose Color";
            // 
            // buttonFilter
            // 
            this.buttonFilter.Location = new System.Drawing.Point(152, 20);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(75, 23);
            this.buttonFilter.TabIndex = 6;
            this.buttonFilter.Text = "Filter Results";
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // comboBoxInstrument
            // 
            this.comboBoxInstrument.FormattingEnabled = true;
            this.comboBoxInstrument.Location = new System.Drawing.Point(13, 20);
            this.comboBoxInstrument.Name = "comboBoxInstrument";
            this.comboBoxInstrument.Size = new System.Drawing.Size(121, 21);
            this.comboBoxInstrument.TabIndex = 4;
            this.comboBoxInstrument.SelectedIndexChanged += new System.EventHandler(this.comboBoxInstrument_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.linkLabelDefault);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(783, 586);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Importing Data";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.buttonCreatePicturePack);
            this.groupBox2.Controls.Add(this.buttonCardSetSave);
            this.groupBox2.Controls.Add(this.buttonCardSetLoad);
            this.groupBox2.Location = new System.Drawing.Point(398, 173);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(372, 95);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Load And Save Card Sets";
            // 
            // buttonCardSetSave
            // 
            this.buttonCardSetSave.Location = new System.Drawing.Point(120, 17);
            this.buttonCardSetSave.Name = "buttonCardSetSave";
            this.buttonCardSetSave.Size = new System.Drawing.Size(75, 23);
            this.buttonCardSetSave.TabIndex = 0;
            this.buttonCardSetSave.Text = "Save";
            this.buttonCardSetSave.UseVisualStyleBackColor = true;
            this.buttonCardSetSave.Click += new System.EventHandler(this.buttonCardSetSave_Click);
            // 
            // buttonCardSetLoad
            // 
            this.buttonCardSetLoad.Location = new System.Drawing.Point(25, 17);
            this.buttonCardSetLoad.Name = "buttonCardSetLoad";
            this.buttonCardSetLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonCardSetLoad.TabIndex = 0;
            this.buttonCardSetLoad.Text = "Load";
            this.buttonCardSetLoad.UseVisualStyleBackColor = true;
            this.buttonCardSetLoad.Click += new System.EventHandler(this.buttonCardSetLoad_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBoxCardImage);
            this.groupBox1.Controls.Add(this.dataGridViewResults);
            this.groupBox1.Location = new System.Drawing.Point(6, 274);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(770, 306);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Results of Card Set Search";
            // 
            // pictureBoxCardImage
            // 
            this.pictureBoxCardImage.Location = new System.Drawing.Point(564, 15);
            this.pictureBoxCardImage.Name = "pictureBoxCardImage";
            this.pictureBoxCardImage.Size = new System.Drawing.Size(200, 285);
            this.pictureBoxCardImage.TabIndex = 10;
            this.pictureBoxCardImage.TabStop = false;
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.AllowUserToAddRows = false;
            this.dataGridViewResults.AllowUserToDeleteRows = false;
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.Location = new System.Drawing.Point(3, 15);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(561, 285);
            this.dataGridViewResults.TabIndex = 0;
            this.dataGridViewResults.SelectionChanged += new System.EventHandler(this.dataGridViewResults_SelectionChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.textBoxTimer);
            this.groupBox3.Controls.Add(this.progressBar1);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.textBoxFetchPagesInParallel);
            this.groupBox3.Controls.Add(this.comboBoxFetchMTGEdition);
            this.groupBox3.Controls.Add(this.buttonFetch);
            this.groupBox3.Location = new System.Drawing.Point(9, 173);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(383, 95);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Fetch Card Set Data from Web Site";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(253, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Time Taken for Results";
            // 
            // textBoxTimer
            // 
            this.textBoxTimer.Location = new System.Drawing.Point(256, 46);
            this.textBoxTimer.Name = "textBoxTimer";
            this.textBoxTimer.ReadOnly = true;
            this.textBoxTimer.Size = new System.Drawing.Size(114, 20);
            this.textBoxTimer.TabIndex = 10;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 72);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(358, 17);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(168, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "In Parallel";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(158, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Pages to Fetch";
            // 
            // textBoxFetchPagesInParallel
            // 
            this.textBoxFetchPagesInParallel.Location = new System.Drawing.Point(160, 46);
            this.textBoxFetchPagesInParallel.Name = "textBoxFetchPagesInParallel";
            this.textBoxFetchPagesInParallel.Size = new System.Drawing.Size(54, 20);
            this.textBoxFetchPagesInParallel.TabIndex = 9;
            this.textBoxFetchPagesInParallel.Text = "1";
            // 
            // comboBoxFetchMTGEdition
            // 
            this.comboBoxFetchMTGEdition.FormattingEnabled = true;
            this.comboBoxFetchMTGEdition.Location = new System.Drawing.Point(12, 19);
            this.comboBoxFetchMTGEdition.Name = "comboBoxFetchMTGEdition";
            this.comboBoxFetchMTGEdition.Size = new System.Drawing.Size(121, 21);
            this.comboBoxFetchMTGEdition.TabIndex = 6;
            // 
            // buttonFetch
            // 
            this.buttonFetch.Location = new System.Drawing.Point(12, 43);
            this.buttonFetch.Name = "buttonFetch";
            this.buttonFetch.Size = new System.Drawing.Size(75, 26);
            this.buttonFetch.TabIndex = 0;
            this.buttonFetch.Text = "Fetch";
            this.buttonFetch.UseVisualStyleBackColor = true;
            this.buttonFetch.Click += new System.EventHandler(this.buttonFetch_Click);
            // 
            // linkLabelDefault
            // 
            this.linkLabelDefault.AutoSize = true;
            this.linkLabelDefault.Location = new System.Drawing.Point(18, 18);
            this.linkLabelDefault.Name = "linkLabelDefault";
            this.linkLabelDefault.Size = new System.Drawing.Size(145, 13);
            this.linkLabelDefault.TabIndex = 1;
            this.linkLabelDefault.TabStop = true;
            this.linkLabelDefault.Text = "Magic the Gathering Website";
            this.linkLabelDefault.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDefault_LinkClicked);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 615);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.TabIndex = 1;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // buttonCreatePicturePack
            // 
            this.buttonCreatePicturePack.Location = new System.Drawing.Point(139, 56);
            this.buttonCreatePicturePack.Name = "buttonCreatePicturePack";
            this.buttonCreatePicturePack.Size = new System.Drawing.Size(75, 23);
            this.buttonCreatePicturePack.TabIndex = 1;
            this.buttonCreatePicturePack.Text = "Create";
            this.buttonCreatePicturePack.UseVisualStyleBackColor = true;
            this.buttonCreatePicturePack.Click += new System.EventHandler(this.buttonCreatePicturePack_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Create a Picture Pack:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 637);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Magic the Gather";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.t.ResumeLayout(false);
            this.t.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCardImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage t;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.LinkLabel linkLabelDefault;
        private System.Windows.Forms.Button buttonFetch;
        private System.Windows.Forms.ComboBox comboBoxInstrument;
        private System.Windows.Forms.Button buttonFilter;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBoxFetchMTGEdition;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxFetchPagesInParallel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxTimer;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBoxCardImage;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonCardSetSave;
        private System.Windows.Forms.Button buttonCardSetLoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCreatePicturePack;
    }
}

