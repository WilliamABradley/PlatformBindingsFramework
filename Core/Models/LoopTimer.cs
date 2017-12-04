// ******************************************************************
// Copyright (c) William Bradley
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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