using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Caulius.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the source <see cref="string"/> without diacritics (non spacing marks).
        /// </summary>
        /// <param name="source">
        /// The source <see cref="string"/>.
        /// </param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return string.Concat(source.Normalize(NormalizationForm.FormKD).Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
        }
    }
}
