using Glovebox.Graphics;
using Glovebox.Graphics.Components;
using Glovebox.Graphics.Drivers;
using Glovebox.Graphics.Grid;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace LedHost {
    public sealed class StartupTask : IBackgroundTask {

        BackgroundTaskDeferral _deferral;
        LedMonoMatrix monoMatrix = new LedMonoMatrix();
        LedBiColourMatrix biColorMatrix = new LedBiColourMatrix();
        LedMonoMatrixSPI MonoSPIColorMatrix = new LedMonoMatrixSPI();

        public void Run(IBackgroundTaskInstance taskInstance) {
            _deferral = taskInstance.GetDeferral();

            Task.Run(() => monoMatrix.Start());
            //Task.Run(() => biColorMatrix.Start());
            Task.Run(() => MonoSPIColorMatrix.Start());

            Task.Delay(-1).Wait();


          
        }
    }
}