using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambilight
{
    public class LEDRegion
    {
        public int LEDindex { get; set; }
        public Rectangle rect { get; set; }
        public bool enabled = true;

        public int R = 0;
        public int G = 0;
        public int B = 0;
    }
}
