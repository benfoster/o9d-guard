using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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
    }
}