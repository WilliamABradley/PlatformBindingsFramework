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