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
        public static Task<IEnumerable<T>> ImportData<T>(string filePath, string[] mappedItems, char seperator = ',', bool smartCsvSplit = true) where T : new()
        {
            return ImportData<T>(File.Open(filePath, FileMode.Open), mappedItems, seperator, smartCsvSplit);
        }

        public static async Task<IEnumerable<T>> ImportData<T>(Stream stream, string[] mappedItems, char seperator = ',', bool smartCsvSplit = true) where T : new()
        {
            var data = new List<T>();
            using var streamReader = new StreamReader(stream, Encoding.UTF8);
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

                    SetObjectProperty(prop, part.Sanitize(), model);
                }

                data.Add(model);
            }

            return data;
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