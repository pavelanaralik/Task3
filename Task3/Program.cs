using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Task3.EventArgs;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter search line...");
            var str = Console.ReadLine();
            //str = "signature_epam_com";
            Regex regex = new Regex($"^{str}");
            var directory = new DirectoryInfo("C:\\Users\\Pavel_Anaralik\\Downloads");
            //var directory = new DirectoryInfo("C:\\Users\\Pavel_Anaralik\\Downloads\\signature_epam_com\\3");
            var fileSystemVisitor = new FileSystemVisitor(fileSystemInfo => regex.IsMatch(fileSystemInfo.Name));

            fileSystemVisitor.Start += (s, e) => { Console.WriteLine("Start search"); Console.WriteLine(); };
            fileSystemVisitor.Finish += (s, e) => { Console.WriteLine();  Console.WriteLine("Finish search");};
            fileSystemVisitor.FileFound += (s, e) => { Act(e, $"File \"{e.Info.Name}\" found."); };
            fileSystemVisitor.DirectoryFound += (s, e) => { Act(e, $"Directory \"{e.Info.Name}\" found."); };
            fileSystemVisitor.FilteredFileFound += (s, e) => { Act(e, $"Filtered file \"{e.Info.Name}\" found."); };
            fileSystemVisitor.FilteredDirectoryFound += (s, e) => { Act(e, $"Filtered directory \"{e.Info.Name}\" found."); };

            try
            {
                var sequence = fileSystemVisitor.GetFileSystemInfoSequence(directory).ToList();
                Console.WriteLine();
                Console.WriteLine("Sequence: ");
                int i = 0;
                foreach (var info in sequence)
                {
                    Console.WriteLine($"{++i} : {info.FullName}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static void Act<T>(FindedEventArgs<T> e, string message) where T: FileSystemInfo
        {
            Console.WriteLine(message);
            //Console.WriteLine(e.Info.Name);
            Console.WriteLine();
            Console.WriteLine("Press <S> to skip or press <Enter> for continue...");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Escape:
                        e.Action = Action.StopSearch;
                        Console.WriteLine();
                    break;

                    case ConsoleKey.S:
                        e.Action = Action.Exclude;
                        Console.WriteLine();
                    break;

                    default:
                        e.Action = Action.Next;
                        Console.WriteLine("+");
                    break;
                }
        }
    }
}
