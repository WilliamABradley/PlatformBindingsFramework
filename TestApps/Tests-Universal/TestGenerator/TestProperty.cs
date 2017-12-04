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

namespace Tests.TestGenerator
{
    public class TestProperty : ITestItem
    {
        public TestProperty(string Name)
        {
            this.Name = Name;
        }

        public void UpdateValue(string NewValue)
        {
            Value = NewValue;
            PropertyUpdated?.Invoke(this, NewValue);
        }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        public string Name { get; }
        public string Value { get; private set; }

        public event EventHandler<string> PropertyUpdated;
    }
}