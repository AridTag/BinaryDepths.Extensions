using System.Text.RegularExpressions;

namespace BinaryDepths.Extensions
{
    public static class StringExtensions
    {
        private static Regex _SplitCamelCaseInnerRegex = null;
        private static Regex _SplitCamelCaseOuterRegex = null;

        /// <summary>
        /// Will split a string such as FileSystem into File System or CPUsRock into CPUs Rock.
        /// </summary>
        /// <remarks>
        /// Note that the first call will be slower as the regex is compiled
        /// </remarks>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SplitCamelCase(this string str)
        {
            if (_SplitCamelCaseInnerRegex == null)
            {
                _SplitCamelCaseOuterRegex = new Regex(@"(\p{Ll})(\P{Ll})", RegexOptions.Compiled);
                _SplitCamelCaseInnerRegex = new Regex(@"(\P{Ll})(\P{Ll}\p{Ll})", RegexOptions.Compiled);
            }

            return _SplitCamelCaseOuterRegex.Replace(_SplitCamelCaseInnerRegex.Replace(str, "$1 $2"), "$1 $2");
        }
    }
}
