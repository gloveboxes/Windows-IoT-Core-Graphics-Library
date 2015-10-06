using LightLibrary;
using LightLibrary.Components;
using LightLibrary.Drivers;
using LightLibrary.Grid;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace LedHost {
    public sealed class StartupTask : IBackgroundTask {

        BackgroundTaskDeferral _deferral;

        public void Run(IBackgroundTaskInstance taskInstance) {
            _deferral = taskInstance.GetDeferral();

            LED8x8MatrixMAX7219 matrix = new LED8x8MatrixMAX7219(new MAX7219(MAX7219.Rotate.D90), 2);



            while (true) {
                matrix.FrameClear();

                for (int c = 0; c < matrix.TotalColumns; c = c + 2) {
                    matrix.ColumnDrawLine((ushort)c);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                matrix.DrawSymbol(Grid8x8.Symbols.Heart, Pixel.Mono.On, 0);
                matrix.DrawLetter('2', Pixel.Mono.On, 1);
                matrix.FrameDraw();
                Task.Delay(1000).Wait();

                for (int r = 0; r < matrix.Rows * 4; r++) {
                    matrix.FrameRowDown();
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                for (int r = 0; r < matrix.Rows * 4; r++) {
                    matrix.FrameRowUp();
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                for (int c = 0; c < matrix.TotalColumns * 2; c++) {
                    matrix.FrameRollRight();
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                for (int c = 0; c < matrix.TotalColumns * 2; c++) {
                    matrix.FrameRollLeft();
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
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

                matrix.SetBrightness(2);


                Task.Delay(500).Wait();
                matrix.FrameClear();

                matrix.ScrollStringInFromRight("Hello World ", 100);
                matrix.FrameClear();
                matrix.ScrollStringInFromLeft("Happy Birthday ", 100);

                for (int i = 0; i < matrix.fontSimple.Length / 2 * 2; i = i + 2) {
                    matrix.DrawBitmap(matrix.fontSimple[i], Pixel.Mono.On, 0);
                    matrix.DrawBitmap(matrix.fontSimple[i + 1], Pixel.Mono.On, 1);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                foreach (Grid8x8.Symbols sym in Enum.GetValues(typeof(Grid8x8.Symbols))) {
                    matrix.DrawSymbol(sym, Pixel.Mono.On, 0);
                    matrix.DrawSymbol(sym, Pixel.Mono.On, 1);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }
            }
        }
    }
}
