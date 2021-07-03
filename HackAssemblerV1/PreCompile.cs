using System;
using System.Collections.Generic;

namespace HackAssemblerV1
{
    public static class PreCompile
    {
        public static void RemoveComments(List<string> lines)
        {

            for (int i = 0; i< lines.Count; i++)
            {
                var line = lines[i];

                var indexof = line.IndexOf("//");
                indexof = indexof < 0 ? line.Length : indexof;

                line = line.Substring(0, indexof);
                line = line.Replace(" ", "");
                line = line.Replace("\t", "");

                if (line.Length == 0) { lines.RemoveAt(i); i--; }
                else { lines[i] = line; }

            }

            //DebugHelper.PrintCodeLines(lines);

        }
    }
}
