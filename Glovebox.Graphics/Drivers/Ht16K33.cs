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

        protected uint NumberOfPanels = 1;
        const uint bufferSize = 17;
        protected byte[] Frame = new byte[bufferSize];
        protected ushort Columns { get; set; }
        protected ushort Rows { get; set; }

        protected I2cDevice i2cDevice;

        private const byte OSCILLATOR_ON = 0x21;
        private const byte OSCILLATOR_OFF = 0x20;

        private const string I2C_CONTROLLER_NAME = "I2C1";        /* For Raspberry Pi 2, use I2C1 */
        private byte I2CAddress = 0x70;


        private byte currentDisplayState;
        private byte[] displayStates = { 0x81, 0x80 }; // on, off

        private byte currentBlinkrate = 0x00;  // off
        private byte[] blinkRates = { 0x00, 0x02, 0x04, 0x06};  //off, 2hz, 1hz, 0.5 hz for off, fast, medium, slow


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
        /// <param name="display">On or Off - defaults to On</param>
        /// <param name="brightness">Between 0 and 15</param>
        /// <param name="blinkrate">Defaults to Off.  Blink rates Fast = 2hz, Medium = 1hz, slow = 0.5hz</param>
        public Ht16K33(byte I2CAddress = 0x70, Rotate rotate = Rotate.None, Display display = Display.On, byte brightness = 2, BlinkRate blinkrate = BlinkRate.Off) {

            Columns = 8;
            Rows = 8;
            this.rotate = rotate;
            this.brightness = brightness;
            this.I2CAddress = I2CAddress;

            currentDisplayState = displayStates[(byte)display];
            currentBlinkrate = blinkRates[(byte)blinkrate];

            Initialize();
        }


        private void Initialize() {
            Task.Run(() => I2cConnect()).Wait();
            //await I2cConnect();
                InitController();
        }

        private async Task I2cConnect() {
            try {
                var settings = new I2cConnectionSettings(I2CAddress);
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
         //   Write(new byte[] { OSCILLATOR_OFF, 0x00 });

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
            this.NumberOfPanels = panels;
        }

        private void UpdateDisplayState() {
            Write(new byte[] { (byte)((byte)currentDisplayState | (byte)this.currentBlinkrate), 0x00 });
        }

        public void Write(ulong frameMap) {
            //DrawBitmap(frameMap);
            //i2cDevice.Write(Frame);
        }

        private void Write(byte[] frame) {

            lock (LockI2C) {
                //Task.Delay(1).Wait();
                //var result = i2cDevice.WritePartial(frame);
                //if (result.Status != I2cTransferStatus.FullTransfer) {
                //    throw new Exception(result.ToString());
                //}
                i2cDevice.Write(frame);
            }
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
                i2cDevice.Write(Frame);
            }
        }

        public virtual void Write(Pixel[] frame) {
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
