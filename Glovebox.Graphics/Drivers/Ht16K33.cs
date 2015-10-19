using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace Glovebox.Graphics.Drivers {

    /// <summary>
    /// Represents a I2C connection to a PCF8574 I/O Expander.
    /// </summary>
    /// <remarks>See <see cref="http://www.adafruit.com/datasheets/ht16K33v110.pdf"/> for more information.</remarks>
    public class Ht16K33 : LedDriver, IDisposable, ILedDriver {
        #region Fields

        public int PanelsPerFrame { get; private set; }
        const uint bufferSize = 17;
        protected byte[] Frame = new byte[bufferSize];
        protected ushort Columns { get; set; }
        protected ushort Rows { get; set; }

        protected I2cDevice[] i2cDevice;

        private const byte OSCILLATOR_ON = 0x21;
        private const byte OSCILLATOR_OFF = 0x20;

        private string I2cControllerName = "I2C1";        /* For Raspberry Pi 2, use I2C1 */
        private byte[] I2CAddress; // = 0x70;


        private byte currentFrameState;
        private byte[] frameStates = { 0x81, 0x80 }; // on, off

        private byte currentBlinkrate = 0x00;  // off
        private byte[] blinkRates = { 0x00, 0x02, 0x04, 0x06 };  //off, 2hz, 1hz, 0.5 hz for off, fast, medium, slow


        private byte brightness;

        public enum Rotate {
            None = 0,
            D90 = 1,
            D180 = 2,
        }
        protected Rotate rotate = Rotate.None;


        #endregion

        /// <summary>
        /// Initializes a new instance of the Ht16K33 I2C controller as found on the Adafriut Mini LED Matrix.
        /// </summary>
        /// <param name="frame">On or Off - defaults to On</param>
        /// <param name="brightness">Between 0 and 15</param>
        /// <param name="blinkrate">Defaults to Off.  Blink rates Fast = 2hz, Medium = 1hz, slow = 0.5hz</param>
        public Ht16K33(byte[] I2CAddress = null, Rotate rotate = Rotate.None, Display frame = LedDriver.Display.On, byte brightness = 2, BlinkRate blinkrate = BlinkRate.Off, string I2cControllerName = "I2C1") {

            Columns = 8;
            Rows = 8;
            this.rotate = rotate;
            this.brightness = brightness;
            this.I2cControllerName = I2cControllerName;

            if (I2CAddress == null) {
                this.I2CAddress = new byte[] { 0x70 };
            }
            else {
                this.I2CAddress = I2CAddress;
            }

            this.PanelsPerFrame = this.I2CAddress.Length;
            this.i2cDevice = new I2cDevice[PanelsPerFrame];

            currentFrameState = frameStates[(byte)frame];
            currentBlinkrate = blinkRates[(byte)blinkrate];

            Initialize();
        }

        private void Initialize() {
            for (int panel = 0; panel < PanelsPerFrame; panel++) {
                Task.Run(() => I2cConnect(panel)).Wait();
            }
            InitPanels();
        }

        private async Task I2cConnect(int panel) {
            try {
                var settings = new I2cConnectionSettings(I2CAddress[panel]);
                settings.BusSpeed = I2cBusSpeed.FastMode;

                string aqs = I2cDevice.GetDeviceSelector(I2cControllerName);  /* Find the selector string for the I2C bus controller                   */
                var dis = await DeviceInformation.FindAllAsync(aqs);            /* Find the I2C bus controller device with our selector string           */
                i2cDevice[panel] = await I2cDevice.FromIdAsync(dis[0].Id, settings);    /* Create an I2cDevice with our selected bus controller and I2C settings */
            }
            catch (Exception e) {
                throw new Exception("ht16k33 initisation problem: " + e.Message);
            }
        }

        private void InitPanels() {
            WriteAll(new byte[] { OSCILLATOR_ON, 0x00 });
            Write(0); // clear the screen
            UpdateFrameState();
            SetBrightness(brightness);
        }

        public void SetBrightness(byte level) {
            if (level > 15) { level = 15; }
            WriteAll(new byte[] { (byte)(0xE0 | level), 0x00 });
        }

        public void SetBlinkRate(BlinkRate blinkrate) {
            currentBlinkrate = blinkRates[(byte)blinkrate];
            UpdateFrameState();
        }

        public void SetFrameState(Display state) {
            currentFrameState = frameStates[(byte)state];
            UpdateFrameState();
        }

        public int GetNumberOfPanels() {
            return (int)PanelsPerFrame;
        }

        private void UpdateFrameState() {
            WriteAll(new byte[] { (byte)((byte)currentFrameState | (byte)this.currentBlinkrate), 0x00 });
        }

        private void WriteAll(byte[] data) {
            for (int panel = 0; panel < PanelsPerFrame; panel++) {
                i2cDevice[panel].Write(data);
            }
        }

        // required for Interface but implementation is overridden below
        public void Write(ulong frameMap) {
        }

        public void Write(ulong[] input) {
            // perform any required display rotations
            for (int rotations = 0; rotations < (int)rotate; rotations++) {
                for (int panel = 0; panel < input.Length; panel++) {
                    input[panel] = RotateAntiClockwise(input[panel]);
                }
            }

            for (int p = 0; p < input.Length; p++) {
                DrawBitmap(input[p]);
                i2cDevice[p].Write(Frame);
            }
        }

        public virtual void Write(Pixel[] frame) {
            ulong[] output = new ulong[PanelsPerFrame];
            ulong pixelState = 0;

            for (int panels = 0; panels < PanelsPerFrame; panels++) {

                for (int i = panels * 64; i < 64 + (panels * 64); i++) {
                    pixelState = frame[i].ColourValue > 0 ? 1UL : 0;
                    pixelState = pixelState << i;
                    output[panels] = output[panels] | pixelState;
                }
            }

            Write(output);
        }

        void IDisposable.Dispose() {
            for (int panel = 0; panel < PanelsPerFrame; panel++) {
                i2cDevice[panel].Dispose();
            }
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

        protected ulong RotateAntiClockwise(ulong input) {
            ulong output = 0;
            byte row;

            for (int byteNumber = 0; byteNumber < 8; byteNumber++) {

                row = (byte)(input >> 8 * byteNumber);

                ulong mask = 0;   //build the new column bit mask                
                int bit = 0;    // bit pointer/counter

                do {
                    mask = mask << 8 | (byte)(row >> (bit++) & 1);
                } while (bit < 8);

                mask = mask << byteNumber;

                // merge in the new column bit mask
                output = output | mask;
            }
            return output;
        }
    }
}
