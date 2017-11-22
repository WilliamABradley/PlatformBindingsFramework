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