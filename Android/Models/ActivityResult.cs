using Android.App;
using Android.Content;

namespace PlatformBindings.Models
{
    public class ActivityResult
    {
        public ActivityResult(int requestCode, Result resultCode, Intent data)
        {
            RequestCode = requestCode;
            ResultCode = resultCode;
            Data = data;
        }

        public int RequestCode { get; }
        public Result ResultCode { get; }
        public Intent Data { get; }
    }
}