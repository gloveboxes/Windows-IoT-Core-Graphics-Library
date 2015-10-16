using Glovebox.Graphics;
using Glovebox.Graphics.Components;
using Glovebox.Graphics.Drivers;
using Glovebox.Graphics.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedHost {
    public sealed class LedMonoMatrixSPI {

        public void Start() {

            var driver = new MAX7219(4, MAX7219.Rotate.D90, MAX7219.ChipSelect.CE0);

            LED8x8Matrix matrix = new LED8x8Matrix(driver);

            matrix.SetBrightness(1);

            while (true) {

                matrix.FrameClear();
                matrix.ScrollStringInFromRight("Hello World 2015", 100);

                matrix.FrameClear();
                matrix.ScrollStringInFromLeft("Hello World 2015", 100);

                //continue;

                for (ushort p = 0; p < matrix.PanelsPerFrame; p++) {
                    matrix.DrawSymbol(Grid8x8.Symbols.Block, Mono.On, p);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }


                for (int p = 0; p < matrix.Length; p++) {
                    matrix.FrameSet(Mono.On, p);
                    matrix.FrameSet(Mono.On, matrix.Length - 1 - p);

                    matrix.FrameDraw();
                    Task.Delay(2).Wait();

                    matrix.FrameSet(Mono.Off, p);
                    matrix.FrameSet(Mono.Off, matrix.Length - 1 - p);

                    matrix.FrameDraw();
                    Task.Delay(2).Wait();
                }


                for (int c = 0; c < matrix.ColumnsPerFrame; c = c + 2) {
                    matrix.ColumnDrawLine(c);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }


                for (int r = 0; r < matrix.RowsPerPanel; r = r + 2) {
                    matrix.RowDrawLine(r);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                Task.Delay(1000).Wait();

                for (ushort i = 0; i < matrix.PanelsPerFrame; i++) {
                    matrix.DrawLetter(i.ToString()[0], Mono.On, i);
                }

                matrix.FrameDraw();
                Task.Delay(1000).Wait();

                for (int r = 0; r < matrix.RowsPerPanel * 2; r++) {
                    matrix.FrameRollDown();
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                for (int r = 0; r < matrix.RowsPerPanel * 2; r++) {
                    matrix.FrameRollUp();
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                for (int c = 0; c < matrix.ColumnsPerFrame * 1; c++) {
                    matrix.FrameRollRight();
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                for (int c = 0; c < matrix.ColumnsPerFrame * 1; c++) {
                    matrix.FrameRollLeft();
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }


                //Task.Delay(1000).Wait();
                //continue;

                matrix.DrawString("ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890", 100, 0);
                matrix.FrameClear();

                for (int i = 0; i < matrix.RowsPerPanel; i++) {
                    matrix.DrawBox(i, i, matrix.ColumnsPerFrame - (i * 2), matrix.RowsPerPanel - (i * 2), Mono.On);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                for (byte l = 0; l < 2; l++) {
                    matrix.SetFrameState(LedDriver.Display.Off);
                    Task.Delay(250).Wait();
                    matrix.SetFrameState(LedDriver.Display.On);
                    Task.Delay(250).Wait();
                }



                matrix.FrameClear();

                for (int r = 0; r < 2; r++) {
                    for (int i = 0; i < matrix.RowsPerPanel; i++) {
                        matrix.RowDrawLine(i, i - 0, matrix.ColumnsPerFrame - i - 1);
                        matrix.FrameDraw();
                        Task.Delay(50).Wait();
                    }

                    //for (byte l = 0; l < 6; l++) {
                    //    matrix.SetBrightness(l);
                    //    Task.Delay(250).Wait();
                    //}

                    //matrix.SetBrightness(1);

                    for (int i = 0; i < matrix.RowsPerPanel; i++) {
                        matrix.RowDrawLine(i, i - 0, matrix.ColumnsPerFrame - i - 1, Mono.Off);
                        matrix.FrameDraw();
                        Task.Delay(50).Wait();
                    }
                }

                Task.Delay(500).Wait();
                matrix.FrameClear();

                matrix.ScrollStringInFromRight("Hello World ", 100);
                matrix.FrameClear();
                matrix.ScrollStringInFromLeft("Happy Birthday ", 100);


                for (int i = 0; i < matrix.fontSimple.Length; i = i + matrix.PanelsPerFrame) {
                    for (int p = 0; p < matrix.PanelsPerFrame; p++) {
                        if (p + i >= matrix.fontSimple.Length) { break; }
                        matrix.DrawBitmap(matrix.fontSimple[p + i], Mono.On, (p + i) % matrix.PanelsPerFrame);
                    }
                    matrix.FrameDraw();
                    Task.Delay(100 * matrix.PanelsPerFrame).Wait();
                }

                foreach (Grid8x8.Symbols sym in Enum.GetValues(typeof(Grid8x8.Symbols))) {
                    for (int p = 0; p < matrix.PanelsPerFrame; p++) {
                        matrix.DrawSymbol(sym, Mono.On, p);
                    }
                    matrix.FrameDraw();
                    Task.Delay(100 * matrix.PanelsPerFrame).Wait();
                }
            }
        }
    }
}
