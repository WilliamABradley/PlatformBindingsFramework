using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Tests.TestGenerator;

namespace Tests.Tests
{
    public class NavigationOptions : TestPage
    {
        public NavigationOptions(ITestPageGenerator PageGenerator) : base(null, PageGenerator)
        {
            foreach (var task in TestService.TestRegister)
            {
                if (task.Value.Key == TestNavigationPage.Home) continue;

                AddTestItem(new TestTask
                {
                    Name = GetDescription(task.Value.Key),
                    Test = ui => Task.Run(() =>
                    {
                        TestService.Navigation.Navigate(task.Value.Key);
                        return (string)null;
                    })
                });
            }
        }

        public static string GetDescription(Enum value)
        {
            return value
                .GetType()
                .GetTypeInfo()
                .GetDeclaredField(value.ToString())
                .GetCustomAttribute<DescriptionAttribute>().Description;
        }
    }
}