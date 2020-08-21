using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tobin
{
    public static class Extensions
    {
        public static string Sanitize(this string value)
        {
            if (value.Contains(",") == false || value.ToCharArray().Count(f => f == ',') > 1)
            {
                return value;
            }

            var numbers = "0123456789.".ToCharArray().ToDictionary(x => x);
            var index = value.IndexOf(",");

            var sub = value.Substring(0, index) + "." + value.Substring(index + 1, value.Length - index - 1);
            if (sub.All(x => numbers.ContainsKey(x)))
            {
                return sub;
            }
            
            return value;
        }

        public static string CamelCase(this string value, char[] spaceChars = null)
        {
            spaceChars ??= new[] {' ', '_', '-'};
            StringBuilder sb = new StringBuilder(value.Length);
            bool nextCharUpper = true;
            for (int i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (spaceChars.Contains(item))
                {
                    nextCharUpper = true;
                    continue;
                }
                if (nextCharUpper)
                {
                    sb.Append(item.ToString().ToUpper());
                    nextCharUpper = false;
                }
                else
                {
                    sb.Append(item.ToString().ToLower());
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// this will split but ignore splits with in quotes and remove quotes from the split
        /// </summary>
        /// <param name="value"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string[] SmartCsvSplit(this string value, char split)
        {
            bool inQuote = false;
            var output = new List<string>();
            StringBuilder current = new StringBuilder(value.Length);
            foreach (var c in value)
            {
                if (c == split && inQuote == false)
                {
                    output.Add(current.ToString());
                    current.Clear();
                    continue;
                }

                if (c == '"')
                {
                    inQuote = !inQuote;
                    continue;
                }

                current.Append(c);
            }

            output.Add(current.ToString());
            return output.ToArray();
        }
    }
}