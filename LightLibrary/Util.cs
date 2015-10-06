using System.Threading.Tasks;

namespace LightLibrary {
    static class Util {
        static public void Delay(int milliseconds) {
            Task.Delay(milliseconds).Wait();
        }
    }
}
