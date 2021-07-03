using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackAssemblerV1
{
    public static class Compile
    {
        private static Dictionary<string, int> symbolTable;


        public static async Task RunAsync(List<string> lines)
        {
            InitializeSymbolTable();
            SymbolFirstPass(lines);
            //await FileHandling.WriteFileAsync(lines, "debug_firstpass.txt", "");
            SymbolSecondPass(lines);

            //DebugHelper.PrintCodeLines(lines);

            //await FileHandling.WriteFileAsync(lines, "debug_clean.txt", "");
            CompileGeneral(lines);

            //DebugHelper.PrintCodeLines(lines);


        }

        public static void CompileGeneral(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {

                var line = lines[i];
                if (line.Substring(0, 1).CompareTo("@") == 0)
                {
                    lines[i] = A_Instructions(line, i);
                }
                else if ( line.IndexOf("=") > 0 || line.IndexOf(";") > 0)
                {
                    lines[i] = C_Instruction(line);
                }
                else { new Exception("Invalid instruction at line " + i + " - " + line );  }
            }
        }

        private static void SymbolSecondPass(List<string> lines)
        {
            var indexPadding = 16;
            for (int i = 0; i < lines.Count; i++)
            {

                var line = lines[i];
                if (line.Substring(0, 1).CompareTo("@") == 0)
                {
                    var lineSubStr = line.Substring(1);
                    if (int.TryParse(lineSubStr, out _) == false)
                    {
                        if (symbolTable.TryAdd(lineSubStr, indexPadding) == true)
                        { indexPadding++;  }                        

                        lines[i] = "@" + symbolTable[lineSubStr];
                    }
                }


            }

        }

        private static void SymbolFirstPass(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.Substring(0, 1).CompareTo("(") == 0)
                {
                    if (line.LastIndexOf(")") <= 0) { new Exception("Invalid sintax at line "+i+" - Missing ')'"); }
                    line = line.Replace("(","").Replace(")","");

                    symbolTable.Add(line, i);

                    lines.RemoveAt(i); i--;

                }
            }


        }

        private static string C_Instruction(string line)
        {
            var instrPrefix = "111"; string dest = ""; string comp = ""; string jump = "";
            string[] instructions;

            if (line.IndexOf("=") >= 0)
            {
                instructions = line.Split("=");
                dest = instructions[0];
                comp = instructions[1].Split(";")[0];
            }

            if (line.IndexOf(";") >= 0)
            {
                instructions = line.Split(";");
                if(string.IsNullOrEmpty(comp) == true)
                {
                    comp = instructions[0];
                }
                jump = instructions[1];
            }

            dest = C_InstructionDest(dest);
            comp = C_InstructionComp(comp);
            jump = C_InstructionJump(jump);

            return instrPrefix + comp + dest + jump;
        }

        private static string C_InstructionJump(string jump)
        {
            string valReturn;
            switch (jump ?? String.Empty)
            {
                case "":
                    valReturn = "000";
                    break;

                case "JGT":
                    valReturn = "001";
                    break;
                case "JEQ":
                    valReturn = "010";
                    break;
                case "JGE":
                    valReturn = "011";
                    break;
                case "JLT":
                    valReturn = "100";
                    break;
                case "JNE":
                    valReturn = "101";
                    break;
                case "JLE":
                    valReturn = "110";
                    break;
                case "JMP":
                    valReturn = "111";
                    break;
                default:
                    valReturn = "000";
                    break;
            }
            return valReturn;

        }

        private static string C_InstructionComp(string comp)
        {
            string valReturn;
            switch (comp ?? String.Empty)
            {
                case "0":
                    valReturn = "0" + "101010";
                    break;
                case "1":
                    valReturn = "0" + "111111";
                    break;
                case "-1":
                    valReturn = "0" + "111010";
                    break;
                case "D":
                    valReturn = "0" + "001100";
                    break;
                case "A":
                    valReturn = "0" + "110000";
                    break;
                case "!D":
                    valReturn = "0" + "001101";
                    break;
                case "!A":
                    valReturn = "0" + "110001";
                    break;
                case "-D":
                    valReturn = "0" + "001111";
                    break;
                case "-A":
                    valReturn = "0" + "110011";
                    break;
                case "D+1":
                    valReturn = "0" + "011111";
                    break;
                case "A+1":
                    valReturn = "0" + "110111";
                    break;
                case "D-1":
                    valReturn = "0" + "001110";
                    break;
                case "A-1":
                    valReturn = "0" + "110010";
                    break;
                case "D+A":
                    valReturn = "0" + "000010";
                    break;
                case "D-A":
                    valReturn = "0" + "010011";
                    break;
                case "A-D":
                    valReturn = "0" + "000111";
                    break;
                case "D&A":
                    valReturn = "0" + "000000";
                    break;
                case "D|A":
                    valReturn = "0" + "010101";
                    break;
                case "M":
                    valReturn = "1" + "110000";
                    break;
                case "!M":
                    valReturn = "1" + "110001";
                    break;
                case "-M":
                    valReturn = "1" + "110011";
                    break;
                case "M+1":
                    valReturn = "1" + "110111";
                    break;
                case "M-1":
                    valReturn = "1" + "110010";
                    break;
                case "D+M":
                    valReturn = "1" + "000010";
                    break;
                case "D-M":
                    valReturn = "1" + "010011";
                    break;
                case "M-D":
                    valReturn = "1" + "000111";
                    break;
                case "D&M":
                    valReturn = "1" + "000000";
                    break;
                case "D|M":
                    valReturn = "1" + "010101";
                    break;
                default:
                    valReturn = "0" + "000000";
                    break;

            }

            return valReturn;

        }

        private static string C_InstructionDest(string dest)
        {
            string valReturn;
            switch(dest ?? String.Empty)
            {
                case "":
                    valReturn = "000";
                    break;
                case "M":
                    valReturn = "001";
                    break;
                case "D":
                    valReturn = "010";
                    break;
                case "MD":
                    valReturn = "011";
                    break;
                case "A":
                    valReturn = "100";
                    break;
                case "AM":
                    valReturn = "101";
                    break;
                case "AD":
                    valReturn = "110";
                    break;
                case "AMD":
                    valReturn = "111";
                    break;
                default:
                    valReturn = "000";
                    break;
            }

            return valReturn;
        }

        private static string A_Instructions(string line, int lineNumber)
        {
            var inputsubstring = line.Substring(1);
            if (int.TryParse(inputsubstring, out _) == true) { line = ConvertTo(inputsubstring); }
            else { new Exception("Error Compiling Line: " + lineNumber + " - " + line); }

            return line;
        }

        private static string ConvertTo(string value, int wordLength = 16, int baseFrom = 10, int baseTo = 2)
        {
            string conversion = ("0000000000000000" + Convert.ToString(Convert.ToInt32(value, baseFrom), baseTo));
            conversion = conversion.Substring(conversion.Length - wordLength, wordLength);
            //Console.WriteLine(conversion);
            return conversion;
        }

        private static void InitializeSymbolTable()
        {
            symbolTable = new Dictionary<string, int>();
            symbolTable.Add("R0",0);
            symbolTable.Add("R1", 1);
            symbolTable.Add("R2", 2);
            symbolTable.Add("R3", 3);
            symbolTable.Add("R4", 4);
            symbolTable.Add("R5", 5);
            symbolTable.Add("R6", 6);
            symbolTable.Add("R7", 7);
            symbolTable.Add("R8", 8);
            symbolTable.Add("R9", 9);
            symbolTable.Add("R10", 10);
            symbolTable.Add("R11", 11);
            symbolTable.Add("R12", 12);
            symbolTable.Add("R13", 13);
            symbolTable.Add("R14", 14);
            symbolTable.Add("R15", 15);

            symbolTable.Add("SCREEN", 16384);
            symbolTable.Add("KBD", 24576);

            symbolTable.Add("SP", 0);
            symbolTable.Add("LCL", 1);
            symbolTable.Add("ARG", 2);
            symbolTable.Add("THIS", 3);
            symbolTable.Add("THAT", 4);

            //PrintSymbolTable();

        }

        private static void PrintSymbolTable()
        {
            foreach (KeyValuePair<string, int> i in symbolTable)
            {
                Console.WriteLine("Key = {0}, Value = {1}", i.Key, i.Value);
            }
        }


    }


}
