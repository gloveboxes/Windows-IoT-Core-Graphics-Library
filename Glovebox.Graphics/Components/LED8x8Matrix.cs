using Glovebox.Graphics.Drivers;
using Glovebox.Graphics.Grid;
using System;

namespace Glovebox.Graphics.Components {
    public class LED8x8Matrix : Grid8x8, ILedDriver {

        ILedDriver driver;

        public LED8x8Matrix(ILedDriver driver) : base("matrix", driver.PanelsPerFrame) {
            this.driver = driver;
        }

        public int PanelsPerFrame {
            get {
                return driver.PanelsPerFrame;
            }
        }

        public void SetBlinkRate(LedDriver.BlinkRate blinkrate) {
            driver.SetBlinkRate(blinkrate);
        }

        public void SetBrightness(byte level) {
            driver.SetBrightness(level);
        }

        public void SetFrameState(LedDriver.Display state) {
            driver.SetFrameState(state);
        }

        public void Write(ulong[] frame)
        {
            throw new NotImplementedException();
        }

        public void Write(Pixel[] frame) {
            // never called - implementation is overridden
            throw new NotImplementedException();
        }

        protected override void FrameDraw(Pixel[] frame) {
            driver.Write(frame);
        }
    }
}
