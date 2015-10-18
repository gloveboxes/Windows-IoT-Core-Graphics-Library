using System;

/// This code is provided as-is, without any warrenty, so use it at your own risk.
/// You can freely use and modify this code.
namespace Glovebox.Graphics {

    /// <summary>
    /// From http://www.w3schools.com/HTML/html_colornames.asp
    /// </summary>
    public enum PixelColour {
        White,
        Black,
        Red,
        Orange,
        Yellow,
        Green,
        Purple,
        Blue,
    }

    public static class Led {
        public static Pixel On = new Pixel((byte)0xFF, (byte)0x00, (byte)0x00);
        public static Pixel Off = new Pixel((byte)0x00, (byte)0x00, (byte)0x00);
    }

    public static class BiColour {
        public static Pixel Red = new Pixel((byte)0xFF, (byte)0x00, (byte)0x00);
        public static Pixel Green = new Pixel((byte)0x00, (byte)0xFF, (byte)0x00);
        public static Pixel Yellow = new Pixel((byte)0xFF, (byte)0xFF, (byte)0x00);
    }

    public static class Colour {
        public static Pixel White = new Pixel((byte)0xFF, (byte)0xFF, (byte)0xFF);
        public static Pixel Black = new Pixel((byte)0x00, (byte)0x00, (byte)0x00);

        public static Pixel Red = new Pixel((byte)0xFF, (byte)0x00, (byte)0x00);
        public static Pixel Orange = new Pixel((byte)0xFF, (byte)0xA5, (byte)0x00);
        public static Pixel Yellow = new Pixel((byte)0xFF, (byte)0xFF, (byte)0x00);
        public static Pixel Green = new Pixel((byte)0x00, (byte)0x80, (byte)0x00);
        public static Pixel Purple = new Pixel((byte)0x80, (byte)0x00, (byte)0x80);
        public static Pixel Blue = new Pixel((byte)0x00, (byte)0x00, (byte)0xFF);

        public static Pixel CoolRed = new Pixel(0x020000);
        public static Pixel CoolOrange = new Pixel(0x040200);
        public static Pixel CoolYellow = new Pixel(0x020200);
        public static Pixel CoolGreen = new Pixel(0x000200);
        public static Pixel CoolBlue = new Pixel(0x000002);
        public static Pixel CoolPurple = new Pixel(0x020002);

        public static Pixel WarmRed = new Pixel(0x080000);
        public static Pixel WarmOrange = new Pixel(0x080400);
        public static Pixel WarmYellow = new Pixel(0x090900);
        public static Pixel WarmGreen = new Pixel(0x000800);
        public static Pixel WarmBlue = new Pixel(0x000008);
        public static Pixel WarmPurple = new Pixel(0x080008);

        public static Pixel HotRed = new Pixel(0x160000);
        public static Pixel HotOrange = new Pixel(0x160800);
        public static Pixel HotYellow = new Pixel(0x161600);
        public static Pixel HotGreen = new Pixel(0x001600);
        public static Pixel HotBlue = new Pixel(0x000016);
        public static Pixel HotPurple = new Pixel(0x160016);
    }


    /// <summary>
    /// Class representing one pixel<br />
    /// Highly inspired by frank26080115's NeoPixel class on NeoPixel-on-NetduinoPlus2 @ github
    /// </summary>
    public class Pixel {

        public int ColourValue {
            get {
                return this.Red << 16 | this.Green << 8 | this.Blue;
            }
        }

        /// <summary>
        /// Green, 0 to 255
        /// </summary>
        public byte Green {
            get;
            set;
        }

        /// <summary>
        /// Red, 0 to 255
        /// </summary>
        public byte Red {
            get;
            set;
        }

        /// <summary>
        /// Blue, 0 to 255
        /// </summary>
        public byte Blue {
            get;
            set;
        }

        /// <summary>
        /// Creates a new pixel, black
        /// </summary>
        public Pixel()
            : this((byte)0, (byte)0, (byte)0) {
        }

        /// <summary>
        /// Creates a new pixel with given color
        /// </summary>
        /// <param name="r">Initial red, 0 to 255</param>
        /// <param name="g">Initial green, 0 to 255</param>
        /// <param name="b">Initial blue, 0 to 255</param>
        public Pixel(byte r, byte g, byte b) {
            this.Green = g;
            this.Red = r;
            this.Blue = b;
        }

        /// <summary>
        /// Creates a new pixel with given color
        /// </summary>
        /// <param name="r">Initial red, 0 to 255</param>
        /// <param name="g">Initial green, 0 to 255</param>
        /// <param name="b">Initial blue, 0 to 255</param>
        public Pixel(int r, int g, int b) {
            this.Green = (byte)g;
            this.Red = (byte)r;
            this.Blue = (byte)b;
        }

        /// <summary>
        /// Creates a new pixel with given ARGB color, where A is ignored
        /// </summary>
        /// <param name="argb">ARGB color value</param>
        public Pixel(int argb) {
            this.Blue = (byte)argb;
            this.Green = (byte)(argb >> 8);
            this.Red = (byte)(argb >> 16);
        }
    }
}
