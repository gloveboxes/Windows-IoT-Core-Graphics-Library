using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using LightLibrary.Components;
using LightLibrary.Drivers;
using System.Threading.Tasks;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace LedHost {
    public sealed class StartupTask : IBackgroundTask {

        BackgroundTaskDeferral _deferral;

        public void Run(IBackgroundTaskInstance taskInstance) {
            _deferral = taskInstance.GetDeferral();
            LED8x8MatrixMAX7219 matrix = new LightLibrary.Components.LED8x8MatrixMAX7219(new MAX7219(), 2);

            while (true) {


                matrix.ScrollStringInFromRight("Hello World ", 100);

                matrix.DrawLetter('1', LightLibrary.Pixel.Colour.Red, 0);
                matrix.DrawLetter('2', LightLibrary.Pixel.Colour.Red, 1);
                matrix.FrameDraw();
                Task.Delay(1000).Wait();

                for (int i = 0; i < matrix.fontSimple.Length / 2 * 2; i = i + 2) {
                    matrix.DrawBitmap(matrix.fontSimple[i], LightLibrary.Pixel.Colour.Red, 0);
                    matrix.DrawBitmap(matrix.fontSimple[i + 1], LightLibrary.Pixel.Colour.Red, 1);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }

                foreach (LightLibrary.Grid.Grid8x8.Symbols sym in Enum.GetValues(typeof(LightLibrary.Grid.Grid8x8.Symbols))) {
                    matrix.DrawSymbol(sym, LightLibrary.Pixel.Colour.Red, 0);
                    matrix.DrawSymbol(sym, LightLibrary.Pixel.Colour.Red, 1);
                    matrix.FrameDraw();
                    Task.Delay(100).Wait();
                }
            }
        }
    }
}
