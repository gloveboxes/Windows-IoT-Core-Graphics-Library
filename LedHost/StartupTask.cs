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

            LED8x8MatrixMAX7219 matrix = new LED8x8MatrixMAX7219(new MAX7219(MAX7219.Rotate.D90, MAX7219.ChipSelect.CE1), 5);



            while (true) {
                matrix.FrameClear();

                for (int c = 0; c < matrix.TotalColumns; c = c + 2) {
                    matrix.ColumnDrawLine((ushort)c);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                for (ushort i = 0; i < matrix.Panels; i++) {
                    matrix.DrawLetter(i.ToString()[0], Pixel.Mono.On, i);
                }

                matrix.FrameDraw();
                Task.Delay(1000).Wait();

                for (int r = 0; r < matrix.Rows * 2; r++) {
                    matrix.FrameRowDown();
                    matrix.FrameDraw();
                    Task.Delay(50).Wait();
                }

                for (int r = 0; r < matrix.Rows * 2; r++) {
                    matrix.FrameRowUp();
                    matrix.FrameDraw();
                    Task.Delay(50).Wait();
                }

                for (int c = 0; c < matrix.TotalColumns * 1; c++) {
                    matrix.FrameRollRight();
                    matrix.FrameDraw();
                    Task.Delay(50).Wait();
                }

                for (int c = 0; c < matrix.TotalColumns * 1; c++) {
                    matrix.FrameRollLeft();
                    matrix.FrameDraw();
                    Task.Delay(50).Wait();
                }


                //Task.Delay(1000).Wait();
                //continue;

                matrix.DrawString("Dave and Freddie", 100, 1);
                matrix.FrameClear();

                for (int i = 0; i < matrix.Rows; i++) {
                    matrix.DrawBox(i, i, (int)matrix.TotalColumns - (i * 2), (int)matrix.Rows - (i * 2), Pixel.Mono.On);
                    matrix.FrameDraw();
                    Task.Delay(50).Wait();
                }

                for (byte l = 0; l < 2; l++) {
                    matrix.SetDisplayState(LedDriver.Display.Off);
                    Task.Delay(250).Wait();
                    matrix.SetDisplayState(LedDriver.Display.On);
                    Task.Delay(250).Wait();
                }


                for (int r = 0; r < 2; r++) {
                    for (uint i = 0; i < matrix.Rows; i++) {
                        matrix.RowDrawLine(i, i - 0, matrix.TotalColumns - i - 1);
                        matrix.FrameDraw();
                        Task.Delay(50).Wait();
                    }

                    for (uint i = 0; i < matrix.Rows; i++) {
                        matrix.RowDrawLine(i, i - 0, matrix.TotalColumns - i - 1, Pixel.Mono.Off);
                        matrix.FrameDraw();
                        Task.Delay(50).Wait();
                    }
                }

                for (byte l = 0; l < 8; l++) {
                    matrix.SetBrightness(l);
                    Task.Delay(250).Wait();
                }

                matrix.SetBrightness(1);


                Task.Delay(500).Wait();
                matrix.FrameClear();

                matrix.ScrollStringInFromRight("Hello World ", 50);
                matrix.FrameClear();
                matrix.ScrollStringInFromLeft("Happy Birthday ", 50);


                for (int i = 0; i < matrix.fontSimple.Length; i = (int)(i + matrix.Panels)) {
                    for (int p = 0; p < matrix.Panels; p++) {
                        if (p + i >= matrix.fontSimple.Length) { break; }
                        matrix.DrawBitmap(matrix.fontSimple[p + i], Pixel.Mono.On, (ushort)((p + i) % matrix.Panels));
                    }
                    matrix.FrameDraw();
                    Task.Delay((int)(100 * matrix.Panels)).Wait();
                }

                foreach (Grid8x8.Symbols sym in Enum.GetValues(typeof(Grid8x8.Symbols))) {
                    for (int p = 0; p < matrix.Panels; p++) {
                        matrix.DrawSymbol(sym, Pixel.Mono.On, (ushort)p);
                    }
                    matrix.FrameDraw();
                    Task.Delay((int)(100 * matrix.Panels)).Wait();
                }


                for (uint r = 0; r < matrix.Rows; r++) {
                    for (uint c = 0; c < matrix.TotalColumns; c++) {
                        matrix.FrameSet(Pixel.Mono.On, matrix.PointPostion(r, c));
                        matrix.FrameSet(Pixel.Mono.On, matrix.PointPostion(matrix.Rows - r, matrix.TotalColumns - c));

                        matrix.FrameDraw();
                        Task.Delay(6).Wait();

                        matrix.FrameSet(Pixel.Mono.Off, matrix.PointPostion(r, c));
                        matrix.FrameSet(Pixel.Mono.Off, matrix.PointPostion(matrix.Rows - r, matrix.TotalColumns - c));
                        matrix.FrameDraw();
                        Task.Delay(6).Wait();
                    }
                }


                //for (int j = 0; j < 2; j++) {
                //    for (int i = 0; i < matrix.Length; i++) {
                //        matrix.FrameSet(Pixel.Mono.On, (ushort)i);
                //        matrix.FrameSet(Pixel.Mono.On, (ushort)(matrix.Length - i));
                //        matrix.FrameDraw();
                //        Task.Delay(6).Wait();
                //        matrix.FrameSet(Pixel.Mono.Off, (ushort)i);
                //        matrix.FrameSet(Pixel.Mono.Off, (ushort)(matrix.Length - i));
                //        matrix.FrameDraw();
                //        Task.Delay(6).Wait();
                //    }
                //}
            }
        }
    }
}