using BashSoft.Exceptions;
using BashSoft.IO.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BashSoft
{
    public class CommandInterpreter
    {
        private Tester judge;
        private StudentsRepository repository;
        private IOManager inputOutputManeger;
        public CommandInterpreter(Tester judge, StudentsRepository repository, IOManager inputOutputManeger)
        {
            this.judge = judge;
            this.repository = repository;
            this.inputOutputManeger = inputOutputManeger;
        }
        public void InterpredCommand(string input)
        {
            string[] data = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string commandName = data[0].ToLower();

            try
            {
                Command command = this.ParseCommand(input, commandName, data);
                command.Execute();
            }
            catch (Exception ex)
            {
                OutputWriter.DisplayException(ex.Message);
            }

        }

        private Command ParseCommand(string input, string command, string[] data)
        {
            switch (command)
            {
                case "open":
                    return new OpenFileCommand(input, data, this.judge, this.repository, this.inputOutputManeger);

                case "mkdir":
                    return new MakeDirectoryCommand(input, data, this.judge, this.repository, this.inputOutputManeger);

                case "ls":
                    return new TraverseFoldersCommand(input, data, this.judge, this.repository, this.inputOutputManeger);

                case "cmp":
                    return new CompareFilesCommand(input, data, this.judge, this.repository, this.inputOutputManeger);

                case "cdRel":
                    return new ChangeRelativePathCommand(input, data, this.judge, this.repository, this.inputOutputManeger);

                case "cdAbs":
                    return new ChangeAbsolutePathCommand(input, data, this.judge, this.repository, this.inputOutputManeger);

                case "readdb":
                    return new ReadDatabaseCommand(input, data, this.judge, this.repository, this.inputOutputManeger);

                case "help":
                    return new GetHelpCommand(input, data, this.judge, this.repository, this.inputOutputManeger);

                case "filter":
                    return new PrintFilteredStudentsCommand(input, data, this.judge, this.repository, this.inputOutputManeger);

                case "order":
                    return new PrintOrderedStudentsCommand(input, data, this.judge, this.repository, this.inputOutputManeger);

                case "download":
                    //TODO
                    throw new InvalidCommandException(input);
                case "downloadAsynch":
                    //TODO
                    throw new InvalidCommandException(input);
                  
                case "show":
                    return new ShowCourseCommand(input, data, this.judge, this.repository, this.inputOutputManeger);

                case "dropdb":
                    return new DropDatabaseCommand(input, data, this.judge, this.repository, this.inputOutputManeger);

                default:
                    throw new InvalidCommandException(input);
            }
        }
    }
}