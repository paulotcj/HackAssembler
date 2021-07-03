using System;
using System.Collections.Generic;

namespace HackAssemblerV1
{
    public static class DebugHelper
    {
        public static void PrintCodeLines(List<string> lines)
        {

            Console.WriteLine("         Clean Code\n-------------------");
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var lineNumdisplay = "     " + i;
                lineNumdisplay = lineNumdisplay.Substring(lineNumdisplay.Length - 5, 5);
                Console.WriteLine("{0} |  " + line, lineNumdisplay);

            }
            Console.WriteLine("-------------------");

        }
    }
}
