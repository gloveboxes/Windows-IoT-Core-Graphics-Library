
namespace Glovebox.Graphics.Drivers {
    public interface ILedDriver {
        int PanelsPerDisplay { get; }
        void SetBlinkRate(LedDriver.BlinkRate blinkrate);
        void SetBrightness(byte level);
        void SetDisplayState(LedDriver.Display state);
        void Write(Pixel[] frame);
    }
}