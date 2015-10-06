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

            LED8x8MatrixMAX7219 matrix = new LED8x8MatrixMAX7219(new MAX7219(), 2);

            while (true) {


                matrix.ScrollStringInFromRight("Hello World ", 100);

                matrix.DrawLetter('1', Pixel.Mono.On, 0);
                matrix.DrawLetter('2', Pixel.Mono.On, 1);
                matrix.FrameDraw();
                Task.Delay(1000).Wait();

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
