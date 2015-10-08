using Glovebox.Graphics;
using Glovebox.Graphics.Components;
using Glovebox.Graphics.Drivers;
using Glovebox.Graphics.Grid;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace LedHost {
    public sealed class StartupTask : IBackgroundTask {

        BackgroundTaskDeferral _deferral;

        public void Run(IBackgroundTaskInstance taskInstance) {
            _deferral = taskInstance.GetDeferral();

            //LED8x8MatrixMAX7219 matrix = new LED8x8MatrixMAX7219(new MAX7219(MAX7219.Rotate.D90, MAX7219.ChipSelect.CE1), 5);

            LED8x8MatrixHT16K33 matrix = new LED8x8MatrixHT16K33(new Ht16K33(112, LedDriver.Display.On,1));

            matrix.SetBrightness(1);

            while (true) {

                matrix.FrameClear();
                matrix.ScrollStringInFromRight("Hello World 2015", 100);

                matrix.FrameClear();
                matrix.ScrollStringInFromLeft("Hello World 2015", 100);

                //continue;

                for (ushort p = 0; p < matrix.Panels; p++) {
                    matrix.DrawSymbol(Grid8x8.Symbols.Block, Pixel.Mono.On, p);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }


                for (int p = 0; p < matrix.Length; p++) {
                    matrix.FrameSet(Pixel.Mono.On, p);
                    matrix.FrameSet(Pixel.Mono.On, (int)(matrix.Length - 1 - p));

                    matrix.FrameDraw();
                    Task.Delay(2).Wait();

                    matrix.FrameSet(Pixel.Mono.Off, p);
                    matrix.FrameSet(Pixel.Mono.Off, (int)(matrix.Length - 1 - p));

                    matrix.FrameDraw();
                    Task.Delay(2).Wait();
                }


                for (int c = 0; c < matrix.ColumnsPerRow; c = c + 2) {
                    matrix.ColumnDrawLine((ushort)c);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }


                for (int r = 0; r < matrix.RowsPerPanel; r = r + 2) {
                    matrix.RowDrawLine(r);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                Task.Delay(1000).Wait();

                for (ushort i = 0; i < matrix.Panels; i++) {
                    matrix.DrawLetter(i.ToString()[0], Pixel.Mono.On, i);
                }

                matrix.FrameDraw();
                Task.Delay(1000).Wait();

                for (int r = 0; r < matrix.RowsPerPanel * 2; r++) {
                    matrix.FrameRowDown();
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                for (int r = 0; r < matrix.RowsPerPanel * 2; r++) {
                    matrix.FrameRowUp();
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                for (int c = 0; c < matrix.ColumnsPerRow * 1; c++) {
                    matrix.FrameRollRight();
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                for (int c = 0; c < matrix.ColumnsPerRow * 1; c++) {
                    matrix.FrameRollLeft();
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }


                //Task.Delay(1000).Wait();
                //continue;

                matrix.DrawString("ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890", 100, 0);
                matrix.FrameClear();

                for (int i = 0; i < matrix.RowsPerPanel; i++) {
                    matrix.DrawBox(i, i, (int)matrix.ColumnsPerRow - (i * 2), (int)matrix.RowsPerPanel - (i * 2), Pixel.Mono.On);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                for (byte l = 0; l < 2; l++) {
                    matrix.SetDisplayState(LedDriver.Display.Off);
                    Task.Delay(250).Wait();
                    matrix.SetDisplayState(LedDriver.Display.On);
                    Task.Delay(250).Wait();
                }



                matrix.FrameClear();

                for (int r = 0; r < 2; r++) {
                    for (int i = 0; i < matrix.RowsPerPanel; i++) {
                        matrix.RowDrawLine(i, i - 0, matrix.ColumnsPerRow - i - 1);
                        matrix.FrameDraw();
                        Task.Delay(50).Wait();
                    }

                    //for (byte l = 0; l < 6; l++) {
                    //    matrix.SetBrightness(l);
                    //    Task.Delay(250).Wait();
                    //}

                    //matrix.SetBrightness(1);

                    for (int i = 0; i < matrix.RowsPerPanel; i++) {
                        matrix.RowDrawLine(i, i - 0, matrix.ColumnsPerRow - i - 1, Pixel.Mono.Off);
                        matrix.FrameDraw();
                        Task.Delay(50).Wait();
                    }
                }

                Task.Delay(500).Wait();
                matrix.FrameClear();

                matrix.ScrollStringInFromRight("Hello World ", 100);
                matrix.FrameClear();
                matrix.ScrollStringInFromLeft("Happy Birthday ", 100);


                for (int i = 0; i < matrix.fontSimple.Length; i = (int)(i + matrix.Panels)) {
                    for (int p = 0; p < matrix.Panels; p++) {
                        if (p + i >= matrix.fontSimple.Length) { break; }
                        matrix.DrawBitmap(matrix.fontSimple[p + i], Pixel.Mono.On, (ushort)((p + i) % matrix.Panels));
                    }
                    matrix.FrameDraw();
                    Task.Delay(100 * matrix.Panels).Wait();
                }

                foreach (Grid8x8.Symbols sym in Enum.GetValues(typeof(Grid8x8.Symbols))) {
                    for (int p = 0; p < matrix.Panels; p++) {
                        matrix.DrawSymbol(sym, Pixel.Mono.On, (ushort)p);
                    }
                    matrix.FrameDraw();
                    Task.Delay(100 * matrix.Panels).Wait();
                }
            }
        }
    }
}