namespace Glovebox.Graphics.Drivers {
    public class LedDriver {

        public enum Frame : byte {
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
