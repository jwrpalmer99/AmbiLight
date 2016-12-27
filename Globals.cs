using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambilight
{
    public static class Globals
    {

        public static int pixelStep = 40;
        public static int timeout = 20;
        public static int region_size = 120;
        public static int _width = 2560;
        public static int _height = 1440;

        public static LEDRegion[] LEDRegions = new LEDRegion[32];

        public static void setRegions()
        {
            setRegions(_width, _height);
        }

        public static void setRegions(int width, int height)
        {
            _width = width;
            _height = height;
            double heightpercent = 0.7f; //ratio of screen height with leds on

            int h =  (int)((heightpercent *  height) / 6.0); //7 led on each side
            int w =  (int)(Math.Ceiling((width - 2 * region_size) / 18.0)); //18 led along top
            int starth = h * 6;

            //left side
            for (int i = 0; i < 8; i++)
            {
                LEDRegions[i] = new LEDRegion();
                LEDRegions[i].LEDindex = i;
                LEDRegions[i].rect = new System.Drawing.Rectangle(0, starth - (i * h), region_size, h);
            }

            //topside
            for (int i = 7; i < 25; i++)
            {
                LEDRegions[i] = new LEDRegion();
                LEDRegions[i].LEDindex = i;
                LEDRegions[i].rect = new System.Drawing.Rectangle((i - 7)* w + region_size, 0, w, region_size);
            }

            //right side
            for (int i = 25; i < 32; i++)
            {
                LEDRegions[i] = new LEDRegion();
                LEDRegions[i].LEDindex = i;
                LEDRegions[i].rect = new System.Drawing.Rectangle(width - region_size, starth - ((i-25) * h), region_size, h);
            }
        }

        public static int[] gamma8 = new int[] {
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1,  1,  1,  1,
            1,  1,  1,  1,  1,  1,  1,  1,  1,  2,  2,  2,  2,  2,  2,  2,
            2,  3,  3,  3,  3,  3,  3,  3,  4,  4,  4,  4,  4,  5,  5,  5,
            5,  6,  6,  6,  6,  7,  7,  7,  7,  8,  8,  8,  9,  9,  9, 10,
           10, 10, 11, 11, 11, 12, 12, 13, 13, 13, 14, 14, 15, 15, 16, 16,
           17, 17, 18, 18, 19, 19, 20, 20, 21, 21, 22, 22, 23, 24, 24, 25,
           25, 26, 27, 27, 28, 29, 29, 30, 31, 32, 32, 33, 34, 35, 35, 36,
           37, 38, 39, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 50,
           51, 52, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 66, 67, 68,
           69, 70, 72, 73, 74, 75, 77, 78, 79, 81, 82, 83, 85, 86, 87, 89,
           90, 92, 93, 95, 96, 98, 99,101,102,104,105,107,109,110,112,114,
          115,117,119,120,122,124,126,127,129,131,133,135,137,138,140,142,
          144,146,148,150,152,154,156,158,160,162,164,167,169,171,173,175,
          177,180,182,184,186,189,191,193,196,198,200,203,205,208,210,213,
          215,218,220,223,225,228,231,233,236,239,241,244,247,249,252,255 };
        internal static bool useRegions;
        internal static bool preview;
        internal static Bitmap previewbitmap;
        internal static bool pause;

        public static void setgamma(double gamma)
        {
            for (int i = 0; i < 256; i++)
            {
                gamma8[i] = Clamp((int)((255.0 * System.Math.Pow(i / 255.0, 1.0 / gamma)) + 0.5), 255, 0);

            }
        }

        private static int Clamp(int Value, int Max, int Min)
        {
            Value = Value > Max? Max : Value;
            Value = Value<Min? Min : Value;
            return Value;
        }
}
}
