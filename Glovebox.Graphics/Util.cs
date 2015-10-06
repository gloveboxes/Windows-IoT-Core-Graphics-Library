using System.Threading.Tasks;

namespace Glovebox.Graphics {
    static class Util {
        static public void Delay(int milliseconds) {
            Task.Delay(milliseconds).Wait();
        }
    }
}
