using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlinkStickDotNet;

namespace Ambilight
{
    public partial class Form1 : Form
    {
        internal static BlinkStick bs;
        int numberOfLeds = 32;
        internal bool ambientOn = false;

        public Form1()
        {
            InitializeComponent();
            bs = BlinkStickDotNet.BlinkStick.FindFirst();
            bs.OpenDevice();
        }

        public Form1(string[] args)
        {
            this.args = args;
            InitializeComponent();

            if (args.Contains("mini"))
                this.WindowState = FormWindowState.Minimized;

            bs = BlinkStickDotNet.BlinkStick.FindFirst();
            bs.OpenDevice();
            
            if (args.Contains("start"))
                startAmbient();
        }

        BackgroundWorker bw;
        private string[] args;

        public void DoAmbient(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while (true)
            {
                try
                {
                    AmbiCapture ac = new AmbiCapture();
                    ac.MainForm = this;
                    ac.getColor();
                    while (true)
                    {
                        if ((worker.CancellationPending == true))
                        {
                            ac.Cancel();
                            e.Cancel = true;
                            break;
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(10);
                        }

                    }
                    if ((worker.CancellationPending == true || e.Cancel))
                    {
                        ac.Cancel();
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(10);
                    }
                }
                catch (Exception ex3)
                { Console.WriteLine(ex3.Message); }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ambientOnToolStripMenuItem.Checked = true;
            startAmbient();
            //Open the device
            //if (bs.OpenDevice())
            //{
            //    int numberOfLeds = 32;

            //    for (byte i = 0; i < numberOfLeds; i++)
            //    {
            //        Console.WriteLine(String.Format("Device {0} opened successfully", bs.Serial));
            //        Random r = new Random();
            //        bs.SetColor(0, i, (byte)r.Next(), (byte)r.Next(), (byte)r.Next());
            //    }

            //}
        }

        private void startAmbient()
        {
            ambientOnToolStripMenuItem.Checked = true;
            ambientOn = true;
            button2.Enabled = true;
            bw = new BackgroundWorker();
            bw.DoWork += DoAmbient;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.WorkerSupportsCancellation = true;
            bw.RunWorkerAsync();
            button1.Enabled = false;
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseDown();
        }

        private void CloseDown()
        {
            this.bw.CancelAsync();
            System.Threading.Thread.Sleep(100);
            TurnAllLEDOff();
        }

        private void TurnAllLEDOff()
        {
            byte[] colorData = new byte[numberOfLeds * 3];
            for (byte ib = 0; ib < numberOfLeds; ib++)
            {
                colorData[ib * 3] = 0;
                colorData[ib * 3 + 1] = 0;
                colorData[ib * 3 + 2] = 0;
            }

            bs.SetColors(0, colorData);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stopAmbient();
        }

        private void stopAmbient()
        {
            ambientOn = false;
            ambientOnToolStripMenuItem.Checked = false;
            this.bw.CancelAsync();
            // Disable the Cancel button.
            button2.Enabled = false;
            TurnAllLEDOff();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int.TryParse(numericUpDown1.Value.ToString(), out Globals.pixelStep);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            Globals.setgamma((double)(numericUpDown2.Value));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Globals.setgamma((double)(numericUpDown2.Value));
        }

        private void chkPreview_CheckedChanged(object sender, EventArgs e)
        {
            Globals.preview = chkPreview.Checked;
        }

        private void chkRegions_CheckedChanged(object sender, EventArgs e)
        {
            changeUseRegions();
        }

        private void changeUseRegions()
        {
            Globals.useRegions = chkRegions.Checked;
            useRegionsToolStripMenuItem.Checked = Globals.useRegions;
        }

        public void setPic(Bitmap b)
        {
            try
            {
                this.Invoke((MethodInvoker)(() =>
                   showbmp(b) // runs on UI thread
                ));
            }
            catch { }
        }

        private bool creatingpreview = false;

        private void showbmp(Bitmap bmp)
        {           
            if (Globals.useRegions)
            {                
                Graphics g = Graphics.FromImage(bmp);
                Pen pen1 = new Pen(System.Drawing.Color.Green, 2);
                Brush brush1 = new SolidBrush(System.Drawing.Color.FromArgb(128, 255, 255, 255));
                for (int i = 0; i < 32; i++)
                {
                    g.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(255, Globals.LEDRegions[i].R, Globals.LEDRegions[i].G, Globals.LEDRegions[i].B)), Globals.LEDRegions[i].rect);
                    g.DrawRectangle(pen1, Globals.LEDRegions[i].rect);
                }
                g.Dispose();
            }
            picPreview.Image = bmp;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            Globals.region_size = (int)(numericUpDown3.Value);
            Globals.setRegions();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void ambientOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ambientOn)
            {
                stopAmbient();
            }
            else
            {
                startAmbient();
            }
        }

        private void useRegionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Globals.useRegions)
            {
                Globals.useRegions = false;
                useRegionsToolStripMenuItem.Checked = false;
                chkRegions.Checked = false;
            }
            else
            {
                Globals.useRegions = true;
                useRegionsToolStripMenuItem.Checked = true;
                chkRegions.Checked = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
