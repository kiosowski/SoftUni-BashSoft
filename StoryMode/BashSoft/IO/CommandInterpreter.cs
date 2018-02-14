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
                    break;
                case "cmp":
                    return new CompareFilesCommand(input, data, this.judge, this.repository, this.inputOutputManeger);
                    break;
                case "cdRel":
                    return new ChangeRelativePathCommand(input, data, this.judge, this.repository, this.inputOutputManeger);
                    break;
                case "cdAbs":
                    return new ChangeAbsolutePathCommand(input, data, this.judge, this.repository, this.inputOutputManeger);
                    break;
                case "readDb":
                    return new ReadDatabaseCommand(input, data, this.judge, this.repository, this.inputOutputManeger);
                    break;
                case "help":
                    return new GetHelpCommand(input, data, this.judge, this.repository, this.inputOutputManeger);
                    break;
                case "filter":
                    return new PrintFilteredStudentsCommand(input, data, this.judge, this.repository, this.inputOutputManeger);
                    break;
                case "order":
                    return new PrintOrderedStudentsCommand(input, data, this.judge, this.repository, this.inputOutputManeger);
                    break;
                case "decOrder":
                    //todo
                    break;
                case "download":
                    //todo
                    break;
                case "downloadAsynch":
                    //todo
                    break;
                case "show":
                    return new ShowCourseCommand(input, data, this.judge, this.repository, this.inputOutputManeger);
                    break;
                case "dropdb":
                    return new DropDatabaseCommand(input, data, this.judge, this.repository, this.inputOutputManeger);
                    break;
                default:
                    throw new InvalidCommandException(input);
                    
            }
        }

        private void TryDropDb(string input, string[] data)
        {
            if (data.Length != 1)
            {
                this.InvalidCommandException(input);
                return;
            }
            this.repository.UnloadData();
            OutputWriter.WriteMessageOnNewLine("Database dropped!");
        }

        private void TryOrderAndTake(string input, string[] data)
        {
            if (data.Length == 5)
            {
                string courseName = data[1];
                string orderBy = data[2].ToLower();
                string command = data[3].ToLower();
                string quantity = data[4].ToLower();

                TryParseParametersForOrderAndTake(command, orderBy, courseName, quantity);
            }
            else
            {
                InvalidCommandException(input);
            }
        }

        private void TryParseParametersForOrderAndTake(string command, string orderBy, string courseName, string quantity)
        {
            if (command == "take")
            {
                if (quantity == "all")
                {
                    this.repository.OrderAndTake(courseName, orderBy);
                }
                else
                {
                    int studentsToTake;
                    bool hasParsed = int.TryParse(quantity, out studentsToTake);
                    if (hasParsed)
                    {
                        this.repository.OrderAndTake(courseName, orderBy, studentsToTake);
                    }
                    else
                    {
                        OutputWriter.DisplayException(ExceptionMessages.InvalidTakeQuantityParameter);
                    }
                }
            }
        }

        private void TryFilterAndTake(string input, string[] data)
        {
            if (data.Length == 5)
            {
                string courseName = data[1];
                string filter = data[2].ToLower();
                string command = data[3].ToLower();
                string quantity = data[4].ToLower();

                TryParseParametersFromFilterAndTake(command, quantity, courseName, filter);
            }
            else
            {
                InvalidCommandException(input);
            }
        }

        private void TryParseParametersFromFilterAndTake(string command, string quantity, string courseName, string filter)
        {
            if (command == "take")
            {
                if (quantity == "all")
                {
                    this.repository.FilterAndTake(courseName, filter);
                }
                else
                {
                    int studentsToTake;
                    bool hasParsed = int.TryParse(quantity, out studentsToTake);
                    if (hasParsed)
                    {
                        this.repository.FilterAndTake(courseName, filter, studentsToTake);
                    }
                    else
                    {
                        OutputWriter.DisplayException(ExceptionMessages.InvalidTakeQuantityParameter);
                    }
                }
            }
        }

        private void TryShowWantedData(string input, string[] data)
        {
            if (data.Length == 2)
            {
                string courseName = data[1];
                this.repository.GetAllStudentsFromCourse(courseName);
            }
            else if (data.Length == 3)
            {
                string courseName = data[1];
                string userName = data[2];
                this.repository.GetStudentScoresFromCourse(courseName, userName);
            }
            else
            {
                InvalidCommandException(input);
            }
        }

        private void InvalidCommandException(string input)
        {
            throw new InvalidCommandException(input);
        }

        private void TryReadDatabaseFromFile(string input, string[] data)
        {
            if (data.Length == 2)
            {
                string fileName = data[1];
                this.repository.LoadData(fileName);
            }
            else
            {
                this.InvalidCommandException(input);
            }
        }

        private void TryChangePathAbsolute(string input, string[] data)
        {
            if (data.Length == 2)
            {
                string absolutePath = data[1];
                this.inputOutputManeger.ChangeCurrentDirectoryAbsolute(absolutePath);
            }
            else
            {
                this.InvalidCommandException(input);
            }
        }

        private void TryChangePathRelatively(string input, string[] data)
        {
            if (data.Length == 2)
            {
                string relPath = data[1];
                this.inputOutputManeger.ChangeCurrentDirectoryRelative(relPath);
            }
            else
            {
                this.InvalidCommandException(input);
            }
        }

        private void TryCompareFiles(string input, string[] data)
        {
            if (data.Length == 3)
            {
                string firstPath = data[1];
                string secondPath = data[2];

                this.judge.CompareContent(firstPath, secondPath);
            }
            else
            {
                this.InvalidCommandException(input);
            }
        }

        private void TryTraverseFolders(string input, string[] data)
        {
            if (data.Length == 1)
            {
                this.inputOutputManeger.TraverseDirectory(0);
            }
            else if (data.Length == 2)
            {
                int depth;
                bool hasParsed = int.TryParse(data[1], out depth);
                if (hasParsed)
                {
                    this.inputOutputManeger.TraverseDirectory(depth);
                }
                else
                {
                    OutputWriter.DisplayException(ExceptionMessages.UnableToParseNumber);
                }
            }
            else
            {
                this.InvalidCommandException(input);
            }
        }

        private void TryCreateDirectory(string input, string[] data)
        {
            if (data.Length == 2)
            {
                string folderName = data[1];
                this.inputOutputManeger.CreateDirectoryInCurrentFolder(folderName);
            }
            else
            {
                this.InvalidCommandException(input);
            }
        }

        private void TryOpenFile(string input, string[] data)
        {
            if (data.Length == 2)
            {
                string fileName = data[1];
                Process.Start(SessionData.currentPath + "\\" + fileName);
            }
            else
            {
                this.InvalidCommandException(input);
            }
        }
    }
}
