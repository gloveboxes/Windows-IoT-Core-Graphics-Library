using LightLibrary;

namespace Glovebox.Components.Drivers {
    public interface ILedDriver {

        void SetBlinkRate(LedDriver.BlinkRate blinkrate);
        void SetBrightness(byte level);
        void SetDisplayState(LedDriver.Display state);
        void SetPanels(ushort panels);
        void Write(ulong frameMap);
        void Write(Pixel[] frame);
    }
}