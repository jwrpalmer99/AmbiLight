namespace Ambilight
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.chkPreview = new System.Windows.Forms.CheckBox();
            this.numPixels = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numGamma = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.chkRegions = new System.Windows.Forms.CheckBox();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numRegion = new System.Windows.Forms.NumericUpDown();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ambientOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useRegionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.timerEmail = new System.Windows.Forms.Timer(this.components);
            this.lblEmail = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numTimer = new System.Windows.Forms.NumericUpDown();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.cmdScanColor = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPixels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGamma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRegion)).BeginInit();
            this.menuIcon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimer)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(230, 152);
            this.button1.TabIndex = 0;
            this.button1.Text = "start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(252, 31);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 92);
            this.button2.TabIndex = 1;
            this.button2.Text = "stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // chkPreview
            // 
            this.chkPreview.AutoSize = true;
            this.chkPreview.Location = new System.Drawing.Point(326, 445);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size(88, 24);
            this.chkPreview.TabIndex = 2;
            this.chkPreview.Text = "preview";
            this.chkPreview.UseVisualStyleBackColor = true;
            this.chkPreview.CheckedChanged += new System.EventHandler(this.chkPreview_CheckedChanged);
            // 
            // numPixels
            // 
            this.numPixels.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numPixels.Location = new System.Drawing.Point(102, 195);
            this.numPixels.Name = "numPixels";
            this.numPixels.Size = new System.Drawing.Size(120, 26);
            this.numPixels.TabIndex = 3;
            this.numPixels.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.numPixels.ValueChanged += new System.EventHandler(this.pixelStep_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 195);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "pixel step";
            // 
            // numGamma
            // 
            this.numGamma.DecimalPlaces = 1;
            this.numGamma.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numGamma.Location = new System.Drawing.Point(102, 279);
            this.numGamma.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numGamma.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numGamma.Name = "numGamma";
            this.numGamma.Size = new System.Drawing.Size(120, 26);
            this.numGamma.TabIndex = 5;
            this.numGamma.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numGamma.ValueChanged += new System.EventHandler(this.gamma_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 281);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "gamma";
            // 
            // chkRegions
            // 
            this.chkRegions.AutoSize = true;
            this.chkRegions.Checked = true;
            this.chkRegions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRegions.Location = new System.Drawing.Point(12, 320);
            this.chkRegions.Name = "chkRegions";
            this.chkRegions.Size = new System.Drawing.Size(117, 24);
            this.chkRegions.TabIndex = 7;
            this.chkRegions.Text = "use regions";
            this.chkRegions.UseVisualStyleBackColor = true;
            this.chkRegions.CheckedChanged += new System.EventHandler(this.chkRegions_CheckedChanged);
            // 
            // picPreview
            // 
            this.picPreview.Location = new System.Drawing.Point(420, 31);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(700, 438);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPreview.TabIndex = 8;
            this.picPreview.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 240);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "region size";
            // 
            // numRegion
            // 
            this.numRegion.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numRegion.Location = new System.Drawing.Point(102, 238);
            this.numRegion.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numRegion.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numRegion.Name = "numRegion";
            this.numRegion.Size = new System.Drawing.Size(120, 26);
            this.numRegion.TabIndex = 9;
            this.numRegion.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numRegion.ValueChanged += new System.EventHandler(this.RegionSize_ValueChanged);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.menuIcon;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "AmbiLight";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // menuIcon
            // 
            this.menuIcon.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ambientOnToolStripMenuItem,
            this.useRegionsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuIcon.Name = "menuIcon";
            this.menuIcon.Size = new System.Drawing.Size(195, 94);
            // 
            // ambientOnToolStripMenuItem
            // 
            this.ambientOnToolStripMenuItem.CheckOnClick = true;
            this.ambientOnToolStripMenuItem.Name = "ambientOnToolStripMenuItem";
            this.ambientOnToolStripMenuItem.Size = new System.Drawing.Size(194, 30);
            this.ambientOnToolStripMenuItem.Text = "Ambient On";
            this.ambientOnToolStripMenuItem.Click += new System.EventHandler(this.ambientOnToolStripMenuItem_Click);
            // 
            // useRegionsToolStripMenuItem
            // 
            this.useRegionsToolStripMenuItem.CheckOnClick = true;
            this.useRegionsToolStripMenuItem.Name = "useRegionsToolStripMenuItem";
            this.useRegionsToolStripMenuItem.Size = new System.Drawing.Size(194, 30);
            this.useRegionsToolStripMenuItem.Text = "Use Regions";
            this.useRegionsToolStripMenuItem.Click += new System.EventHandler(this.useRegionsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(194, 30);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(8, 400);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(162, 24);
            this.checkBox1.TabIndex = 11;
            this.checkBox1.Text = "alert on new email";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // timerEmail
            // 
            this.timerEmail.Interval = 60000;
            this.timerEmail.Tick += new System.EventHandler(this.timerEmail_Tick);
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(4, 469);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(103, 20);
            this.lblEmail.TabIndex = 13;
            this.lblEmail.Text = "unread mails:";
            this.lblEmail.Click += new System.EventHandler(this.lblEmail_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 432);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "check rate (s)";
            // 
            // numTimer
            // 
            this.numTimer.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTimer.Location = new System.Drawing.Point(119, 430);
            this.numTimer.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.numTimer.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTimer.Name = "numTimer";
            this.numTimer.Size = new System.Drawing.Size(86, 26);
            this.numTimer.TabIndex = 14;
            this.numTimer.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numTimer.ValueChanged += new System.EventHandler(this.numTimer_ValueChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(264, 155);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(90, 34);
            this.button3.TabIndex = 16;
            this.button3.Text = "FireFX";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(264, 226);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(90, 34);
            this.button4.TabIndex = 17;
            this.button4.Text = "Scanner";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // cmdScanColor
            // 
            this.cmdScanColor.BackgroundImage = global::Ambilight.Properties.Resources._9881os_abr3_3;
            this.cmdScanColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cmdScanColor.Location = new System.Drawing.Point(298, 264);
            this.cmdScanColor.Name = "cmdScanColor";
            this.cmdScanColor.Size = new System.Drawing.Size(56, 41);
            this.cmdScanColor.TabIndex = 18;
            this.cmdScanColor.UseVisualStyleBackColor = true;
            this.cmdScanColor.Click += new System.EventHandler(this.cmdScanColor_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1140, 498);
            this.Controls.Add(this.cmdScanColor);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numTimer);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numRegion);
            this.Controls.Add(this.picPreview);
            this.Controls.Add(this.chkRegions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numGamma);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numPixels);
            this.Controls.Add(this.chkPreview);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "AmbiLight";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.numPixels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGamma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRegion)).EndInit();
            this.menuIcon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numTimer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox chkPreview;
        private System.Windows.Forms.NumericUpDown numPixels;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numGamma;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkRegions;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numRegion;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip menuIcon;
        private System.Windows.Forms.ToolStripMenuItem ambientOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useRegionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Timer timerEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numTimer;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button cmdScanColor;
    }
}

