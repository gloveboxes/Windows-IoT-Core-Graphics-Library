
namespace Glovebox.Graphics.Drivers {
    public interface ILedDriver {

        void SetBlinkRate(LedDriver.BlinkRate blinkrate);
        void SetBrightness(byte level);
        void SetDisplayState(LedDriver.Display state);
        void SetPanels(ushort panels);
        void Write(Pixel[] frame);
    }
}