using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Olli.Common.Logging;

namespace Olli.Common.Console.Parameters
{
    /// <summary>
    /// Provides a simple strongly typed interface to work with command line parameters.
    /// </summary>
    /// <seealso cref="ConsoleParameterManager"/>
    public class ParameterManager
    {
        /// <summary>
        /// A private dictonary containing the parameters.
        /// </summary>
        private Dictionary<string, Parameter> parameters = new Dictionary<string, Parameter>();

        /// <summary>
        /// Full argument text writed on commad line. Can be used as debug and development purposes.
        /// Printed in command line when using -VALUES argument
        /// </summary>
        private string CommandLineText = string.Empty;

        /// <summary>
        /// Logging handler, directs log messages to console, output file, etc.
        /// </summary>
        public ILogger logger;

        /// <summary>
        /// Creats a new empty command line object.
        /// </summary>
        public ParameterManager(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Returns a command line parameter by the name.
        /// </summary>
        /// <param name="name">The name of the parameter (the word after the initial hyphen (-).</param>
        /// <returns>A reference to the named comman line object.</returns>
        public Parameter this[string name]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentNullException(nameof(name));

                if (!parameters.ContainsKey(name.ToUpperInvariant()))
                    throw new ArgumentException("Not a registered parameter", name);

                return parameters[name.ToUpperInvariant()];
            }
        }

        /// <summary>
        /// Registers parameters to be used and adds them to the help screen.
        /// </summary>
        /// <param name="arguments">The parameter to add.</param>
        public Parameter AddParameter(bool required, string description, params string[] names)
        {
            logger.LogInfo($"+ New basic parameter [names:{string.Join(", ", names)}; req:{required}; desc:{description}]");

            Parameter newParameter = new Parameter(names, required, description);
            if (parameters.ContainsKey(newParameter.PrimaryName))
                throw new ArgumentException("Parameter is already registered", newParameter.PrimaryName);

            parameters.Add(newParameter.PrimaryName, newParameter);
            return newParameter;
        }

        /// <summary>
        /// Registers flag parameter to be used and adds them to the help screen.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public FlagParameter AddFlagParameter(string description, params string[] names)
        {
            logger.LogInfo($"+ New flag parameter [names:{string.Join(", ", names)}; desc:{description}]");

            FlagParameter newParameter = new FlagParameter(names, description);
            if (parameters.ContainsKey(newParameter.PrimaryName))
                throw new ArgumentException("Parameter is already registered", newParameter.PrimaryName);

            parameters.Add(newParameter.PrimaryName, newParameter);
            return newParameter;
        }

        /// <summary>
        /// Registers integer parameter to be used and adds them to the help screen.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IntegerParameter AddIntegerParameter(bool required, string description, params string[] names)
        {
            logger.LogInfo($"+ New integer parameter [names:{string.Join(", ", names)}; desc:{description}]");

            IntegerParameter newParameter = new IntegerParameter(names, required, description);
            if (parameters.ContainsKey(newParameter.PrimaryName))
                throw new ArgumentException("Parameter is already registered", newParameter.PrimaryName);

            parameters.Add(newParameter.PrimaryName, newParameter);
            return newParameter;
        }

        /// <summary>
        /// Parses the command line and sets the value of each registered parmaters.
        /// </summary>
        /// <param name="args">The arguments array sent to main()</param>
        /// <returns>Any reminding strings after arguments has been processed.</returns>
        public bool Parse(string[] args)
        {
            // Reset all arguments, it is possible to relay parameters to pre-existing instance
            foreach (KeyValuePair<string, Parameter> item in parameters)
                parameters[item.Key].ResetValue();

            // Original command line text for debugging purposes
            CommandLineText = string.Join(" ", args);

            // Parsing
            Parameter currentParam = null;
            foreach (string item in args)
            {
                // Key already found, link to matching value parameter
                if (item.Length <= 1 || !item.StartsWith("-", StringComparison.OrdinalIgnoreCase))
                {
                    if (currentParam == null)
                        logger.LogWarning($"Missing paramater name, ignoring argument value: {item}");
                    else
                        currentParam.AddValue(item);

                    continue;
                }

                // Throw error if key cannot be mapped to parameter
                if (!parameters.ContainsKey(item.Substring(1).ToUpperInvariant()))
                    throw new ArgumentException("Parameter not supported", item);

                // Store parameter
                currentParam = parameters[item.Substring(1).ToUpperInvariant()];
                if (!currentParam.Exists)
                    currentParam.Exists = true;
                else
                    throw new ArgumentException($"Parameter provided more than once", item);
            }

            // Check that required parameters are present in the command line.
            foreach (string key in parameters.Keys)
                if (parameters[key].Required && !parameters[key].Exists)
                    throw new ArgumentException("Required parameter missing", key);

            return true;
        }

        /// <summary>
        /// Generates the help screen or screen with current parameters
        /// </summary>
        /// <param name="showValues">If set, replaces the help text with current values and full arguments text.</param>
        /// <returns>Formatted text for console</returns>
        public string HelpScreen(bool showValues)
        {
            StringBuilder txt = new StringBuilder();

            txt.AppendLine(AppDomain.CurrentDomain.FriendlyName);
            txt.Append('*', AppDomain.CurrentDomain.FriendlyName.Length);
            txt.AppendLine();

            if (showValues)
            {
                txt.AppendLine("Command line text:");
                txt.AppendLine(CommandLineText);
                txt.AppendLine();
            }

            txt.AppendLine("Command line parameters:");

            foreach (string key in parameters.Keys)
            {
                txt.AppendFormat(CultureInfo.InvariantCulture,
                                 "\n-{0}\n",
                                 parameters[key].PrimaryName);

                txt.AppendLine(parameters[key].Required == true ? "Required" : "Optional");

                if (!showValues)
                {
                    txt.AppendLine(parameters[key].Description); // Ohje
                    continue;
                }

                if (parameters[key].Exists)
                {
                    txt.AppendFormat(CultureInfo.InvariantCulture,
                                     "Parameter has been set with value:\n{0}\n", parameters[key].RawValue);
                    continue;
                }

                txt.AppendLine("Parameter has not set");
            }

            txt.AppendLine();
            return txt.ToString();
        }
    }
}
