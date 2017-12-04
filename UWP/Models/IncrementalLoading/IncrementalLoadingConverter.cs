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
                    return new UWPListSupportsIncrementalLoading(coreIncremental);
                }
                else return new UWPSupportsIncrementalLoading(coreIncremental);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is UWPListSupportsIncrementalLoading coreIncremental)
            {
                return coreIncremental.Source;
            }
            return value;
        }
    }
}