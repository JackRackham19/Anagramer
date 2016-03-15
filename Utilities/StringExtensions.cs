using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anagramer.Utilities
{
    public static class StringExtensions
    {
        public static string RemoveWhitespace(this string subject)
        {
            var nonWhitespace = subject.Where(c => !char.IsWhiteSpace(c));
            var builder = new StringBuilder();
            foreach(var letter in nonWhitespace)
            {
                builder.Append(letter);
            }
            return builder.ToString();
        }
    }
}
