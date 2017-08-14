using PlatformBindings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Models.Settings;
using PlatformBindings.Services;

namespace PlatformBindings.Common
{
    public static class CoreHelpers
    {
        public static async void OnUIThread(Action action)
        {
            await OnUIThreadAsync(action);
        }

        public static async Task OnUIThreadAsync(Action action)
        {
            await AppServices.Services.UI.DefaultUIBinding.ExecuteAsync(action);
        }

        public static void OnUIThread(IUIBindingInfo UIBinding, Action action)
        {
            UIBinding.Execute(action);
        }

        public static async Task OnUIThreadAsync(IUIBindingInfo UIBinding, Action action)
        {
            await UIBinding.ExecuteAsync(action);
        }

        public static ISettingsContainer GetSettingsContainer(bool GetLocal)
        {
            return GetLocal || !AppServices.Services.IO.SupportsRoaming ? AppServices.Services.IO.GetLocalSettingsContainer() : AppServices.Services.IO.GetRoamingSettingsContainer();
        }

        public static ObjectType DetermineGeneric<T>()
        {
            var generic = typeof(T);
            if (generic == typeof(string))
            {
                return ObjectType.String;
            }
            else if (generic == typeof(int))
            {
                return ObjectType.Int;
            }
            else if (generic == typeof(int?))
            {
                return ObjectType.NullableInt;
            }
            else if (generic == typeof(long))
            {
                return ObjectType.Long;
            }
            else if (generic == typeof(long?))
            {
                return ObjectType.NullableLong;
            }
            else if (generic == typeof(bool))
            {
                return ObjectType.Bool;
            }
            else if (generic == typeof(bool?))
            {
                return ObjectType.NullableBool;
            }
            else if (generic == typeof(float))
            {
                return ObjectType.Float;
            }
            else if (generic == typeof(float?))
            {
                return ObjectType.NullableFloat;
            }
            else
            {
                return ObjectType.ComplexObject;
            }
        }
    }
}