using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;

namespace Glovebox.Graphics.Drivers
{
    public class MAX7219 : ILedDriver, IDisposable
    {

        private string SPIControllerName = "SPI0";  // Use SPI0 for RPi2

        private byte[] SendDataBytes;

        // http://datasheets.maximintegrated.com/en/ds/MAX7219-MAX7221.pdf

        private static readonly byte[] MODE_DECODE = { 0x09, 0x00 };
        private static byte[] MODE_INTENSITY = { 0x0A, 0x02 };
        private static readonly byte[] MODE_SCAN_LIMIT = { 0x0B, 0x07 };
        private static readonly byte[] MODE_POWER_ON = { 0x0C, 0x01 };
        private static readonly byte[] MODE_POWER_OFF = { 0x0C, 0x00 };
        private static readonly byte[] MODE_TEST = { 0x0F, 0x00 };
        private static readonly byte[] MODE_NOOP = { 0x00, 0x00 };

        private SpiDevice SpiDisplay;

        public int PanelsPerFrame { get; private set; }

        public enum Rotate
        {
            None = 0,
            D90 = 1,
            D180 = 2,
        }

        public enum Transform
        {
            None,
            HorizontalFlip,
        }

        public enum ChipSelect
        {
            CE0 = 0, CE1 = 1
        }

        private ChipSelect chipSelect = ChipSelect.CE0;
        private Rotate rotate = Rotate.None;
        private Transform transform = Transform.None;


        public MAX7219(int numberOfPanels = 1, Rotate rotate = Rotate.None, Transform transform = Transform.None, ChipSelect chipSelect = ChipSelect.CE0, string SPIControllerName = "SPI0")
        {
            this.PanelsPerFrame = numberOfPanels < 0 ? 0 : numberOfPanels;
            this.rotate = rotate;
            this.transform = transform;
            this.chipSelect = chipSelect;
            this.SPIControllerName = SPIControllerName;

            SendDataBytes = new byte[2 * PanelsPerFrame];

            Task.Run(() => InitSpi()).Wait();
            InitDisplay();
        }

        /// <summary>
        /// Initialize SPI.
        /// </summary>
        /// <returns></returns>
        private async Task InitSpi()
        {
            try
            {
                var settings = new SpiConnectionSettings((int)chipSelect);
                settings.ClockFrequency = 10000000;
                settings.Mode = SpiMode.Mode0;
                settings.SharingMode = SpiSharingMode.Shared;

                string spiAqs = SpiDevice.GetDeviceSelector(SPIControllerName);       /* Find the selector string for the SPI bus controller          */
                var devicesInfo = await DeviceInformation.FindAllAsync(spiAqs);         /* Find the SPI bus controller device with our selector string  */
                SpiDisplay = await SpiDevice.FromIdAsync(devicesInfo[0].Id, settings);  /* Create an SpiDevice with our bus controller and SPI settings */
            }
            /* If initialization fails, display the exception and stop running */
            catch (Exception ex)
            {
                throw new Exception("SPI Initialization Failed", ex);
            }
        }

        /// <summary>
        /// Initialize LED Display. Refer to the datasheet of MAX7219
        /// </summary>
        /// <returns></returns>
        private void InitDisplay()
        {
            InitPanel(MODE_SCAN_LIMIT);
            InitPanel(MODE_DECODE);
            InitPanel(MODE_POWER_ON);
            InitPanel(MODE_TEST);
            InitPanel(MODE_INTENSITY);
        }

        private void InitPanel(byte[] control)
        {
            for (int p = 0; p < PanelsPerFrame * 2; p = p + 2)
            {
                SendDataBytes[p] = control[0];
                SendDataBytes[p + 1] = control[1]; ;
            }
            SpiDisplay.Write(SendDataBytes);
        }

        public void SetBlinkRate(LedDriver.BlinkRate blinkrate)
        {

        }

        public void SetBrightness(byte level)
        {
            if (level >= 0 && level < 16)
            {
                MODE_INTENSITY[1] = level;
                InitPanel(MODE_INTENSITY);
            }
        }

        public void SetFrameState(LedDriver.Display state)
        {
            if (state == LedDriver.Display.On) { InitPanel(MODE_POWER_ON); }
            else { InitPanel(MODE_POWER_OFF); }
        }

        // required for Interface but implementation is overridden below
        public void Write(ulong frameMap) { }

        public void Write(ulong[] input)
        {
            byte row;


            // perform any required display rotations
            for (int rotations = 0; rotations < (int)rotate; rotations++)
            {
                for (int panel = 0; panel < input.Length; panel++)
                {
                    input[panel] = RotateAntiClockwise(input[panel]);

                    if (transform == Transform.HorizontalFlip)
                    {
                        input[panel] = HorizontalFlip(input[panel]);
                    }
                }
            }

            for (int panel = 0; panel < input.Length; panel++)
            {
                if (transform == Transform.HorizontalFlip)
                {
                    input[panel] = HorizontalFlip(input[panel]);
                }
            }


            for (int rowNumber = 0; rowNumber < 8; rowNumber++)
            {
                for (int panel = 0; panel < input.Length; panel++)
                {

                    SendDataBytes[panel * 2] = (byte)(rowNumber + 1); // Address   
                    row = (byte)(input[input.Length - 1 - panel] >> 8 * rowNumber);
                    SendDataBytes[(panel * 2) + 1] = row;

                    SpiDisplay.Write(SendDataBytes);
                }
            }
        }

        public void Write(Pixel[] frame)
        {
            ulong[] output = new ulong[PanelsPerFrame];
            ulong pixelState = 0;

            for (int panels = 0; panels < PanelsPerFrame; panels++)
            {

                for (int i = panels * 64; i < 64 + (panels * 64); i++)
                {
                    pixelState = frame[i].ColourValue > 0 ? 1UL : 0;
                    pixelState = pixelState << i;
                    output[panels] = output[panels] | pixelState;
                }
            }

            Write(output);
        }

        private ulong RotateAntiClockwise(ulong input)
        {
            ulong output = 0;
            byte row;

            for (int byteNumber = 0; byteNumber < 8; byteNumber++)
            {

                row = (byte)(input >> 8 * byteNumber);

                ulong mask = 0;   //build the new column bit mask                
                int bit = 0;    // bit pointer/counter

                do
                {
                    mask = mask << 8 | (byte)(row >> (7 - bit++) & 1);
                } while (bit < 8);

                mask = mask << byteNumber;

                output = output | mask; // merge in the new column bit mask
            }
            return output;
        }


        private ulong HorizontalFlip(ulong input)
        {
            ulong output = 0;
            ulong newRow;

            byte row = 0;

            for (int byteNumber = 0; byteNumber < 8; byteNumber++)
            {
                row = (byte)(input >> 8 * byteNumber);
                newRow = (ulong)((ulong)row << ((7 - byteNumber) * 8));
                output |= newRow;


            }
            return output;
        }

        public void Dispose()
        {
            SpiDisplay.Dispose();
        }
    }
}
