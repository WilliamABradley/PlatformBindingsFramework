using System;

using UIKit;

namespace Test_IOS.Pages
{
    public partial class TestController : UIViewController, IUIAlertViewDelegate
    {
        public TestController() : base("TestController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var button = new UIButton(UIButtonType.Plain) { Frame = new CoreGraphics.CGRect(0f, 0f, 200, 50), BackgroundColor = UIColor.Red };
            button.SetTitle("Demo Button", UIControlState.Normal);
            button.TouchUpInside += Button_TouchUpInside;
            this.Add(button);
        }

        private void Button_TouchUpInside(object sender, EventArgs e)
        {
            var dlg = UIAlertController.Create("Warning", "Demo Button Pressed", UIAlertControllerStyle.Alert);
            dlg.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, view => { }));
            dlg.AddAction(UIAlertAction.Create("Cool", UIAlertActionStyle.Cancel, view => { }));

            PresentViewController(dlg, true, null);
        }
    }
}