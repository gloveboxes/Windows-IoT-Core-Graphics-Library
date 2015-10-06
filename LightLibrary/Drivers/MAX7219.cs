using Glovebox.Components.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;
using LightLibrary;

namespace LightLibrary.Drivers {
    public class MAX7219 : ILedDriver {

        private const string SPI_CONTROLLER_NAME = "SPI0";  // Use SPI0.
        private const Int32 SPI_CHIP_SELECT_LINE = 0;       // Line 0 is the CS0 pin which is 
                                                            // the physical pin 24 on the Rpi2.
        private const UInt32 DISPLAY_COLUMNS = 8;

        private byte[] SendBytes; // = new byte[4];             // Send to Spi Display without drawing memory.

        // COMMAND MODES for MAX7219. Refer to the table in the datasheet.
        private static readonly byte[] MODE_DECODE = { 0x09, 0x00 }; // , 0x09, 0x00 };
        private static readonly byte[] MODE_INTENSITY = { 0x0A, 0x01 }; // , 0x0A, 0x00 };
        private static readonly byte[] MODE_SCAN_LIMIT = { 0x0B, 0x07 }; // , 0x0B, 0x07 };
        private static readonly byte[] MODE_POWER = { 0x0C, 0x01 }; // , 0x0C, 0x01 };
        private static readonly byte[] MODE_TEST = { 0x0F, 0x00 }; // , 0x0F, 0x00 };
        private static readonly byte[] MODE_NOOP = { 0x00, 0x00 }; // , 0x00, 0x00 };

        private SpiDevice SpiDisplay;                   // SPI device on Raspberry Pi 2

        private int uCtr;     // Counter variables for updating message.

        uint NumberOfPanels = 1;


        public MAX7219() {
            Initialize();        // Initialize SPI and GPIO on the current system.
        }

        /// <summary>
        /// Initialize SPI, GPIO and LED Display
        /// </summary>
        private async void Initialize() {
            await InitSpi();
        }

        /// <summary>
        /// Initialize SPI.
        /// </summary>
        /// <returns></returns>
        private async Task InitSpi() {
            try {
                var settings = new SpiConnectionSettings(SPI_CHIP_SELECT_LINE);
                settings.ClockFrequency = 1000000;
                settings.Mode = SpiMode.Mode0;

                string spiAqs = SpiDevice.GetDeviceSelector(SPI_CONTROLLER_NAME);       /* Find the selector string for the SPI bus controller          */
                var devicesInfo = await DeviceInformation.FindAllAsync(spiAqs);         /* Find the SPI bus controller device with our selector string  */
                SpiDisplay = await SpiDevice.FromIdAsync(devicesInfo[0].Id, settings);  /* Create an SpiDevice with our bus controller and SPI settings */
            }
            /* If initialization fails, display the exception and stop running */
            catch (Exception ex) {
                throw new Exception("SPI Initialization Failed", ex);
            }
        }

        /// <summary>
        /// Initialize LED Display. Refer to the datasheet of MAX7219
        /// </summary>
        /// <returns></returns>
        private void InitDisplay() {
            InitPanel(MODE_SCAN_LIMIT);
            InitPanel(MODE_DECODE);
            InitPanel(MODE_POWER);
            InitPanel(MODE_TEST);
            InitPanel(MODE_INTENSITY);
        }


        private void InitPanel(byte[] control) {
            for (int p = 0; p < NumberOfPanels * 2; p = p + 2) {
                SendBytes[p] = control[0];
                SendBytes[p + 1] = control[1]; ;
            }
            SpiDisplay.Write(SendBytes);
        }

        public void SetBlinkRate(LedDriver.BlinkRate blinkrate) {

        }

        public void SetBrightness(byte level) {

        }

        public void SetDisplayState(LedDriver.Display state) {

        }

        public void SetPanels(ushort panels) {
            this.NumberOfPanels = panels;
            SendBytes = new byte[2 * panels];
            InitDisplay();
        }

        public void Write(ulong frameMap) { }

        public void Write(ulong[] input) {
            ulong[] output = new ulong[input.Length];
            byte row;

            for (uCtr = 0; uCtr < 8; uCtr++) {
                for (int i = 0; i < input.Length; i++) {
                    row = (byte)(input[i] >> 8 * uCtr);
                    RotateAntiClockwise((ushort)uCtr, row, ref output[i]);
                }
            }

            for (uCtr = 0; uCtr < 8; uCtr++) {

                for (int panel = 0; panel < output.Length; panel++) {

                    SendBytes[panel * 2] = (byte)(uCtr + 1); // Address   
                    row = (byte)(output[output.Length - 1 - panel] >> 8 * uCtr);
                    SendBytes[(panel * 2) + 1] = row;

                    SpiDisplay.Write(SendBytes);
                }
            }
        }

        public void Write(Pixel[] frame) {
            ulong[] output = new ulong[NumberOfPanels];
            ulong pixelState = 0;

            for (int panels = 0; panels < NumberOfPanels; panels++) {

                for (int i = panels * 64; i < 64 + (panels * 64); i++) {
                    pixelState = frame[i].State ? 1UL : 0;
                    pixelState = pixelState << i;
                    output[panels] = output[panels] | pixelState;
                }
            }

            Write(output);
        }

        private void RotateAntiClockwise(ushort colIndex, byte value, ref ulong output) {

            colIndex = (ushort)(colIndex % 8);

            //build the new column bit mask
            ulong mask = (byte)(value >> 7 & 1);

            for (int col = 1; col < 8; col++) {
                mask = mask << 8 | (byte)(value >> (7 - col) & 1);
            }

            mask = mask << colIndex;

            // merge in the new column bit mask
            output = output | mask;
        }
    }
}
