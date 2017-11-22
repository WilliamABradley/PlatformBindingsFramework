using PlatformBindings;
using PlatformBindings.Services;
using System;
using System.Collections.Generic;
using Tests.TestGenerator;
using Tests.Tests;

namespace Tests
{
    public static class TestService
    {
        public static void Register(Navigator<TestNavigationPage> Navigation)
        {
            SMBService.Register();
            TestService.Navigation = Navigation;

            RegisterTest(TestNavigationPage.Home, typeof(NavigationOptions));
            RegisterTest(TestNavigationPage.ContextMenu, typeof(ContextMenuTestPage));
            RegisterTest(TestNavigationPage.Pickers, typeof(PickerTestPage));
            RegisterTest(TestNavigationPage.FileFolder, typeof(FileFolderTestPage));
            RegisterTest(TestNavigationPage.LoopTimer, typeof(LoopTimerTestPage));
            RegisterTest(TestNavigationPage.Settings, typeof(SettingTestPage));
            RegisterTest(TestNavigationPage.Credential, typeof(CredentialTestPage));
            RegisterTest(TestNavigationPage.OAuth, typeof(OAuthTestPage));
        }

        private static void RegisterTest(TestNavigationPage Identifier, Type Page)
        {
            TestRegister.Add(new KeyValuePair<TestNavigationPage, Type>(Identifier, Page));
        }

        public static void AddAddtionalTestItem(Type TestType, ITestItem Item)
        {
            if (!TestExtensions.ContainsKey(TestType))
            {
                TestExtensions.Add(TestType, new List<ITestItem>());
            }

            var list = TestExtensions[TestType];
            list.Add(Item);
        }

        public static IReadOnlyList<ITestItem> GetExtensions(Type TestType)
        {
            if (TestExtensions.ContainsKey(TestType))
            {
                return TestExtensions[TestType];
            }
            else return new List<ITestItem>();
        }

        public static Navigator<TestNavigationPage> Navigation { get; private set; }

        private static Dictionary<Type, List<ITestItem>> TestExtensions = new Dictionary<Type, List<ITestItem>>();

        public static List<KeyValuePair<TestNavigationPage, Type>?> TestRegister = new List<KeyValuePair<TestNavigationPage, Type>?>();
    }
}