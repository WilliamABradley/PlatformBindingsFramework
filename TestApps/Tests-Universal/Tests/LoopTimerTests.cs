using PlatformBindings.Models;
using PlatformBindings.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Tests.Tests
{
    public class LoopTimerTests : ViewModelBase
    {
        public LoopTimerTests()
        {
            Looper.Tick += Looper_Tick;
        }

        public void Start()
        {
            Stop();

            Timer.Start();
            Looper.Start();
        }

        public void Stop()
        {
            Looper.Stop();
            Timer.Reset();
        }

        private async void Looper_Tick(object sender, EventArgs e)
        {
            UIBinding.Execute(() =>
            {
                Elapsed = $"Elapsed: {Timer.Elapsed.TotalSeconds}s";
                UpdateProperty(nameof(Elapsed));
            });

            //verify async tasks don't disturb loop
            await Task.Delay(4000);
        }

        public string Elapsed { get; private set; }

        private LoopTimer Looper = new LoopTimer(TimeSpan.FromSeconds(2), true);
        private Stopwatch Timer = new Stopwatch();
    }
}