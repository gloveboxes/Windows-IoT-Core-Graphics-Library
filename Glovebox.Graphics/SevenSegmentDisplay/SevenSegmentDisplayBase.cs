using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glovebox.Graphics.SevenSegmentDisplay {
    public class SevenSegmentDisplayBase {
        int panelsPerFrame = 0;
        ulong[] frame;

        static object deviceLock = new object();


        #region font

        /*
        Seven Segment Display Bitmap

                    |-64--|
                    2     32
                    |--1--|
                    4     16
                    |--8--|.128
        */




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
            84,    //    m 
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

        #endregion font



        public SevenSegmentDisplayBase(string name, int panelsPerFrame) {
            if (panelsPerFrame < 1) { throw new Exception("Number of panels must be greater than zero"); }
            this.panelsPerFrame = panelsPerFrame;
            frame = new ulong[this.panelsPerFrame];
        }

        public void DrawString(int number, int panel = 0) {
            DrawString(number.ToString(), panel);
        }

        public void DrawString(string data, int panel = 0) {
            lock (deviceLock) {
                string characters = data.ToUpper();
                char c;

                if (panel < 0 || panel >= panelsPerFrame) { return; }

                frame[panel] = 0;

                for (int i = 0; i < characters.Length; i++) {
                    c = characters.Substring(i, 1)[0];
                    if (c >= ' ' && c <= 'Z') {
                        if (c == '.') { frame[panel] += 128; }
                        else {
                            if (i > 0) { frame[panel] <<= 8; }
                            frame[panel] += Alphanumeric[c - 32];
                        }
                    }
                }
            }
        }

        public void FrameClear() {
            lock (deviceLock) {
                for (int i = 0; i < frame.Length; i++) {
                    frame[i] = 0;
                }
            }
        }


        public void FrameDraw() {
            lock (deviceLock) {
                FrameDraw(frame);
            }
        }

        protected virtual void FrameDraw(ulong[] frame) { }

    }
}
