using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Oleexo.RealtimeDistributedSystem.Common.Extensions;

public static class StringExtensions {
    /// <summary>
    ///     Indicates whether the specified string is null or an empty string ("")
    /// </summary>
    /// <param name="value">The string to test</param>
    /// <returns>true if the value parameter is null or an empty string (""); otherwise, false </returns>
    [Pure]
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value) {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    ///     Indicates whether a specified string is null, empty, or consists only of white-space characters
    /// </summary>
    /// <param name="s">The string to test</param>
    /// <returns>true if the value parameter is null or Empty, or if value consists exclusively of white-space characters</returns>
    [Pure]
    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? s) {
        return string.IsNullOrWhiteSpace(s);
    }
}