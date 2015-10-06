using System;
using System.Threading.Tasks;
using LightLibrary;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;
using LightLibrary.Drivers;

namespace Glovebox.Components.Drivers {

    /// <summary>
    /// Represents a I2C connection to a PCF8574 I/O Expander.
    /// </summary>
    /// <remarks>See <see cref="http://www.adafruit.com/datasheets/ht16K33v110.pdf"/> for more information.</remarks>
    public class Ht16K33 : LedDriver, IDisposable, ILedDriver {
        #region Fields

        const uint bufferSize = 17;
        private byte[] Frame = new byte[bufferSize];
        private ushort Columns { get; set; }
        private ushort Rows { get; set; }

        private I2cDevice i2cDevice;

        private const byte OSCILLATOR_ON = 0x21;
        private const byte OSCILLATOR_OFF = 0x20;

        private const string I2C_CONTROLLER_NAME = "I2C1";        /* For Raspberry Pi 2, use I2C1 */
        private byte I2C_ADDR = 0x70;


        private byte currentDisplayState;
        private byte[] displayStates = { 0x81, 0x80 }; // on, off

        private byte currentBlinkrate = 0x00;  // off
        private byte[] blinkRates = { 0x00, 0x02, 0x04, 0x06};  //off, 2hz, 1hz, 0.5 hz for off, fast, medium, slow


        private byte brightness;
        ushort panels;


        #endregion

        /// <summary>
        /// Initializes a new instance of the Ht16K33 I2C controller as found on the Adafriut Mini LED Matrix.
        /// </summary>
        /// <param name="display">On or Off - defaults to On</param>
        /// <param name="brightness">Between 0 and 15</param>
        /// <param name="blinkrate">Defaults to Off.  Blink rates Fast = 2hz, Medium = 1hz, slow = 0.5hz</param>
        public Ht16K33(byte I2CAddress = 0x70, Display display = Display.On, byte brightness = 2, BlinkRate blinkrate = BlinkRate.Off) {

            Columns = 8;
            Rows = 8;

            I2C_ADDR = I2CAddress;
            currentDisplayState = displayStates[(byte)display];
            this.brightness = brightness;
            currentBlinkrate = blinkRates[(byte)blinkrate];

            Task.Run(() => I2cConnect()).Wait();

            InitController();
        }

        private async Task I2cConnect() {
            try {
                var settings = new I2cConnectionSettings(I2C_ADDR);
                settings.BusSpeed = I2cBusSpeed.FastMode;

                string aqs = I2cDevice.GetDeviceSelector(I2C_CONTROLLER_NAME);  /* Find the selector string for the I2C bus controller                   */
                var dis = await DeviceInformation.FindAllAsync(aqs);            /* Find the I2C bus controller device with our selector string           */
                i2cDevice = await I2cDevice.FromIdAsync(dis[0].Id, settings);    /* Create an I2cDevice with our selected bus controller and I2C settings */
            }
            catch (Exception e) {
                throw new Exception("ht16k33 initisation problem: " + e.Message);
            }
        }

        private void InitController() {
            Write(new byte[] { OSCILLATOR_ON, 0x00 });
            Write(0); // clear the screen
            UpdateDisplayState();
            SetBrightness(brightness);
        }

        public void SetBrightness(byte level) {
            if (level > 15) { level = 15; }
            Write(new byte[] { (byte)(0xE0 | level), 0x00 });
        }

        public void SetBlinkRate(BlinkRate blinkrate) {
            currentBlinkrate = blinkRates[(byte)blinkrate];
            UpdateDisplayState();
        }

        public void SetDisplayState(Display state) {
            currentDisplayState = displayStates[(byte)state];
            UpdateDisplayState();
        }

        public void SetPanels(ushort panels) {
            this.panels = panels;
        }

        private void UpdateDisplayState() {
            Write(new byte[] { (byte)((byte)currentDisplayState | (byte)this.currentBlinkrate), 0x00 });
        }

        public void Write(ulong frameMap) {
            DrawBitmap(frameMap);
            i2cDevice.Write(Frame);
        }

        private void Write(byte[] frame) {
            i2cDevice.Write(frame);
        }

        public void Write(Pixel[] frame) {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose() {
            i2cDevice.Dispose();
        }

        private void DrawBitmap(ulong bitmap) {
            for (ushort row = 0; row < Rows; row++) {
                Frame[row * 2 + 1] = FixBitOrder((byte)(bitmap >> (row * Columns)));
            }
        }

        // Fix bit order problem with the ht16K33 controller or Adafruit 8x8 matrix
        // Bits offset by 1, roll bits forward by 1, replace 8th bit with the 1st 
        private byte FixBitOrder(byte b) {
            return (byte)(b >> 1 | (b << 7));
        }
    }
}
