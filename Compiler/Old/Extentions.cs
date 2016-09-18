#pragma warning disable 1591
using System.Collections.Generic;

namespace CPU_OS_Simulator.Compiler.Old
{
    /// <summary>
    /// This class contains extension methods used by the compiler module
    /// </summary>
    public static class Extentions
    {
        /// <summary>
        /// This function splits a string while keeping all delimiter characters
        /// </summary>
        /// <param name="s">the string to split</param>
        /// <param name="delims"> the delimiters to split on</param>
        /// <returns></returns>
        public static IEnumerable<string> SplitAndKeep(this string s, char[] delims)
        {
            int start = 0, index;

            while ((index = s.IndexOfAny(delims, start)) != -1)
            {
                if (index - start > 0)
                    yield return s.Substring(start, index - start);
                yield return s.Substring(index, 1);
                start = index + 1;
            }

            if (start < s.Length)
            {
                yield return s.Substring(start);
            }
        }
    }
}
