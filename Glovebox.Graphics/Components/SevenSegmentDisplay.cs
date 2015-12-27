using System;
using Glovebox.Graphics.Drivers;
using Glovebox.Graphics.SevenSegmentDisplay;

namespace Glovebox.Graphics.Components
{
    public class SevenSegmentDisplay : SevenSegmentDisplayBase, ILedDriver
    {
        ILedDriver driver;

        public SevenSegmentDisplay(ILedDriver driver) : base("ssd", driver.PanelsPerFrame)
        {
            this.driver = driver;
        }

        public int PanelsPerFrame
        {
            get
            {
                return driver.PanelsPerFrame;
            }
        }

        public void SetBlinkRate(LedDriver.BlinkRate blinkrate)
        {
            driver.SetBlinkRate(blinkrate);
        }

        public void SetBrightness(byte level)
        {
            driver.SetBrightness(level);
        }

        public void SetFrameState(LedDriver.Display state)
        {
            driver.SetFrameState(state);
        }

        public void Write(ulong[] frame)
        {
            driver.Write(frame);
        }

        public void Write(Pixel[] frame)
        {
            driver.Write(frame);
        }


        protected override void FrameDraw(ulong[] frame)
        {
            driver.Write(frame);
        }
    }
}
