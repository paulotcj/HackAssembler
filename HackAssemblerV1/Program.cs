using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HackAssemblerV1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var inputFileName = "Pong.asm"; var inputPath = "";
            var outputFileName = "Pong.hack"; var outputPath = "";

            var lines = await FileHandling.ReadFileAsync(inputFileName, inputPath);


            PreCompile.RemoveComments(lines);

            await Compile.RunAsync(lines);

            await FileHandling.WriteFileAsync(lines, outputFileName, outputPath);


            Console.WriteLine("File written: " + outputPath + outputFileName);
            Console.WriteLine("Done!");
        }



    }
}
