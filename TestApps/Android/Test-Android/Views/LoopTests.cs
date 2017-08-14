using Android.App;
using Android.OS;
using Android.Widget;
using Tests.Tests;
using GalaSoft.MvvmLight.Helpers;
using PlatformBindings;

namespace Test_Android.Views
{
    [Activity(Label = "LoopTests")]
    public class LoopTests : LibActivity
    {
        public LoopTimerTests Viewmodel { get; } = new LoopTimerTests();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoopTests);

            var start = FindViewById<Button>(Resource.Id.LoopTest_Start);
            start.Click += delegate { Viewmodel.Start(); };

            var stop = FindViewById<Button>(Resource.Id.LoopTest_Stop);
            stop.Click += delegate { Viewmodel.Stop(); };

            this.SetBinding(() => Viewmodel.Elapsed, () => Elapsed.Text, BindingMode.OneWay);
        }

        private TextView _Elapsed;

        public TextView Elapsed
        {
            get { return _Elapsed ?? (_Elapsed = FindViewById<TextView>(Resource.Id.LoopTest_Elapsed)); }
        }
    }
}