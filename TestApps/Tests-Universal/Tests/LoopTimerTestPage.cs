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

using PlatformBindings.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Tests.TestGenerator;

namespace Tests.Tests
{
    public class LoopTimerTestPage : TestPage
    {
        public LoopTimerTestPage(ITestPageGenerator PageGenerator) : base("Loop Timer Tests", PageGenerator)
        {
            Looper.Tick += Looper_Tick;
            AddTestItem(new TestTask
            {
                Name = "Start",
                Test = context =>
                {
                    Start();
                    return Task.FromResult((string)null);
                }
            });

            AddTestItem(new TestTask
            {
                Name = "Stop",
                Test = context =>
                {
                    Stop();
                    return Task.FromResult((string)null);
                }
            });

            AddTestItem(TimeDisplay);
        }

        private void Start()
        {
            Stop();

            Timer.Start();
            Looper.Start();
        }

        private void Stop()
        {
            Looper.Stop();
            Timer.Reset();
        }

        private async void Looper_Tick(object sender, EventArgs e)
        {
            TimeDisplay.UpdateValue($"{Timer.Elapsed.TotalSeconds}s");

            //verify async tasks don't disturb loop
            await Task.Delay(4000);
        }

        public string Elapsed { get; private set; }

        private LoopTimer Looper = new LoopTimer(TimeSpan.FromSeconds(2), true);
        private Stopwatch Timer = new Stopwatch();

        private TestProperty TimeDisplay = new TestProperty("Elapsed");
    }
}