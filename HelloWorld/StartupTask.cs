using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Glovebox.Graphics.Drivers;
using Glovebox.Graphics.Components;
using Glovebox.Graphics.Grid;
using Glovebox.Graphics;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace HelloWorld
{
    public sealed class StartupTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;   // for a headless you need to hold a deferral to keep the app active in the background

        public void Run(IBackgroundTaskInstance taskInstance) {
            _deferral = taskInstance.GetDeferral();

            MAX7219 driver = new MAX7219(4, MAX7219.Rotate.D90, MAX7219.ChipSelect.CE0);  // 4 panels, rotate 90 degrees, SPI CE0
            LED8x8Matrix matrix = new LED8x8Matrix(driver);     // pass the driver to the LED8x8Matrix Graphics Library

            while (true) {
                matrix.ScrollStringInFromRight("Hello World 2015", 100);

                matrix.DrawSymbol(Grid8x8.Symbols.Heart, Mono.On, 0);
                matrix.DrawSymbol(new Grid8x8.Symbols[] { Grid8x8.Symbols.Heart, Grid8x8.Symbols.HourGlass }, Mono.On, 100);
            }
        }

    }
}
