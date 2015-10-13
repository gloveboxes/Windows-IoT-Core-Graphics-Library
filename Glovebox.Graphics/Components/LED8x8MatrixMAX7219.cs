//using Glovebox.Graphics.Drivers;
//using Glovebox.Graphics.Grid;
//using System;

//namespace Glovebox.Graphics.Components {
//    public class LED8x8MatrixMAX7219 : Grid8x8, ILedDriver {
//        ILedDriver driver;
//        public LED8x8MatrixMAX7219(ILedDriver driver, ushort panels=1) : base("matrix", driver.GetNumberOfPanels()) {
//            this.driver = driver;
//        }

//        public int GetNumberOfPanels() {
//            return driver.GetNumberOfPanels();
//        }

//        public void SetBlinkRate(LedDriver.BlinkRate blinkrate) {

//        }

//        public void SetBrightness(byte level) {
//            driver.SetBrightness(level);
//        }

//        public void SetDisplayState(LedDriver.Display state) {
//            driver.SetDisplayState(state);
//        }

//        public void Write(Pixel[] frame) {
//            throw new NotImplementedException();
//        }

//        protected override void FrameDraw(Pixel[] frame) {
//            driver.Write(frame);
//        }
//    }
//}
