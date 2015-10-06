using System;
using Glovebox.Components.Drivers;
using LightLibrary.Grid;
using LightLibrary;

namespace LightLibrary.Components {
    public class LED8x8MatrixMAX7219 : Grid8x8, ILedDriver {
        ILedDriver driver;
        public LED8x8MatrixMAX7219(ILedDriver driver, ushort panels=1) : base("matrix", panels) {
            this.driver = driver;
            SetPanels(panels);
        }

        public void SetBlinkRate(LedDriver.BlinkRate blinkrate) {
            throw new NotImplementedException();
        }

        public void SetBrightness(byte level) {
            throw new NotImplementedException();
        }

        public void SetDisplayState(LedDriver.Display state) {
            throw new NotImplementedException();
        }

        public void SetPanels(ushort panels) {
            driver.SetPanels(panels);
        }

        public void Write(ulong frameMap) {
            throw new NotImplementedException();
        }

        public void Write(Pixel[] frameMap) {
            throw new NotImplementedException();
        }


        protected override void FrameDraw(Pixel[] frame) {
            driver.Write(frame);
        }
    }
}
