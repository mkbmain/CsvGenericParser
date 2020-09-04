using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tobin.CsvBasicModelGenericImporter
{
    public class CsvBasicModelGenericImporter
    {
        public static async Task<IEnumerable<T>> ImportData<T>(Stream stream, string[] mappedItems, char seperator = ',', bool changeEuNumbersToUk = true,
            bool smartCsvSplit = true,
            Encoding encoding = null)
            where T : new()
        {
            var list = new List<T>();
            await foreach (var item in ImportDataAsyncEnumerable<T>(stream, mappedItems, seperator, changeEuNumbersToUk, smartCsvSplit, encoding))
            {
                list.Add(item);
            }

            return list;
        }

        public static async IAsyncEnumerable<T> ImportDataAsyncEnumerable<T>(Stream stream, string[] mappedItems, char seperator = ',',
            bool changeEuNumbersToUk = true, bool smartCsvSplit = true, Encoding encoding = null)
            where T : new()
        {
            using var streamReader = new StreamReader(stream, encoding ?? Encoding.UTF8);
            string line;
            var firstLine = true;
            while ((line = (await streamReader.ReadLineAsync().ConfigureAwait(false))) != null)
            {
                var parts = smartCsvSplit ? line.SmartCsvSplit(seperator) : line.Split(seperator);
                if (firstLine)
                {
                    firstLine = false;
                    continue;
                }

                if (parts.Length != mappedItems.Length)
                {
                    throw new DataException("Filed Length Miss Match");
                }

                var model = new T();
                for (var i = 0; i < parts.Length; i++)
                {
                    var part = parts[i]?.Trim();
                    var prop = mappedItems[i];
                    if (string.IsNullOrWhiteSpace(part))
                    {
                        continue;
                    }

                    SetObjectProperty(prop, changeEuNumbersToUk ? part.SanitizeNumbers() : part, model);
                }

                yield return model;
            }
        }

        private static void SetObjectProperty(string propertyName, string value, object obj)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName);
            var type = propertyInfo.PropertyType;
            if (type.GenericTypeArguments.Any())
            {
                type = type.GenericTypeArguments.First();
            }

            propertyInfo.SetValue(obj, Convert.ChangeType(value, type), null);
        }
    }
}