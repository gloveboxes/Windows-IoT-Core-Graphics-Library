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
        

        public byte[] numbers = new byte[] {
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

        public SevenSegmentDisplayBase(string name, int panelsPerFrame) : base(64)
        {
            NumberOfPanels = panelsPerFrame;

        }

        public void DrawNumber(int number)
        {
            ulong bm = 0;
            
 
            string data = number.ToString();

            int len = data.Length < 9 ? data.Length : 8;

            for (int i = 0; i < len; i++)
            {
                if (i > 0) { bm <<= 8; }
               var c =  (int)(data[i]) - 48;
                bm = bm + numbers[c];
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
