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

        public async void Start() {

            var driver = new MAX7219(1, MAX7219.Rotate.D90, MAX7219.Transform.None, MAX7219.ChipSelect.CE0);

            LED8x8Matrix matrix = new LED8x8Matrix(driver);

            matrix.SetBrightness(2);

            while (true) {

                matrix.FrameClear();
                matrix.ScrollStringInFromRight("Hello World 2015", 100);

                matrix.FrameClear();
                matrix.ScrollStringInFromLeft("Hello World 2015", 100);

                //continue;

                for (ushort p = 0; p < matrix.PanelsPerFrame; p++) {
                    matrix.DrawSymbol(Grid8x8.Symbols.Block, Led.On, p);
                    matrix.FrameDraw();
                    await Task.Delay(100);
                }


                for (int p = 0; p < matrix.Length; p++) {
                    matrix.FrameSet(Led.On, p);
                    matrix.FrameSet(Led.On, matrix.Length - 1 - p);

                    matrix.FrameDraw();
                    await Task.Delay(2);

                    matrix.FrameSet(Led.Off, p);
                    matrix.FrameSet(Led.Off, matrix.Length - 1 - p);

                    matrix.FrameDraw();
                    await Task.Delay(2);
                }


                for (int c = 0; c < matrix.ColumnsPerFrame; c = c + 2) {
                    matrix.ColumnDrawLine(c);
                    matrix.FrameDraw();
                    await Task.Delay(100);
                }


                for (int r = 0; r < matrix.RowsPerPanel; r = r + 2) {
                    matrix.RowDrawLine(r);
                    matrix.FrameDraw();
                    await Task.Delay(100);
                }

                await Task.Delay(1000);

                for (ushort i = 0; i < matrix.PanelsPerFrame; i++) {
                    matrix.DrawLetter(i.ToString()[0], Led.On, i);
                }

                matrix.FrameDraw();
                await Task.Delay(1000);

                for (int r = 0; r < matrix.RowsPerPanel * 2; r++) {
                    matrix.FrameRollDown();
                    matrix.FrameDraw();
                    await Task.Delay(100);
                }

                for (int r = 0; r < matrix.RowsPerPanel * 2; r++) {
                    matrix.FrameRollUp();
                    matrix.FrameDraw();
                    await Task.Delay(100);
                }

                for (int c = 0; c < matrix.ColumnsPerFrame * 1; c++) {
                    matrix.FrameRollRight();
                    matrix.FrameDraw();
                    await Task.Delay(100);
                }

                for (int c = 0; c < matrix.ColumnsPerFrame * 1; c++) {
                    matrix.FrameRollLeft();
                    matrix.FrameDraw();
                    await Task.Delay(100);
                }


                //await Task.Delay(1000);
                //continue;

                matrix.DrawString("ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890", 100, 0);
                matrix.FrameClear();

                for (int i = 0; i < matrix.RowsPerPanel; i++) {
                    matrix.DrawBox(i, i, matrix.ColumnsPerFrame - (i * 2), matrix.RowsPerPanel - (i * 2), Led.On);
                    matrix.FrameDraw();
                    await Task.Delay(100);
                }

                for (byte l = 0; l < 2; l++) {
                    matrix.SetFrameState(LedDriver.Display.Off);
                    await Task.Delay(250);
                    matrix.SetFrameState(LedDriver.Display.On);
                    await Task.Delay(250);
                }



                matrix.FrameClear();

                for (int r = 0; r < 2; r++) {
                    for (int i = 0; i < matrix.RowsPerPanel; i++) {
                        matrix.RowDrawLine(i, i - 0, matrix.ColumnsPerFrame - i - 1);
                        matrix.FrameDraw();
                        await Task.Delay(50);
                    }

                    //for (byte l = 0; l < 6; l++) {
                    //    matrix.SetBrightness(l);
                    //    await Task.Delay(250);
                    //}

                    //matrix.SetBrightness(1);

                    for (int i = 0; i < matrix.RowsPerPanel; i++) {
                        matrix.RowDrawLine(i, i - 0, matrix.ColumnsPerFrame - i - 1, Led.Off);
                        matrix.FrameDraw();
                        await Task.Delay(50);
                    }
                }

                await Task.Delay(500);
                matrix.FrameClear();

                matrix.ScrollStringInFromRight("Hello World ", 100);
                matrix.FrameClear();
                matrix.ScrollStringInFromLeft("Happy Birthday ", 100);


                for (int i = 0; i < matrix.fontSimple.Length; i = i + matrix.PanelsPerFrame) {
                    for (int p = 0; p < matrix.PanelsPerFrame; p++) {
                        if (p + i >= matrix.fontSimple.Length) { break; }
                        matrix.DrawBitmap(matrix.fontSimple[p + i], Led.On, (p + i) % matrix.PanelsPerFrame);
                    }
                    matrix.FrameDraw();
                    await Task.Delay(100 * matrix.PanelsPerFrame);
                }

                foreach (Grid8x8.Symbols sym in Enum.GetValues(typeof(Grid8x8.Symbols))) {
                    for (int p = 0; p < matrix.PanelsPerFrame; p++) {
                        matrix.DrawSymbol(sym, Led.On, p);
                    }
                    matrix.FrameDraw();
                    await Task.Delay(100 * matrix.PanelsPerFrame);
                }
            }
        }
    }
}
