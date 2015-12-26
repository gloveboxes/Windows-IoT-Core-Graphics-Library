using Glovebox.Graphics;
using Glovebox.Graphics.Components;
using Glovebox.Graphics.Drivers;
using Glovebox.Graphics.SevenSegmentDisplay;
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

            //MAX7219 driver = new MAX7219(4, MAX7219.Rotate.None, MAX7219.Transform.HorizontalFlip, MAX7219.ChipSelect.CE0);  // 4 panels, rotate 90 degrees, SPI CE0
            //LED8x8Matrix matrix = new LED8x8Matrix(driver);     // pass the driver to the LED8x8Matrix Graphics Library

            //while (true) {
            //    matrix.ScrollStringInFromRight("Hello World 2015 ", 100);
            //}


            MAX7219 driver = new MAX7219(1);  // 4 panels, rotate 90 degrees, SPI CE0
        //    LED8x8Matrix matrix = new LED8x8Matrix(driver);     // pass the driver to the LED8x8Matrix Graphics Library
            SevenSegmentDisplay ssd = new SevenSegmentDisplay(driver);

            ssd.FrameClear();
            ssd.FrameDraw();
            ssd.SetBrightness(6);

            while (true)
            {
                for (int i = 0; i < 100000000; i++)
                {
                    ssd.DrawNumber(i);
                    ssd.FrameDraw();
                    Task.Delay(50).Wait();
                }
            }



            //matrix.DrawBitmap(1, Glovebox.Graphics.Led.On);
            //matrix.FrameDraw();


            //matrix.DrawBitmap(2, Glovebox.Graphics.Led.On);
            //matrix.FrameDraw();


            //matrix.DrawBitmap(4, Glovebox.Graphics.Led.On);
            //matrix.FrameDraw();

            //matrix.DrawBitmap(8, Glovebox.Graphics.Led.On);
            //matrix.FrameDraw();

            //matrix.DrawBitmap(16, Glovebox.Graphics.Led.On);
            //matrix.FrameDraw();

            //matrix.DrawBitmap(32, Glovebox.Graphics.Led.On);
            //matrix.FrameDraw();

            //matrix.DrawBitmap(64, Glovebox.Graphics.Led.On);
            //matrix.FrameDraw();

            //matrix.DrawBitmap(128, Glovebox.Graphics.Led.On);
            //matrix.FrameDraw();

            //matrix.DrawBitmap(256, Glovebox.Graphics.Led.On);
            //matrix.FrameDraw();


            //matrix.DrawBitmap(255, Glovebox.Graphics.Led.On);
            //matrix.FrameDraw();

            //matrix.SetBrightness(1);
            //matrix.SetBrightness(4);
            //matrix.SetBrightness(10);

            //while (true)
            //{
            //    matrix.ScrollStringInFromRight("Hello World 2015 ", 100);
            //}

        }
    }
}
