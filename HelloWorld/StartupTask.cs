using Glovebox.Graphics.Components;
using Glovebox.Graphics.Drivers;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace HelloWorld
{
    public sealed class StartupTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;   // for a headless Windows 10 for IoT projects you need to hold a deferral to keep the app active in the background

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();  // get the deferral handle

            MAX7219 driver = new MAX7219(4, MAX7219.Rotate.None, MAX7219.Transform.HorizontalFlip, MAX7219.ChipSelect.CE0);  // 4 panels, rotate 90 degrees, SPI CE0
            LED8x8Matrix matrix = new LED8x8Matrix(driver);     // pass the driver to the LED8x8Matrix Graphics Library

            while (true) {
                matrix.ScrollStringInFromRight("Hello World 2015 ", 100);
            }
        }
    }
}