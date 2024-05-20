using System;

namespace Olli.Common.Console.Parameters
{
    public class Parameter
    {
        private string[] names;

        private string primaryName;

        private string[] alternativeNames;

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        /// <param name="name">Name of parameter.</param>
        /// <param name="required">Require that the parameter is present in the command line.</param>
        /// <param name="description">The explanation of the parameter to add to the help screen.</param>
        public Parameter(string[] names, bool required, string description)
        {
            if (names == null || names.Length == 0)
                throw new ArgumentNullException(nameof(names));

            this.names = names;
            SplitNames();

            Required = required;
            Description = description;
        }

        /// <summary>
        /// Separate combined name array into primary name and alternative names
        /// </summary>
        private void SplitNames()
        {
            // Store primary name
            primaryName = names[0].ToUpperInvariant();

            // Copy alternative names
            alternativeNames = new string[names.Length - 1];
            for (int i = 1; i < names.Length; i++)
                alternativeNames[i - 1] = names[i];

            // Convert to upper invariant
            for (int i = 0; i < alternativeNames.Length; i++)
                alternativeNames[i] = alternativeNames[i].ToUpperInvariant();
        }

        /// <summary>
        /// Returns the value of the parameter.
        /// </summary>
        public string RawValue { get; private set; }

        /// <summary>
        /// Returns true if the parameter was found in the command line.
        /// </summary>
        public bool Exists { get; set; }

        /// <summary>
        /// Returns the primary name of the parameter.
        /// </summary>
        public string PrimaryName { get { return primaryName; } }

        /// <summary>
        /// Returns alternative names of the parameter.
        /// </summary>
        public string[] AlternativeNames { get { return alternativeNames; } }

        /// <summary>
        /// Returns true if the parameter is required in the command line.
        /// </summary>
        public bool Required { get; private set; }

        /// <summary>
        /// Returns the help message associated with the parameter.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Sets the value of the parameter.
        /// </summary>
        /// <param name="value">A string containing a integer expression.</param>
        public virtual void SetValue(string value)
        {
            RawValue = value;
            Exists = true;
        }

        /// <summary>
        /// Resets the value of the parameter.
        /// </summary>
        public virtual void ResetValue()
        {
            RawValue = null;
            Exists = false;
        }

        /// <summary>
        /// Add another value for the parameter
        /// </summary>
        /// <param name="value"></param>
        public virtual void AddValue(string value)
        {
            if (string.IsNullOrEmpty(RawValue))
                SetValue(value);
            else
                RawValue = $"{RawValue} {value}";
        }
    }
}
