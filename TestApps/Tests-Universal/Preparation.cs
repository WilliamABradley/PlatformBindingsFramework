using PlatformBindings;

namespace Tests
{
    public static class Preparation
    {
        public static void Prepare()
        {
            SMBService.Register();
        }
    }
}