using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glovebox.Graphics.SevenSegmentDisplay
{
    public class SevenSegmentDisplayBase : FrameBase
    {
        int NumberOfPanels = 1;
        int PixelsPerPanel = 64;


        public byte[] Numeric = new byte[] {
            1, // minus
            128, // decimal point
            0, // blank
            126, // zero
            48, // one
            109, // two
            121, // three
            51, // four
            91, // five
            95, // six
            112, // seven
            127, // eight
            115, // nine
        };

        public enum Special : byte
        {
            minus = 1,
            space = 0
        }

        public byte[] Alphanumeric = new byte[]
        {
            0, // space
            0, // !            
            0, // "
            0, // #
            0, // $
            0, // %  
            0, // &
            0, // '            
            0, // (
            0, // )
            0, // *
            0, // +
            0, // ,

            1, // -
            128, // .
            0, // blank
            126, // 0
            48, // 1
            109, // 2
            121, // 3
            51, // 4
            91, // 5
            95, // 6
            112, // 7
            127, // 8
            115, // 9

            0, // :
            0, // ;
            0, // <
            0, // =
            0, // >
            0, // ?
            0, // @


            119,   //    a 
            31,    //    b 
            78,    //    c 
            61,    //    d 
            79,    //    e 
            71,    //    f 
            123,   //    g 
            23,    //    h 
            6,     //    i 
            60,    //    j 
            55,    //    k 
            14,    //    l 
            89,    //    m 
            21,    //    n 
            126,   //    o 
            103,   //    p 
            115,   //    q 
            5,     //    r 
            91,    //    s 
            15,    //    t 
            62,    //    u 
            28,    //    v 
            42,    //    w 
            55,    //    x 
            59,    //    y 
            109    //    z 
        };



        public SevenSegmentDisplayBase(string name, int panelsPerFrame) : base(64)
        {
            NumberOfPanels = panelsPerFrame;

        }

        public void DrawString(int number)
        {
            DrawString(number.ToString());
        }

        public void DrawString(string value)
        {
            string characters = value.ToUpper();
            char c;
            ulong bm = 0;

            int len = characters.Length > 8 ? 8 : characters.Length;

            for (int i = 0; i < len; i++)
            {
                c = characters.Substring(i, 1)[0];
                if (c >= ' ' && c <= 'Z')
                {
                    if (i > 0 && c != '.') { bm <<= 8; }
                    if (c == '.') { bm += 128; }
                    else { bm += Alphanumeric[c - 32]; }
                }
            }

            DrawBitmap(bm);
        }

        public virtual void DrawBitmap(ulong bitmap, int panel = 0)
        {
            ulong mask;
            if (panel < 0 || panel >= NumberOfPanels) { return; }

            for (int pos = 0; pos < PixelsPerPanel; pos++)
            {

                mask = (ulong)1 << (int)pos;
                if ((bitmap & mask) == 0)
                {
                    FrameSet(Led.Off, (int)pos, 1);
                }
                else {
                    FrameSet(Led.On, (int)pos, 1);
                }
            }
        }

    }
}
