using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tobin.GusessCsvFieldsAndtypesGen
{
    internal class DetermineTypesForCsv
    {
        public async static Task<(string, string)> Run(string filePath, char seperator, bool smartCsvSplit = true)
        {
            var list = new List<ProposedType>();
            int lineNumber = 1;
            using (var streamReader = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                var firstLine = true;
                while ((line = (await streamReader.ReadLineAsync())) != null)
                {
                    Console.WriteLine(lineNumber++);
                    var parts = smartCsvSplit
                        ? line.SmartCsvSplit(seperator)
                        : line.Split(seperator);
                    parts = parts.Select(x => x.Trim()).ToArray();
                    if (firstLine)
                    {
                        if (parts.Distinct().Count() < parts.Length)
                        {
#if DEBUG
                            Debugger.Break();
#else
                        throw new DataException();
#endif
                        }

                        list.AddRange(parts.Select(x => x.CamelCase()).Select((t, i) => new ProposedType {Name = t.Replace(" ", "_").Replace(".", "_").Replace("-", "_"), index = i, DataType = GTypes.Unknown}));
                        firstLine = false;
                        continue;
                    }

                    if (parts.Length != list.Count)
                    {
#if DEBUG
                        Debugger.Break();
#else
                        throw new DataException();
#endif
                    }

                    for (int i = 0; i < parts.Length; i++)
                    {
                        var part = parts[i];
                        var item = list[i];
                        if (string.IsNullOrWhiteSpace(part))
                        {
                            item.EmptyOrNull = true;
                            continue;
                        }

                        item.DataType = DeterimeTypes.DetermineTypeHelper(item.DataType, part.Sanitize());
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                var b = item.EmptyOrNull ? "?" : "";
                if (item.DataType == GTypes.String)
                {
                    b = "";
                }

                sb.AppendLine($"public {item.DataType.ToString().ToLower()}{b} {item.Name}" + "{get;set;}");
            }

            string mode = sb.ToString();
            string details = string.Join(",", list.Select(f => $"\"{f.Name}\""));
            return (mode, details);
        }
    }
}