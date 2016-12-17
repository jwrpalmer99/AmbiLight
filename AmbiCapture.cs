using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using System.Windows.Forms;
using SlimDX.Direct3D9;

namespace Ambilight
{
    class AmbiCapture
    {
        DxScreenCapture sc;
        public Form1 MainForm;

        //private CaptureInterface captureInterface;
        //private CaptureProcess captureProcess;

        private uint HookedProcessId = 0;
        private CaptureModeEnum CaptureMode = CaptureModeEnum.Desktop;
        private static DesktopCapture dc=null;
        private enum CaptureModeEnum
        {
            None,
            Desktop,
            Application
        }

        public void getColor()
        {            
            if (CaptureMode == CaptureModeEnum.Desktop)
            {
                dc = new DesktopCapture();
                dc.MainForm = MainForm;
                dc.capture();               
            }
        }

        internal void Cancel()
        {
            dc.Cancel();
            dc.Dispose();
        }
    }

    public class DxScreenCapture
    {
        Device d;

        public DxScreenCapture()
        {
            PresentParameters present_params = new PresentParameters();
            present_params.Windowed = true;
            present_params.SwapEffect = SwapEffect.Discard;
            d = new Device(new Direct3D(), 0, DeviceType.Hardware, IntPtr.Zero, CreateFlags.SoftwareVertexProcessing, present_params);
        }

        public Surface CaptureScreen()
        {
            Surface s = Surface.CreateOffscreenPlain(d, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, Format.A8R8G8B8, Pool.Scratch);
            d.GetFrontBufferData(0, s);
            return s;
        }
    }
}
