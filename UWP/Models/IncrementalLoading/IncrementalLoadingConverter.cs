using System;
using System.Collections;
using Windows.UI.Xaml.Data;

namespace PlatformBindings.Models.IncrementalLoading
{
    public class IncrementalLoadingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is ISupportCoreIncrementalLoading coreIncremental)
            {
                if (value is IList)
                {
                    return new WinListSupportsIncrementalLoading(coreIncremental);
                }
                else return new WinSupportsIncrementalLoading(coreIncremental);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is WinListSupportsIncrementalLoading coreIncremental)
            {
                return coreIncremental.Source;
            }
            return value;
        }
    }
}