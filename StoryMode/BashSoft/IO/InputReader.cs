using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft
{
    class InputReader
    {
        private const string endCommand = "quit";
        public static void StartReadingCommands()
        {
            string input = "";
            while (input != endCommand)
            {
                OutputWriter.WriteMessage($"{SessionData.currentPath}>");
                input = Console.ReadLine();
                input = input.Trim();
                CommandInterpreter.InterpredCommand(input);
            }
        }
    }
}
