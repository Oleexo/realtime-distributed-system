using System.Text;

namespace Oleexo.RealtimeDistributedSystem.Common.Extensions;

public static class DictionaryExtensions {
    /// <summary>
    ///     Returns a human-readable text string that describes a dictionary that maps objects to objects.
    /// </summary>
    /// <typeparam name="T1">The type of the dictionary keys.</typeparam>
    /// <typeparam name="T2">The type of the dictionary elements.</typeparam>
    /// <param name="dict">The dictionary to describe.</param>
    /// <param name="toString">
    ///     Converts the element to a string. If none specified, <see cref="object.ToString" /> will be
    ///     used.
    /// </param>
    /// <param name="separator">The separator to use. If none specified, the elements should appear separated by a new line.</param>
    /// <returns>
    ///     A string assembled by wrapping the string descriptions of the individual
    ///     pairs with square brackets and separating them with commas.
    ///     Each key-value pair is represented as the string description of the key followed by
    ///     the string description of the value,
    ///     separated by " -> ", and enclosed in curly brackets.
    /// </returns>
    public static string ToHumanReadableString<T1, T2>(this IReadOnlyDictionary<T1, T2>? dict,
                                          Func<T2, string>?                 toString  = null,
                                          string?                           separator = null) {
        if (dict       == null ||
            dict.Count == 0) {
            return "[]";
        }

        if (separator == null) {
            separator = Environment.NewLine;
        }

        var       sb         = new StringBuilder("[");
        using var enumerator = dict.GetEnumerator();
        var       index      = 0;
        while (enumerator.MoveNext()) {
            var pair = enumerator.Current;
            sb.Append("{");
            sb.Append(pair.Key);
            sb.Append(" -> ");

            string? val;
            if (toString != null) {
                val = toString(pair.Value);
            }
            else {
                val = pair.Value == null
                          ? "null"
                          : pair.Value.ToString();
            }

            sb.Append(val);

            sb.Append("}");
            if (index++ < dict.Count - 1) {
                sb.Append(separator);
            }
        }

        sb.Append("]");
        return sb.ToString();
    }
}
