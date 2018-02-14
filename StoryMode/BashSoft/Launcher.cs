using System;

namespace BashSoft
{
    class Launcher
    {
        public static void Main(string[] args)
        {
            Tester tester = new Tester();
            IOManager ioManeger = new IOManager();
            StudentsRepository repo = new StudentsRepository(new RepositorySorter(), new RepositoryFilter());

            CommandInterpreter currentInterpreter = new CommandInterpreter(tester, repo, ioManeger);

            InputReader reader = new InputReader(currentInterpreter);

            reader.StartReadingCommands();

        }
    }
}
