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
using S22.Imap;
using System.Net.Mail;
using System.Reflection;
using Microsoft.WindowsAPICodePack.ApplicationServices;

namespace Ambilight
{
    public partial class Form1 : Form
    {
        internal static BlinkStick bs;
        int numberOfLeds = 32;
        internal bool ambientOn = false;

        void MonitorOnChanged(object sender, EventArgs e)
        {
            Globals.MonitorOn = PowerManager.IsMonitorOn;
            if (Globals.MonitorOn == false)
                TurnAllLEDOff();
            //Console.WriteLine(string.Format("Monitor status changed (new status: {0})", PowerManager.IsMonitorOn ? "On" : "Off"));
        }

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
            PowerManager.IsMonitorOnChanged += new EventHandler(MonitorOnChanged);
            typeof(PictureBox).InvokeMember("DoubleBuffered",
   BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
   null, picPreview, new object[] { true });

            loadsettings();
            changeUseRegions();

            if (args.Contains("mini"))
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }

            bs = BlinkStickDotNet.BlinkStick.FindFirst();
            bs.OpenDevice();

            startEmail();

            if (args.Contains("start"))
                startAmbient();
        }

        private void loadsettings()
        {
            int px = Properties.Settings.Default.PixelStep;
            int rg = Properties.Settings.Default.RegionSize;
            decimal gm = Properties.Settings.Default.Gamma;
            bool chkr = Properties.Settings.Default.UseRegions;
            Color colr = Properties.Settings.Default.ScanColour;
            colorDialog1.Color = colr;
            numPixels.Value = px;
            numRegion.Value = rg;
            numGamma.Value = gm;
            chkRegions.Checked = chkr;
            changeUseRegions();
        }

        private void savesettings()
        {
            Properties.Settings.Default.PixelStep = (int)numPixels.Value;
            Properties.Settings.Default.RegionSize = (int)numRegion.Value;
            Properties.Settings.Default.Gamma = numGamma.Value;
            Properties.Settings.Default.UseRegions = chkRegions.Checked;
            Properties.Settings.Default.ScanColour = colorDialog1.Color;
            Properties.Settings.Default.Save();
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
                            System.Threading.Thread.Sleep(20);
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
                        System.Threading.Thread.Sleep(20);
                    }
                }
                catch (Exception ex3)
                { Console.WriteLine(ex3.Message); }
            }
        }

        public void DoFire(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            Random rand = new Random();
            int r = 255;
            int g = 40;
            int b = 30;
            int rmin = 120;
            int gmin = 10;
            int bmin = 10;
            int rmax = 255;
            int gmax = 80;
            int bmax = 30;

            byte[] colorData = new byte[numberOfLeds * 3];
            for (byte ib = 0; ib < numberOfLeds; ib++)
            {
                colorData[ib * 3] = (byte)g;
                colorData[ib * 3 + 1] = (byte)r;
                colorData[ib * 3 + 2] = (byte)b;
            }

            while (true)
            {
                try
                {                   
                    while (true)
                    {
                        if ((worker.CancellationPending == true))
                        {
                            e.Cancel = true;
                            break;
                        }
                        else
                        {
                            newFire(rand, rmin, gmin, bmin, rmax, gmax, bmax, colorData);
                        }

                    }
                    if ((worker.CancellationPending == true || e.Cancel))
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        newFire(rand, rmin, gmin, bmin, rmax, gmax, bmax, colorData);
                        System.Threading.Thread.Sleep(20);
                    }
                }
                catch (Exception ex3)
                { Console.WriteLine(ex3.Message); }
            }
        }

        private void newFire(Random rand, int rmin, int gmin, int bmin, int rmax, int gmax, int bmax, byte[] colorData)
        {
            if (!Globals.MonitorOn)
            {
                TurnAllLEDOff();
                return;
            }
            
            for (byte ib = 0; ib < numberOfLeds; ib++)
            {
                int flicker = rand.Next(0, 40);
                flicker -= 15;
                int g1 = colorData[ib * 3] + flicker;
                int r1 = colorData[ib * 3 + 1] + flicker;
                int b1 = colorData[ib * 3 + 2] + flicker;
                if (g1 < gmin) g1 = gmin;
                if (r1 < rmin) r1 = rmin;
                if (b1 < bmin) b1 = bmin;
                if (g1 > gmax) g1 = gmax;
                if (r1 > rmax) r1 = rmax;
                if (b1 > bmax) b1 = bmax;

                colorData[ib * 3] = (byte)g1;
                colorData[ib * 3 + 1] = (byte)r1;
                colorData[ib * 3 + 2] = (byte)b1;
            }

            if (Globals.MonitorOn) bs.SetColors(0, colorData);
            System.Threading.Thread.Sleep(rand.Next(10, 50));
        }

        int scanTime = 10;
        public void DoScanner(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            int counter = 0;
            byte[] colorData = new byte[numberOfLeds * 3];

            try
            {
                while (true)
                {
                    if ((worker.CancellationPending == true))
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        for (counter = 0; counter < 32; counter++)
                        {
                            counter = newScanner(colorData, counter, true);
                            System.Threading.Thread.Sleep(scanTime);
                        }

                        for (counter = 31; counter > -1; counter--)
                        {
                            counter = newScanner(colorData, counter, false);
                            System.Threading.Thread.Sleep(scanTime);
                        }

                    }
                }
            }
            catch (Exception ex3)
            { Console.WriteLine(ex3.Message); }            
        }

        private int newScanner(byte[] colorData, int counter, bool forward = true)
        {
            if (!Globals.MonitorOn)
            {
                TurnAllLEDOff();
                return 0;
            }
            int r = colorDialog1.Color.R;
            int g = colorDialog1.Color.G;
            int b = colorDialog1.Color.B;

            int hr = (int)(r * 0.6);
            int hg = (int)(g * 0.6);
            int hb = (int)(b * 0.6);

            for (byte ib = 0; ib < numberOfLeds; ib++)
                {
                    if (ib == counter)
                    {
                        colorData[ib * 3] = (byte)g;
                        colorData[ib * 3 + 1] = (byte)r;
                        colorData[ib * 3 + 2] = (byte)b;
                    }
                    else if (ib == counter + 1)
                    {
                        colorData[ib * 3] = (byte)hg;
                        colorData[ib * 3 + 1] = (byte)hr;
                        colorData[ib * 3 + 2] = (byte)hb;
                    }
                    else
                    {
                        colorData[ib * 3] = (byte)0;
                        colorData[ib * 3 + 1] = (byte)0;
                        colorData[ib * 3 + 2] = (byte)0;
                    }
                }
                if (Globals.MonitorOn) bs.SetColors(0, colorData);
                   
            
            return counter;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stopAmbient();
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

        private void startEmail()
        {
            timerEmail.Enabled = true;
            timerEmail.Start();
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
            button3.Enabled = false;
            button4.Enabled = false;
        }

        private void startFire()
        {
            button2.Enabled = true;
            bw = new BackgroundWorker();
            bw.DoWork += DoFire;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.WorkerSupportsCancellation = true;
            bw.RunWorkerAsync();
            button1.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
        }

        private void startScanner()
        {
            button2.Enabled = true;
            bw = new BackgroundWorker();
            bw.DoWork += DoScanner;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.WorkerSupportsCancellation = true;
            bw.RunWorkerAsync();
            button1.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = true;
            TurnAllLEDOff();
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
            if (this.bw != null)
                this.bw.CancelAsync();
            // Disable the Cancel button.
            button2.Enabled = false;
            TurnAllLEDOff();
        }

        private void pixelStep_ValueChanged(object sender, EventArgs e)
        {
            int.TryParse(numPixels.Value.ToString(), out Globals.pixelStep);
            savesettings();
        }

        private void gamma_ValueChanged(object sender, EventArgs e)
        {
            Globals.setgamma((double)(numGamma.Value));
            savesettings();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Globals.setgamma((double)(numGamma.Value));
            useRegionsToolStripMenuItem.Checked = chkRegions.Checked;
        }

        private void chkPreview_CheckedChanged(object sender, EventArgs e)
        {
            Globals.preview = chkPreview.Checked;
        }

        private void chkRegions_CheckedChanged(object sender, EventArgs e)
        {
            savesettings();
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
        private Bitmap bmppreview = null;

        private void showbmp(Bitmap bmp)
        {
            
            bmppreview = new Bitmap(picPreview.Width, picPreview.Height);
            Graphics g =  Graphics.FromImage(bmppreview);
            //g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            g.Clear(Color.White);
            //g.DrawImageUnscaled(bmp, 0, 0);
            g.DrawImage(bmp,picPreview.DisplayRectangle);
            if (Globals.useRegions)
            {
                Pen pen1 = new Pen(System.Drawing.Color.Green, 2);
                Brush brush1 = new SolidBrush(System.Drawing.Color.FromArgb(128, 255, 255, 255));
                for (int i = 0; i < 32; i++)
                {
                    Rectangle rect = new Rectangle((int)(Globals.LEDRegions[i].rect.Left * (1.0f * picPreview.Width / bmp.Width)),
                        (int)(Globals.LEDRegions[i].rect.Top * (1.0f *picPreview.Height / bmp.Height)),
                        (int)((Globals.LEDRegions[i].rect.Right - Globals.LEDRegions[i].rect.Left) * (1.0f * picPreview.Width / bmp.Width)),
                        (int)((Globals.LEDRegions[i].rect.Bottom - Globals.LEDRegions[i].rect.Top) * (1.0f * picPreview.Height / bmp.Height))
                        );
                    g.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(255, Globals.LEDRegions[i].R, Globals.LEDRegions[i].G, Globals.LEDRegions[i].B)), rect);
                    g.DrawRectangle(pen1, rect);
                }               
            }
            g.Dispose();

            Graphics gw = picPreview.CreateGraphics();
            gw.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            gw.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            gw.DrawImageUnscaled(bmppreview, 0, 0);
            gw.Dispose();
            //picPreview.Image = bmp;
        }

        private void RegionSize_ValueChanged(object sender, EventArgs e)
        {
            Globals.region_size = (int)(numRegion.Value);
            Globals.setRegions();
            savesettings();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
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

        private void timerEmail_Tick(object sender, EventArgs e)
        {
            checkEmail();
        }

        private List<uint> seenUIDS = new List<uint>();
        private void checkEmail()
        {
            try
            {
                using (ImapClient Client = new ImapClient("imap-mail.outlook.com", 993,
                 "jeremyp@outlook.com", "p!ngutw000", AuthMethod.Login, true))
                {
                    IEnumerable<uint> uids = Client.Search(SearchCondition.Unseen());
                    int unseen = 0;
                    foreach (uint uid in uids)
                    {
                        if (!seenUIDS.Contains(uid))
                        {
                            //new unseen email!
                            //MailMessage m = Client.GetMessage(uid);
                            seenUIDS.Add(uid);
                            unseen++;
                        }
                    }
                    if (unseen > 0)
                    {
                        blink();
                    }
                    if (uids.Count() > 0)
                        lblEmail.Text = "Unread email: " + uids.Count().ToString();
                    else
                        lblEmail.Text = "";
                }
            }
            catch (Exception ex) { }
        }

        private void turnallOn(byte r, byte g, byte b)
        {
            byte[] colorData = new byte[numberOfLeds * 3];
            for (byte ib = 0; ib < numberOfLeds; ib++)
            {
                colorData[ib * 3] = r;
                colorData[ib * 3 + 1] = g;
                colorData[ib * 3 + 2] = b;
            }
            if ( Globals.MonitorOn) bs.SetColors(0, colorData);
        }

        private void blink()
        {
            Globals.pause = true;
            turnallOn(0, 255, 255);
            System.Threading.Thread.Sleep(400);
            TurnAllLEDOff();
            System.Threading.Thread.Sleep(300);
            turnallOn(0, 255, 255);
            System.Threading.Thread.Sleep(400);
            TurnAllLEDOff();
            System.Threading.Thread.Sleep(300);
            turnallOn(0, 255, 255);
            System.Threading.Thread.Sleep(400);
            Globals.pause = false;
        }

        private void lblEmail_Click(object sender, EventArgs e)
        {

        }

        private void numTimer_ValueChanged(object sender, EventArgs e)
        {
            timerEmail.Interval = (int)(numTimer.Value) * 1000;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            stopAmbient();
            startFire();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            stopAmbient();
            startScanner();           
        }

        private void cmdScanColor_Click(object sender, EventArgs e)
        {
            DialogResult r = colorDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                savesettings();
            }
        }
    }
}
