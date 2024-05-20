using System;

namespace Olli.Common.Console.Parameters
{
    public class FlagParameter : Parameter
    {
        public FlagParameter(string[] names, string description)
            : base(names, false, description)
        {

        }

        public bool Value { get { return Exists; } }

        public override void ResetValue()
        {
            base.ResetValue();
        }

        public override void SetValue(string value)
        {
            throw new NotSupportedException($"Flag parameter {PrimaryName} does not support values");
        }

        public override void AddValue(string value)
        {
            throw new NotSupportedException($"Flag parameter {PrimaryName} does not support values");
        }
    }
}
