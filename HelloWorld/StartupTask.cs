using Glovebox.Graphics.Components;
using Glovebox.Graphics.Drivers;
using System;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace HelloWorld
{
    public sealed class StartupTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;   // for a headless Windows 10 for IoT projects you need to hold a deferral to keep the app active in the background
        double temperature;
        bool blink = false;
        StringBuilder data = new StringBuilder(20);

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();  // get the deferral handle

            MAX7219 driver = new MAX7219(); 
            SevenSegmentDisplay ssd = new SevenSegmentDisplay(driver);
            Glovebox.IoT.Devices.Sensors.BMP180 bmp = new Glovebox.IoT.Devices.Sensors.BMP180(Glovebox.IoT.Devices.Sensors.BMP180.Mode.HIGHRES);


            ssd.FrameClear();
            ssd.FrameDraw();
            ssd.SetBrightness(2);


            while (true)
            {
                temperature = bmp.Temperature.DegreesCelsius;

                data.Clear();

                if (temperature < 100) { data.Append($"{Math.Round(temperature, 1)}C".PadRight(5)); }
                else { data.Append($"{Math.Round(temperature, 0)}C".PadRight(4)); }

                data.Append(Math.Round(bmp.Pressure.Hectopascals, 0));

                if (blink = !blink) { data.Append("."); }


                ssd.DrawString(data.ToString());
                ssd.FrameDraw();

                Task.Delay(2000).Wait();             
            }
        }
    }
}