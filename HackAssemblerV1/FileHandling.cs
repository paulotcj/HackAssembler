using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HackAssemblerV1
{
    public static class FileHandling
    {

        public static async Task<List<string>> ReadFileAsync(string file, string path = "")
        {
            var listLines = new List<string>();

            System.IO.StreamReader srFile = new System.IO.StreamReader(path + file);

            string line;
            while ((line = await srFile.ReadLineAsync() ) != null)
            {
                listLines.Add(line);
            }

            srFile.Close();


            return listLines;
        }

        public static async Task WriteFileAsync(List<string> lines, string file, string path = "")
        {
            await File.WriteAllLinesAsync(path + file, lines.ToArray());
        }

    }
}
