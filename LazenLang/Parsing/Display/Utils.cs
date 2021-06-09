using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazenLang.Parsing.Display
{
    public static class Utils
    {
        public static string Indent(int level)
        {
            return String.Concat(Enumerable.Repeat("|    ", level));
        }

        public static string PrettyArray(IPrettyPrintable[] list, int level)
        {
            if (list.Length == 0)
            {
                return "Empty Array";
            }
            else
            {
                var sb = new StringBuilder("Array");
                sb.AppendLine();
                foreach (var elem in list)
                {
                    sb.AppendLine(Indent(level + 1) + elem.Pretty(level + 1));
                }
                return sb.ToString();
            }
        }

        // TODO: find a more elegant way to remove these annoying empty lines
        public static string PostProcessResult(string output)
        {
            var lines = output.Split('\n');
            var result = new StringBuilder();

            foreach (var line in lines)
            {
                if (line.Trim().Length != 0)
                {
                    result.AppendLine(line.Trim());
                }
            }

            return result.ToString();
        }
    }
}
