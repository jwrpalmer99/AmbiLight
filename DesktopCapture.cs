using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DXGI;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Ambilight
{
    class DesktopCapture
    {
        public Form1 MainForm;
        BackgroundWorker bw;
        OutputDuplication duplicatedOutput;
        Resource screenResource;
        SharpDX.DataStream dataStream;
        Surface screenSurface;
        SharpDX.Direct3D11.Device device;
        Factory1 factory;
        SharpDX.Direct3D11.Texture2DDescription texdes;
        SharpDX.Direct3D11.Texture2D screenTexture;

        internal void Dispose()
        {
            try
            {
                duplicatedOutput.Dispose();
                screenResource.Dispose();
                dataStream.Dispose();
                screenSurface.Dispose();
                device.Dispose();
                factory.Dispose();
            }
            catch { }
        }

        public void capture()
        {
            uint numAdapter = 0; // # of graphics card adapter
            uint numOutput = 0; // # of output device (i.e. monitor)

            // create device and factory
            device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware);
            factory = new Factory1();

            // creating CPU-accessible texture resource
            texdes = new SharpDX.Direct3D11.Texture2DDescription();
            texdes.CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.Read;
            texdes.BindFlags = SharpDX.Direct3D11.BindFlags.None;
            texdes.Format = Format.B8G8R8A8_UNorm;
            texdes.Height = factory.Adapters1[numAdapter].Outputs[numOutput].Description.DesktopBounds.Bottom;
            texdes.Width = factory.Adapters1[numAdapter].Outputs[numOutput].Description.DesktopBounds.Right;
            texdes.OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None;
            texdes.MipLevels = 1;
            texdes.ArraySize = 1;
            texdes.SampleDescription.Count = 1;
            texdes.SampleDescription.Quality = 0;
            texdes.Usage = SharpDX.Direct3D11.ResourceUsage.Staging;
            screenTexture = new SharpDX.Direct3D11.Texture2D(device, texdes);

            // duplicate output stuff
            Output1 output = new Output1(factory.Adapters1[numAdapter].Outputs[numOutput].NativePointer);
            duplicatedOutput = output.DuplicateOutput(device);
            screenResource = null;
                   
            Globals.setRegions(texdes.Width, texdes.Height);

            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerAsync();
            return;
        }

        internal void Cancel()
        {
            bw.CancelAsync();
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
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
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        int i = 0;
                        i++;
                        // try to get duplicated frame within given time
                        try
                        {
                            OutputDuplicateFrameInformation duplicateFrameInformation;
                            duplicatedOutput.AcquireNextFrame(Globals.timeout, out duplicateFrameInformation, out screenResource);
                        }
                        catch (SharpDX.SharpDXException ex)
                        {
                            if (ex.ResultCode.Code == SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code)
                            {
                                // this has not been a successful capture
                                // thanks @Randy
                                i--;

                                // keep retrying
                                continue;
                            }
                            else
                            {
                                throw ex;
                            }
                        }

                        // copy resource into memory that can be accessed by the CPU
                        device.ImmediateContext.CopyResource(screenResource.QueryInterface<SharpDX.Direct3D11.Texture2D>(), screenTexture);
                        // cast from texture to surface, so we can access its bytes
                        screenSurface = screenTexture.QueryInterface<Surface>();

                        // map the resource to access it
                        screenSurface.Map(MapFlags.Read, out dataStream);

                        // seek within the stream and read one byte
                        dataStream.Position = 0;

                        if (!Globals.useRegions)
                        {
                            long[] totals = new long[] { 0, 0, 0 };

                            IntPtr backing = dataStream.DataPointer;

                            int step = Globals.pixelStep;
                            long len = dataStream.Length;
                            unsafe
                            {
                                byte* p = (byte*)(void*)backing;

                                for (long y = 0; y < len; y += 4 * step)
                                {
                                    {
                                        totals[0] += p[y];
                                        totals[1] += p[y + 1];
                                        totals[2] += p[y + 2];
                                    }
                                }
                            }

                            long avgB = totals[0] / (texdes.Width * texdes.Height / step);
                            long avgG = totals[1] / (texdes.Width * texdes.Height / step);
                            long avgR = totals[2] / (texdes.Width * texdes.Height / step);


                            int numberOfLeds = 32;

                            byte[] colorData = new byte[numberOfLeds * 3];
                            for (byte ib = 0; ib < numberOfLeds; ib++)
                            {
                                colorData[ib * 3] = (byte)(Globals.gamma8[avgG]);
                                colorData[ib * 3 + 1] = (byte)(Globals.gamma8[avgR]);
                                colorData[ib * 3 + 2] = (byte)(Globals.gamma8[avgB]);
                                //Form1.bs.SetColor(0, ib, (byte)(avgR), (byte)(avgG), (byte)(avgB));
                            }

                         
                            Form1.bs.SetColors(0, colorData);

                            if (Globals.preview)
                            {
                                ShowPreview();
                                //bmp.Dispose();
                            }
                        }
                        else
                        {
                            int numberOfLeds = 32;
                            byte[] colorData = new byte[numberOfLeds * 3];

                            int stride = texdes.Width * 4;
                            IntPtr backing = dataStream.DataPointer;

                            int step = Globals.pixelStep;
                            long len = dataStream.Length;

                            for (int region = 0; region < 32; region++)
                            {
                                long[] totals = new long[] { 0, 0, 0 };
                                                                
                                int left = Globals.LEDRegions[region].rect.Left;
                                int top = Globals.LEDRegions[region].rect.Top;

                                int w = Globals.LEDRegions[region].rect.Width;
                                int h = Globals.LEDRegions[region].rect.Height;

                                unsafe
                                {
                                    long c = 0;
                                    byte* p = (byte*)(void*)backing;

                                    for (int dx = 0; dx < w; dx += Globals.pixelStep)
                                    {
                                        for (int dy = 0; dy < h; dy += Globals.pixelStep)
                                        {
                                            for (int color = 0; color < 3; color++)
                                            {
                                                int idx = ((top + dy) * stride) + (left + dx) * 4 + color;
                                                totals[color] += p[idx];
                                            }
                                           
                                            c++;
                                        }
                                    }

                                    long avgB = totals[0] / c;
                                    long avgG = totals[1] / c;
                                    long avgR = totals[2] / c;

                                    Globals.LEDRegions[region].R = (int)avgR;
                                    Globals.LEDRegions[region].G = (int)avgG;
                                    Globals.LEDRegions[region].B = (int)avgB;

                                    colorData[region * 3] = (byte)(Globals.gamma8[avgG]);
                                    colorData[region * 3 + 1] = (byte)(Globals.gamma8[avgR]);
                                    colorData[region * 3 + 2] = (byte)(Globals.gamma8[avgB]);
                                    
                                }

                                
                            }
                            //bmp.UnlockBits(bmpData);
                            Form1.bs.SetColors(0, colorData);

                            if (Globals.preview)
                            {
                                ShowPreview();
                                //bmp.Dispose();
                            }
                        }
                        //int stride = bmpData.Stride;

                        //IntPtr Scan0 = bmpData.Scan0;



                        //int width = bmp.Width;
                        //int height = bmp.Height;

                        //unsafe
                        //{
                        //    byte* p = (byte*)(void*)Scan0;

                        //    for (int y = 0; y < height; y++)
                        //    {
                        //        for (int x = 0; x < width; x++)
                        //        {
                        //            for (int color = 0; color < 3; color++)
                        //            {
                        //                int idx = (y * stride) + x * 4 + color;

                        //                totals[color] += p[idx];
                        //            }
                        //        }
                        //    }
                        //}



                        //bmp.UnlockBits(bmpData);

                        // free resources
                        dataStream.Close();
                        screenSurface.Unmap();
                        screenSurface.Dispose();
                        screenResource.Dispose();
                        duplicatedOutput.ReleaseFrame();

                        //bmp.Save(@"C:\temp\temp.bmp");
                        
                        sw.Stop();
                        Debug.WriteLine("time: " + sw.ElapsedMilliseconds.ToString() + "ms");
                    }
                }
            }
            catch (Exception ex2)
            { }
        }

        private void ShowPreview()
        {
            Bitmap bmp = new Bitmap(texdes.Width, texdes.Height, PixelFormat.Format32bppArgb);

            var BoundsRect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(BoundsRect, ImageLockMode.WriteOnly, bmp.PixelFormat);

            int bytes = bmpData.Stride * bmp.Height;
            var rgbValues = new byte[bytes];

            int a = dataStream.Read(rgbValues, 0, bytes);
            Marshal.Copy(rgbValues, 0, bmpData.Scan0, bytes);
            bmp.UnlockBits(bmpData);
           
            MainForm.setPic(bmp);      
        }
    }
}
