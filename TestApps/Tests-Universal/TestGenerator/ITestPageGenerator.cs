namespace Tests.TestGenerator
{
    public interface ITestPageGenerator
    {
        void CreateTestUI(TestTask Test);

        void CreateTestProperty(TestProperty Property);
    }
}