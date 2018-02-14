using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft
{
    public class InputReader
    {
        private CommandInterpreter interpreter;
        private const string endCommand = "quit";

        public InputReader(CommandInterpreter interpreter)
        {
            this.interpreter = interpreter;
        }
        public void StartReadingCommands()
        {
            string input = "";
            while (input != endCommand)
            {
                OutputWriter.WriteMessage($"{SessionData.currentPath}>");
                input = Console.ReadLine();
                input = input.Trim();
                this.interpreter.InterpredCommand(input);
            }
        }
    }
}
