using System;

namespace Glovebox.Graphics.Drivers {

    /// <summary>
    /// Represents a I2C connection to a PCF8574 I/O Expander.
    /// </summary>
    /// <remarks>See <see cref="http://www.adafruit.com/datasheets/ht16K33v110.pdf"/> for more information.</remarks>
    public class Ht16K33BiColor : Ht16K33, IDisposable, ILedDriver {

        /// <summary>
        /// Initializes a new instance of the Ht16K33 I2C controller as found on the Adafriut Mini LED Matrix.
        /// </summary>
        /// <param name="frame">On or Off - defaults to On</param>
        /// <param name="brightness">Between 0 and 15</param>
        /// <param name="blinkrate">Defaults to Off.  Blink rates Fast = 2hz, Medium = 1hz, slow = 0.5hz</param>
        public Ht16K33BiColor(byte[] I2CAddress, Rotate rotate = Rotate.None, Display frame = LedDriver.Display.On, byte brightness = 0, BlinkRate blinkrate = BlinkRate.Off, string I2cControllerName="I2C1")
            : base(I2CAddress, rotate, frame, brightness, blinkrate, I2cControllerName) { }
       

        public void Write(ulong[] inputGreen, ulong[] inputRed) {

            // perform any required display rotations
            for (int rotations = 0; rotations < (int)rotate; rotations++) {
                for (int panel = 0; panel < inputGreen.Length; panel++) {
                    inputGreen[panel] = RotateAntiClockwise(inputGreen[panel]);
                    inputRed[panel] = RotateAntiClockwise(inputRed[panel]);
                }
            }


            for (int p = 0; p < inputGreen.Length; p++) {
                DrawBitmap(inputGreen[p], inputRed[p]);
                i2cDevice[p].Write(Frame);
            }
        }

        public override void Write(Pixel[] frame) {
            ulong[] outputGreen = new ulong[PanelsPerFrame];
            ulong[] outputRed = new ulong[PanelsPerFrame];
            ulong pixelStateGreen = 0;
            ulong pixelStateRed = 0;

            for (int panels = 0; panels < PanelsPerFrame; panels++) {

                for (int i = panels * 64; i < 64 + (panels * 64); i++) {

                    switch (frame[i].ColourValue) {
                        case 65280:  // green
                            pixelStateGreen = 1UL;
                            pixelStateGreen = pixelStateGreen << i;
                            outputGreen[panels] = outputGreen[panels] | pixelStateGreen;
                            break;
                        case 16711680: // red
                            pixelStateRed = 1UL;
                            pixelStateRed = pixelStateRed << i;
                            outputRed[panels] = outputRed[panels] | pixelStateRed;
                            break;
                        case 16776960: //yellow
                            pixelStateGreen = 1UL;
                            pixelStateGreen = pixelStateGreen << i;
                            outputGreen[panels] = outputGreen[panels] | pixelStateGreen;

                            pixelStateRed = 1UL;
                            pixelStateRed = pixelStateRed << i;
                            outputRed[panels] = outputRed[panels] | pixelStateRed;

                            break;
                    }              
                }
            }

            Write(outputGreen, outputRed);
        }

        private void DrawBitmap(ulong bitmapGreen, ulong bitmapRed) {
            for (ushort row = 0; row < Rows; row++) {
                Frame[row * 2 + 1] = 0x00;
                Frame[row * 2 + 1] = (byte)(bitmapGreen >> (row * Columns));
                Frame[(row * 2 + 1) + 1] = 0x00;
                Frame[(row * 2 + 1) + 1] = (byte)(bitmapRed >> (row * Columns)); ;

            }
        }
    }
}
