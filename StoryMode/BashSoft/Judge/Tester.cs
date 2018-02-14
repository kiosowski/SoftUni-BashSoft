using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BashSoft
{
    public class Tester
    {
        public void CompareContent(string userOutputPath, string expectedOutputPath)
        {
            try
            {
                OutputWriter.WriteMessageOnNewLine("Reading files...");
                string mismatchPath = GetMismatchPath(expectedOutputPath);

                string[] actualOutputLines = File.ReadAllLines(userOutputPath);
                string[] expectedOutputLines = File.ReadAllLines(expectedOutputPath);

                bool hasMisMatch;
                string[] mismatches = GetLinesWithPossibleMisMatches(actualOutputLines, expectedOutputLines, out hasMisMatch);

                PrintOutput(mismatches, hasMisMatch, mismatchPath);
                OutputWriter.WriteMessageOnNewLine("Files read!");
            }
            catch(IOException io)
            {
                OutputWriter.DisplayException(io.Message);
            }
        }

        private void PrintOutput(string[] mismatches, bool hasMisMatch, string mismatchPath)
        {
            if (hasMisMatch)
            {
                foreach (var line in mismatches)
                {
                    OutputWriter.WriteMessageOnNewLine(line);
                }
                try
                {
                    File.WriteAllLines(mismatchPath, mismatches);
                }
                catch(DirectoryNotFoundException)
                {
                    OutputWriter.DisplayException(ExceptionMessages.InvalidPath);
                }
                return;
            }
            OutputWriter.WriteMessageOnNewLine("Files are identical. There are no mismatches.");
        }

        private string[] GetLinesWithPossibleMisMatches(string[] actualOutputLines, string[] expectedOutputLines, out bool hasMisMatch)
        {
            hasMisMatch = false;
            string output = string.Empty;
            string[] mismatches = new string[actualOutputLines.Length];
            OutputWriter.WriteMessageOnNewLine("Comparing files...");
            int minOutputLines = actualOutputLines.Length;
            if (actualOutputLines.Length!=expectedOutputLines.Length)
            {
                hasMisMatch = true;
                minOutputLines = Math.Min(actualOutputLines.Length, expectedOutputLines.Length);
                OutputWriter.DisplayException(ExceptionMessages.ComparisonOfFilesWithDifferentSizes);
            }
            for (int index = 0; index < minOutputLines; index++)
            {
                string actualLine = actualOutputLines[index];
                string expectedLine = expectedOutputLines[index];
                if (!actualLine.Equals(expectedLine))
                {
                   
                   output = string.Format("Mismatch at line {0} -- expected: \"{1}\", actual: \"{2}\"", index, expectedLine, actualLine);
                    hasMisMatch = true;
                }
                else
                {
                    output = actualLine;
                    output += Environment.NewLine;
                }
                mismatches[index] = output;
            }
            return mismatches;
        }

        public string GetMismatchPath(string expectedOutputPath)
        {
            int indexOf = expectedOutputPath.LastIndexOf('\\');
            string directoryPath = expectedOutputPath.Substring(0, indexOf);
            string finalPath = directoryPath + @"\Mismatches.txt";
            return finalPath;
        }
    }
}
