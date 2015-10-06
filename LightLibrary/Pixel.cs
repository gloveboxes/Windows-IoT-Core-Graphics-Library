using System;
using Windows.UI;

/// This code is provided as-is, without any warrenty, so use it at your own risk.
/// You can freely use and modify this code.
namespace LightLibrary {

    /// <summary>
    /// From http://www.w3schools.com/HTML/html_colornames.asp
    /// </summary>
    public enum PixelColour {
        Black,
        AliceBlue,
        AntiqueWhite,
        Aqua,
        Aquamarine,
        Azure,
        Beige,
        Bisque,
        BlanchedAlmond,
        Blue,
        BlueViolet,
        Brown,
        BurlyWood,
        CadetBlue,
        Chartreuse,
        Chocolate,
        Coral,
        CornflowerBlue,
        Cornsilk,
        Crimson,
        Cyan,
        DarkBlue,
        DarkCyan,
        DarkGoldenRod,
        DarkGray,
        DarkGreen,
        DarkKhaki,
        DarkMagenta,
        DarkOliveGreen,
        DarkOrange,
        DarkOrchid,
        DarkRed,
        DarkSalmon,
        DarkSeaGreen,
        DarkSlateBlue,
        DarkSlateGray,
        DarkTurquoise,
        DarkViolet,
        DeepPink,
        DeepSkyBlue,
        DimGray,
        DodgerBlue,
        FireBrick,
        FloralWhite,
        ForestGreen,
        Fuchsia,
        Gainsboro,
        GhostWhite,
        Gold,
        GoldenRod,
        Gray,
        Green,
        GreenYellow,
        HoneyDew,
        HotPink,
        IndianRed,
        Indigo,
        Ivory,
        Khaki,
        Lavender,
        LavenderBlush,
        LawnGreen,
        LemonChiffon,
        LightBlue,
        LightCoral,
        LightCyan,
        LightGoldenRodYellow,
        LightGray,
        LightGreen,
        LightPink,
        LightSalmon,
        LightSeaGreen,
        LightSkyBlue,
        LightSlateGray,
        LightSteelBlue,
        LightYellow,
        Lime,
        LimeGreen,
        Linen,
        Magenta,
        Maroon,
        MediumAquaMarine,
        MediumBlue,
        MediumOrchid,
        MediumPurple,
        MediumSeaGreen,
        MediumSlateBlue,
        MediumSpringGreen,
        MediumTurquoise,
        MediumVioletRed,
        MidnightBlue,
        MintCream,
        MistyRose,
        Moccasin,
        NavajoWhite,
        Navy,
        OldLace,
        Olive,
        OliveDrab,
        Orange,
        OrangeRed,
        Orchid,
        PaleGoldenRod,
        PaleGreen,
        PaleTurquoise,
        PaleVioletRed,
        PapayaWhip,
        PeachPuff,
        Peru,
        Pink,
        Plum,
        PowderBlue,
        Purple,
        Red,
        RosyBrown,
        RoyalBlue,
        SaddleBrown,
        Salmon,
        SandyBrown,
        SeaGreen,
        SeaShell,
        Sienna,
        Silver,
        SkyBlue,
        SlateBlue,
        SlateGray,
        Snow,
        SpringGreen,
        SteelBlue,
        Tan,
        Teal,
        Thistle,
        Tomato,
        Turquoise,
        Violet,
        Wheat,
        White,
        WhiteSmoke,
        Yellow,
        YellowGreen

    }


    /// <summary>
    /// Class representing one pixel<br />
    /// Highly inspired by frank26080115's NeoPixel class on NeoPixel-on-NetduinoPlus2 @ github
    /// </summary>
    public class Pixel {
        public class Colour {
            #region Pixel Colour Definitions
            /// <summary>
            /// Uses the HTML Color names (found on http://www.w3schools.com/HTML/html_colornames.asp)
            /// </summary>
            public static Pixel AliceBlue = new Pixel((byte)0xF0, (byte)0xF8, (byte)0xFF);
            public static Pixel AntiqueWhite = new Pixel((byte)0xFA, (byte)0xEB, (byte)0xD7);
            public static Pixel Aqua = new Pixel((byte)0x00, (byte)0xFF, (byte)0xFF);
            public static Pixel Aquamarine = new Pixel((byte)0x7F, (byte)0xFF, (byte)0xD4);
            public static Pixel Azure = new Pixel((byte)0xF0, (byte)0xFF, (byte)0xFF);
            public static Pixel Beige = new Pixel((byte)0xF5, (byte)0xF5, (byte)0xDC);
            public static Pixel Bisque = new Pixel((byte)0xFF, (byte)0xE4, (byte)0xC4);
            public static Pixel Black = new Pixel((byte)0x00, (byte)0x00, (byte)0x00);
            public static Pixel BlanchedAlmond = new Pixel((byte)0xFF, (byte)0xEB, (byte)0xCD);
            public static Pixel Blue = new Pixel((byte)0x00, (byte)0x00, (byte)0xFF);
            public static Pixel BlueViolet = new Pixel((byte)0x8A, (byte)0x2B, (byte)0xE2);
            public static Pixel Brown = new Pixel((byte)0xA5, (byte)0x2A, (byte)0x2A);
            public static Pixel BurlyWood = new Pixel((byte)0xDE, (byte)0xB8, (byte)0x87);
            public static Pixel CadetBlue = new Pixel((byte)0x5F, (byte)0x9E, (byte)0xA0);
            public static Pixel Chartreuse = new Pixel((byte)0x7F, (byte)0xFF, (byte)0x00);
            public static Pixel Chocolate = new Pixel((byte)0xD2, (byte)0x69, (byte)0x1E);
            public static Pixel Coral = new Pixel((byte)0xFF, (byte)0x7F, (byte)0x50);
            public static Pixel CornflowerBlue = new Pixel((byte)0x64, (byte)0x95, (byte)0xED);
            public static Pixel Cornsilk = new Pixel((byte)0xFF, (byte)0xF8, (byte)0xDC);
            public static Pixel Crimson = new Pixel((byte)0xDC, (byte)0x14, (byte)0x3C);
            public static Pixel Cyan = new Pixel((byte)0x00, (byte)0xFF, (byte)0xFF);
            public static Pixel DarkBlue = new Pixel((byte)0x00, (byte)0x00, (byte)0x8B);
            public static Pixel DarkCyan = new Pixel((byte)0x00, (byte)0x8B, (byte)0x8B);
            public static Pixel DarkGoldenRod = new Pixel((byte)0xB8, (byte)0x86, (byte)0x0B);
            public static Pixel DarkGray = new Pixel((byte)0xA9, (byte)0xA9, (byte)0xA9);
            public static Pixel DarkGreen = new Pixel((byte)0x00, (byte)0x64, (byte)0x00);
            public static Pixel DarkKhaki = new Pixel((byte)0xBD, (byte)0xB7, (byte)0x6B);
            public static Pixel DarkMagenta = new Pixel((byte)0x8B, (byte)0x00, (byte)0x8B);
            public static Pixel DarkOliveGreen = new Pixel((byte)0x55, (byte)0x6B, (byte)0x2F);
            public static Pixel DarkOrange = new Pixel((byte)0xFF, (byte)0x8C, (byte)0x00);
            public static Pixel DarkOrchid = new Pixel((byte)0x99, (byte)0x32, (byte)0xCC);
            public static Pixel DarkRed = new Pixel((byte)0x8B, (byte)0x00, (byte)0x00);
            public static Pixel DarkSalmon = new Pixel((byte)0xE9, (byte)0x96, (byte)0x7A);
            public static Pixel DarkSeaGreen = new Pixel((byte)0x8F, (byte)0xBC, (byte)0x8F);
            public static Pixel DarkSlateBlue = new Pixel((byte)0x48, (byte)0x3D, (byte)0x8B);
            public static Pixel DarkSlateGray = new Pixel((byte)0x2F, (byte)0x4F, (byte)0x4F);
            public static Pixel DarkTurquoise = new Pixel((byte)0x00, (byte)0xCE, (byte)0xD1);
            public static Pixel DarkViolet = new Pixel((byte)0x94, (byte)0x00, (byte)0xD3);
            public static Pixel DeepPink = new Pixel((byte)0xFF, (byte)0x14, (byte)0x93);
            public static Pixel DeepSkyBlue = new Pixel((byte)0x00, (byte)0xBF, (byte)0xFF);
            public static Pixel DimGray = new Pixel((byte)0x69, (byte)0x69, (byte)0x69);
            public static Pixel DodgerBlue = new Pixel((byte)0x1E, (byte)0x90, (byte)0xFF);
            public static Pixel FireBrick = new Pixel((byte)0xB2, (byte)0x22, (byte)0x22);
            public static Pixel FloralWhite = new Pixel((byte)0xFF, (byte)0xFA, (byte)0xF0);
            public static Pixel ForestGreen = new Pixel((byte)0x22, (byte)0x8B, (byte)0x22);
            public static Pixel Fuchsia = new Pixel((byte)0xFF, (byte)0x00, (byte)0xFF);
            public static Pixel Gainsboro = new Pixel((byte)0xDC, (byte)0xDC, (byte)0xDC);
            public static Pixel GhostWhite = new Pixel((byte)0xF8, (byte)0xF8, (byte)0xFF);
            public static Pixel Gold = new Pixel((byte)0xFF, (byte)0xD7, (byte)0x00);
            public static Pixel GoldenRod = new Pixel((byte)0xDA, (byte)0xA5, (byte)0x20);
            public static Pixel Gray = new Pixel((byte)0x80, (byte)0x80, (byte)0x80);
            public static Pixel Green = new Pixel((byte)0x00, (byte)0x80, (byte)0x00);
            public static Pixel GreenYellow = new Pixel((byte)0xAD, (byte)0xFF, (byte)0x2F);
            public static Pixel HoneyDew = new Pixel((byte)0xF0, (byte)0xFF, (byte)0xF0);
            public static Pixel HotPink = new Pixel((byte)0xFF, (byte)0x69, (byte)0xB4);
            public static Pixel IndianRed = new Pixel((byte)0xCD, (byte)0x5C, (byte)0x5C);
            public static Pixel Indigo = new Pixel((byte)0x4B, (byte)0x00, (byte)0x82);
            public static Pixel Ivory = new Pixel((byte)0xFF, (byte)0xFF, (byte)0xF0);
            public static Pixel Khaki = new Pixel((byte)0xF0, (byte)0xE6, (byte)0x8C);
            public static Pixel Lavender = new Pixel((byte)0xE6, (byte)0xE6, (byte)0xFA);
            public static Pixel LavenderBlush = new Pixel((byte)0xFF, (byte)0xF0, (byte)0xF5);
            public static Pixel LawnGreen = new Pixel((byte)0x7C, (byte)0xFC, (byte)0x00);
            public static Pixel LemonChiffon = new Pixel((byte)0xFF, (byte)0xFA, (byte)0xCD);
            public static Pixel LightBlue = new Pixel((byte)0xAD, (byte)0xD8, (byte)0xE6);
            public static Pixel LightCoral = new Pixel((byte)0xF0, (byte)0x80, (byte)0x80);
            public static Pixel LightCyan = new Pixel((byte)0xE0, (byte)0xFF, (byte)0xFF);
            public static Pixel LightGoldenRodYellow = new Pixel((byte)0xFA, (byte)0xFA, (byte)0xD2);
            public static Pixel LightGray = new Pixel((byte)0xD3, (byte)0xD3, (byte)0xD3);
            public static Pixel LightGreen = new Pixel((byte)0x90, (byte)0xEE, (byte)0x90);
            public static Pixel LightPink = new Pixel((byte)0xFF, (byte)0xB6, (byte)0xC1);
            public static Pixel LightSalmon = new Pixel((byte)0xFF, (byte)0xA0, (byte)0x7A);
            public static Pixel LightSeaGreen = new Pixel((byte)0x20, (byte)0xB2, (byte)0xAA);
            public static Pixel LightSkyBlue = new Pixel((byte)0x87, (byte)0xCE, (byte)0xFA);
            public static Pixel LightSlateGray = new Pixel((byte)0x77, (byte)0x88, (byte)0x99);
            public static Pixel LightSteelBlue = new Pixel((byte)0xB0, (byte)0xC4, (byte)0xDE);
            public static Pixel LightYellow = new Pixel((byte)0xFF, (byte)0xFF, (byte)0xE0);
            public static Pixel Lime = new Pixel((byte)0x00, (byte)0xFF, (byte)0x00);
            public static Pixel LimeGreen = new Pixel((byte)0x32, (byte)0xCD, (byte)0x32);
            public static Pixel Linen = new Pixel((byte)0xFA, (byte)0xF0, (byte)0xE6);
            public static Pixel Magenta = new Pixel((byte)0xFF, (byte)0x00, (byte)0xFF);
            public static Pixel Maroon = new Pixel((byte)0x80, (byte)0x00, (byte)0x00);
            public static Pixel MediumAquaMarine = new Pixel((byte)0x66, (byte)0xCD, (byte)0xAA);
            public static Pixel MediumBlue = new Pixel((byte)0x00, (byte)0x00, (byte)0xCD);
            public static Pixel MediumOrchid = new Pixel((byte)0xBA, (byte)0x55, (byte)0xD3);
            public static Pixel MediumPurple = new Pixel((byte)0x93, (byte)0x70, (byte)0xDB);
            public static Pixel MediumSeaGreen = new Pixel((byte)0x3C, (byte)0xB3, (byte)0x71);
            public static Pixel MediumSlateBlue = new Pixel((byte)0x7B, (byte)0x68, (byte)0xEE);
            public static Pixel MediumSpringGreen = new Pixel((byte)0x00, (byte)0xFA, (byte)0x9A);
            public static Pixel MediumTurquoise = new Pixel((byte)0x48, (byte)0xD1, (byte)0xCC);
            public static Pixel MediumVioletRed = new Pixel((byte)0xC7, (byte)0x15, (byte)0x85);
            public static Pixel MidnightBlue = new Pixel((byte)0x19, (byte)0x19, (byte)0x70);
            public static Pixel MintCream = new Pixel((byte)0xF5, (byte)0xFF, (byte)0xFA);
            public static Pixel MistyRose = new Pixel((byte)0xFF, (byte)0xE4, (byte)0xE1);
            public static Pixel Moccasin = new Pixel((byte)0xFF, (byte)0xE4, (byte)0xB5);
            public static Pixel NavajoWhite = new Pixel((byte)0xFF, (byte)0xDE, (byte)0xAD);
            public static Pixel Navy = new Pixel((byte)0x00, (byte)0x00, (byte)0x80);
            public static Pixel OldLace = new Pixel((byte)0xFD, (byte)0xF5, (byte)0xE6);
            public static Pixel Olive = new Pixel((byte)0x80, (byte)0x80, (byte)0x00);
            public static Pixel OliveDrab = new Pixel((byte)0x6B, (byte)0x8E, (byte)0x23);
            public static Pixel Orange = new Pixel((byte)0xFF, (byte)0xA5, (byte)0x00);
            public static Pixel OrangeRed = new Pixel((byte)0xFF, (byte)0x45, (byte)0x00);
            public static Pixel Orchid = new Pixel((byte)0xDA, (byte)0x70, (byte)0xD6);
            public static Pixel PaleGoldenRod = new Pixel((byte)0xEE, (byte)0xE8, (byte)0xAA);
            public static Pixel PaleGreen = new Pixel((byte)0x98, (byte)0xFB, (byte)0x98);
            public static Pixel PaleTurquoise = new Pixel((byte)0xAF, (byte)0xEE, (byte)0xEE);
            public static Pixel PaleVioletRed = new Pixel((byte)0xDB, (byte)0x70, (byte)0x93);
            public static Pixel PapayaWhip = new Pixel((byte)0xFF, (byte)0xEF, (byte)0xD5);
            public static Pixel PeachPuff = new Pixel((byte)0xFF, (byte)0xDA, (byte)0xB9);
            public static Pixel Peru = new Pixel((byte)0xCD, (byte)0x85, (byte)0x3F);
            public static Pixel Pink = new Pixel((byte)0xFF, (byte)0xC0, (byte)0xCB);
            public static Pixel Plum = new Pixel((byte)0xDD, (byte)0xA0, (byte)0xDD);
            public static Pixel PowderBlue = new Pixel((byte)0xB0, (byte)0xE0, (byte)0xE6);
            public static Pixel Purple = new Pixel((byte)0x80, (byte)0x00, (byte)0x80);
            public static Pixel Red = new Pixel((byte)0xFF, (byte)0x00, (byte)0x00);
            public static Pixel RosyBrown = new Pixel((byte)0xBC, (byte)0x8F, (byte)0x8F);
            public static Pixel RoyalBlue = new Pixel((byte)0x41, (byte)0x69, (byte)0xE1);
            public static Pixel SaddleBrown = new Pixel((byte)0x8B, (byte)0x45, (byte)0x13);
            public static Pixel Salmon = new Pixel((byte)0xFA, (byte)0x80, (byte)0x72);
            public static Pixel SandyBrown = new Pixel((byte)0xF4, (byte)0xA4, (byte)0x60);
            public static Pixel SeaGreen = new Pixel((byte)0x2E, (byte)0x8B, (byte)0x57);
            public static Pixel SeaShell = new Pixel((byte)0xFF, (byte)0xF5, (byte)0xEE);
            public static Pixel Sienna = new Pixel((byte)0xA0, (byte)0x52, (byte)0x2D);
            public static Pixel Silver = new Pixel((byte)0xC0, (byte)0xC0, (byte)0xC0);
            public static Pixel SkyBlue = new Pixel((byte)0x87, (byte)0xCE, (byte)0xEB);
            public static Pixel SlateBlue = new Pixel((byte)0x6A, (byte)0x5A, (byte)0xCD);
            public static Pixel SlateGray = new Pixel((byte)0x70, (byte)0x80, (byte)0x90);
            public static Pixel Snow = new Pixel((byte)0xFF, (byte)0xFA, (byte)0xFA);
            public static Pixel SpringGreen = new Pixel((byte)0x00, (byte)0xFF, (byte)0x7F);
            public static Pixel SteelBlue = new Pixel((byte)0x46, (byte)0x82, (byte)0xB4);
            public static Pixel Tan = new Pixel((byte)0xD2, (byte)0xB4, (byte)0x8C);
            public static Pixel Teal = new Pixel((byte)0x00, (byte)0x80, (byte)0x80);
            public static Pixel Thistle = new Pixel((byte)0xD8, (byte)0xBF, (byte)0xD8);
            public static Pixel Tomato = new Pixel((byte)0xFF, (byte)0x63, (byte)0x47);
            public static Pixel Turquoise = new Pixel((byte)0x40, (byte)0xE0, (byte)0xD0);
            public static Pixel Violet = new Pixel((byte)0xEE, (byte)0x82, (byte)0xEE);
            public static Pixel Wheat = new Pixel((byte)0xF5, (byte)0xDE, (byte)0xB3);
            public static Pixel White = new Pixel((byte)0xFF, (byte)0xFF, (byte)0xFF);
            public static Pixel WhiteSmoke = new Pixel((byte)0xF5, (byte)0xF5, (byte)0xF5);
            public static Pixel Yellow = new Pixel((byte)0xFF, (byte)0xFF, (byte)0x00);
            public static Pixel YellowGreen = new Pixel((byte)0x9A, (byte)0xCD, (byte)0x32);

        }
        #endregion

        public class ColourLowPower {
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

        public bool State {
            get {
                return Red > 0 ? true : false;
            }
            set { Red = value == false ? (byte)0x00 : (byte)0xff; }
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

            // dglover - this doesn't work - replaced with above
            //this.Green = (byte)(argb & 0x0000FF00);
            //this.Red = (byte)(argb & 0x00FF0000);
            //this.Blue = (byte)(argb & 0x000000FF);
        }

        /// <summary>
        /// Creates the bytes needed for transfer via SPI in GRB format<br />
        /// Make sure that zero and one bytes have the same length and are properly initialized
        /// </summary>
        /// <param name="zeroBytes">bytes for zero bit</param>
        /// <param name="oneBytes">bytes for one bit</param>
        /// <returns>transfer bytes</returns>
        public byte[] ToTransferBytes(byte[] zeroBytes, byte[] oneBytes) {
            if ((zeroBytes == null) || (zeroBytes.Length == 0) || (oneBytes == null) || (oneBytes.Length == 0)) {
                return new byte[0];
            }
            int len = zeroBytes.Length;
            if (oneBytes.Length != len) {
                return new byte[0];
            }

            byte[] result = new byte[24 * len];

            int pos = 0;
            byte msk;

            msk = 128;
            for (int i = 7; i >= 0; i--) {
                byte v = (byte)(this.Green & msk);
                if (v > 0) {
                    Array.Copy(oneBytes, 0, result, pos, len);
                }
                else {
                    Array.Copy(zeroBytes, 0, result, pos, len);
                }
                pos += len;
                msk = (byte)(msk >> 1);
            }

            msk = 128;
            for (int i = 7; i >= 0; i--) {
                byte v = (byte)(this.Red & msk);
                if (v > 0) {
                    Array.Copy(oneBytes, 0, result, pos, len);
                }
                else {
                    Array.Copy(zeroBytes, 0, result, pos, len);
                }
                pos += len;
                msk = (byte)(msk >> 1);
            }

            msk = 128;
            for (int i = 7; i >= 0; i--) {
                byte v = (byte)(this.Blue & msk);
                if (v > 0) {
                    Array.Copy(oneBytes, 0, result, pos, len);
                }
                else {
                    Array.Copy(zeroBytes, 0, result, pos, len);
                }
                pos += len;
                msk = (byte)(msk >> 1);
            }

            return result;
        }

    }

}
