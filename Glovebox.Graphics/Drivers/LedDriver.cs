namespace Glovebox.Graphics.Drivers {
    public class LedDriver {
        public static object LockI2C = new object();

        public enum Display : byte {
            On,
            Off,
        }

        public enum BlinkRate : byte {
            Off,
            Fast,
            Medium,
            Slow,
        }
    }
}
