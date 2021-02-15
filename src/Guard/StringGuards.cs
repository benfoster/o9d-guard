using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.IO;

namespace O9d.Guard
{
    /// <summary>
    /// Guards for strings
    /// </summary>
    public static class StringGuards
    {
        /// <summary>
        /// Values that the provided <paramref name="value"/> is not null, empty, or whitespace
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="name">The name of the argument</param>
        /// <returns>The value of <paramref name="value"/> if it is not null, empty or whitespace</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the value is null, empty or whitespace
        /// </exception>
        /// <example>
        /// <code>
        /// _name = name.NotNullOrWhiteSpace(nameof(name));
        /// </code>
        /// </example>
        [DebuggerStepThrough]
        public static string NotNullOrWhiteSpace(this string value, [NotNull] string name)
        {           
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value must be provided", name);
            }

            return value;
        }

        public static void DoTheNaughty(string input)
        {
            var p = new Process();
            p.StartInfo.FileName = "exportLegacy.exe";
            p.StartInfo.Arguments = " -user " + input + " -role user";
            p.Start();

            var doc = new XmlDocument {XmlResolver = null};
            doc.Load("/config.xml");
            doc.SelectNodes("/Config/Devices/Device[id='" + input + "']");

            File.ReadAllBytes(Path.Combine("/", input));
        }
    }
}