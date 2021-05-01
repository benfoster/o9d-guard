using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace O9d.Guard
{
    /// <summary>
    /// Guards for generic types
    /// </summary>
    public static class GenericGuards
    {
        /// <summary>
        /// Validates that the provided <paramref name="value"/> is not null
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="name">The name of the argument</param>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>The value of <paramref name="value"/> if it is not null</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the value is null
        /// </exception>
        /// <example>
        /// <code>
        /// _customer = customer.NotNull(nameof(customer));
        /// </code>
        /// </example>
        [DebuggerStepThrough]
        public static T NotNull<T>([NotNull]this T value, string name)
        {           
            return value ?? throw new ArgumentNullException(name);
        }
    }
}
