using System;

namespace Olli.Common.Console.Parameters
{
    public class IntegerParameter : Parameter
    {
        public IntegerParameter(string[] names, bool required, string description)
            : base(names, required, description) { }

        public int Value { get; set; }

        public override void ResetValue()
        {
            base.ResetValue();
            Value = 0;
        }

        public override void SetValue(string value)
        {
            base.SetValue(value);

            if (int.TryParse(value, out int parsedValue))
                Value = parsedValue;
            else
                throw new ArgumentException("Invalid value for integer parameter", PrimaryName);
        }

        public override void AddValue(string value)
        {
            if (string.IsNullOrEmpty(RawValue))
                SetValue(value);
            else
                throw new Exception($"Invalid value add, int parameter {PrimaryName} already has a value");
        }
    }
}
