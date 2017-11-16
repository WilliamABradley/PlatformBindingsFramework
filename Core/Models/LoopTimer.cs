using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlatformBindings.Models
{
    public class LoopTimer
    {
        public LoopTimer()
        {
        }

        public LoopTimer(TimeSpan Interval) : this(Interval, false)
        {
            this.Interval = Interval;
            this.StartImmediate = StartImmediate;
        }

        public LoopTimer(TimeSpan Interval, bool StartImmediate)
        {
            this.Interval = Interval;
            this.StartImmediate = StartImmediate;
        }

        public void Start()
        {
            Stop();

            if (Interval == null) throw new Exception("Interval not Set");
            TokenSource = new CancellationTokenSource();
            LoopTask = Task.Run(async () =>
            {
                var token = TokenSource.Token;

                if (StartImmediate) Tick?.Invoke(this, EventArgs.Empty);
                while (true)
                {
                    await Task.Delay(Interval);
                    if (!token.IsCancellationRequested)
                    {
                        Tick?.Invoke(this, EventArgs.Empty);
                    }
                    else break;
                }
            }, TokenSource.Token);
        }

        public void Stop()
        {
            TokenSource?.Cancel();
        }

        private Task LoopTask { get; set; }
        private CancellationTokenSource TokenSource { get; set; }

        public bool StartImmediate { get; set; }
        public TimeSpan Interval { get; set; }
        public event EventHandler Tick;
    }
}