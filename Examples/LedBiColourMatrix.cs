using Glovebox.Graphics;
using Glovebox.Graphics.Components;
using Glovebox.Graphics.Drivers;
using Glovebox.Graphics.Grid;
using System;
using System.Threading.Tasks;

namespace LedHost {
    public sealed class LedBiColourMatrix {

        public void Start() {

            Pixel[] palette = new Pixel[] { BiColour.Red, BiColour.Green, BiColour.Yellow };


            var driver = new Ht16K33BiColor(new byte[] { 0x71 }, Ht16K33BiColor.Rotate.D90);
            LED8x8Matrix matrix = new LED8x8Matrix(driver);

            matrix.SetBrightness(6);

            while (true) {

                matrix.FrameClear();
                matrix.ScrollStringInFromRight("Hello World 2015", 60, new Pixel[] { BiColour.Red, BiColour.Green, BiColour.Yellow });

                matrix.FrameClear();
                matrix.ScrollStringInFromLeft("Hello World 2015", 100, palette);

                //continue;

                for (ushort p = 0; p < matrix.PanelsPerFrame; p++) {
                    matrix.DrawSymbol(Grid8x8.Symbols.Block, BiColour.Red, p);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }


                for (int p = 0; p < matrix.Length; p++) {
                    matrix.FrameSet(BiColour.Green, p);
                    matrix.FrameSet(BiColour.Green, matrix.Length - 1 - p);

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
                    matrix.RowDrawLine(r, BiColour.Yellow);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                Task.Delay(1000).Wait();

                for (ushort i = 0; i < matrix.PanelsPerFrame; i++) {
                    matrix.DrawLetter(i.ToString()[0], BiColour.Green, i);
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
                    Task.Delay(50).Wait();
                }

                matrix.DrawString("Wow, such colour :)", palette, 200, 0);
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

                for (int r = 0; r < 4; r++) {
                    for (int i = 0; i < matrix.RowsPerPanel; i++) {
                        matrix.RowDrawLine(i, i - 0, matrix.ColumnsPerFrame - i - 1, palette[i % palette.Length]);
                        matrix.FrameDraw();
                        Task.Delay(50).Wait();
                    }


                    for (int i = 0; i < matrix.RowsPerPanel; i++) {
                        matrix.RowDrawLine(i, i - 0, matrix.ColumnsPerFrame - i - 1, Mono.Off);
                        matrix.FrameDraw();
                        Task.Delay(50).Wait();
                    }
                }

                Task.Delay(250).Wait();
                matrix.FrameClear();


                for (int i = 0; i < matrix.fontSimple.Length; i = i + matrix.PanelsPerFrame) {
                    for (int p = 0; p < matrix.PanelsPerFrame; p++) {
                        if (p + i >= matrix.fontSimple.Length) { break; }
                        matrix.DrawBitmap(matrix.fontSimple[p + i], palette[i % palette.Length], (p + i) % matrix.PanelsPerFrame);
                    }
                    matrix.FrameDraw();
                    Task.Delay(150 * matrix.PanelsPerFrame).Wait();
                }

                foreach (Grid8x8.Symbols sym in Enum.GetValues(typeof(Grid8x8.Symbols))) {
                    for (int p = 0; p < matrix.PanelsPerFrame; p++) {
                        matrix.DrawSymbol(sym, palette[p % palette.Length], p);
                    }
                    matrix.FrameDraw();
                    Task.Delay(150 * matrix.PanelsPerFrame).Wait();
                }
            }
        }
    }
}
