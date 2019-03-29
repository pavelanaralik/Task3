using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Start");
            Regex regex = new Regex("^signature_epam_com");
            var directory = new DirectoryInfo("C:\\Users\\Pavel_Anaralik\\Downloads");
            var fileSystemVisitor = new FileSystemVisitor(directory, fileSystemInfo => regex.IsMatch(fileSystemInfo.Name));

            fileSystemVisitor.Start += (s, e) => { Console.WriteLine(e.Message);};
            fileSystemVisitor.Finish += (s, e) => { Console.WriteLine(e.Message);};
            fileSystemVisitor.FileFinded += (s, e) => { Console.WriteLine(e.Message); };
            fileSystemVisitor.DirectoryFinded += (s, e) =>{Console.WriteLine(e.Message);};

            fileSystemVisitor.FilteredFileFinded += (s, e) =>
            {
                Console.WriteLine(e.Message);
                Act(s);
            };

            fileSystemVisitor.FilteredDirectoryFinded += (s, e) => 
            {
                Console.WriteLine(e.Message);
                Act(s);
            };

            var test = fileSystemVisitor.GetFileSystemInfoSequence().ToList();

            foreach (var info in test)
            {
                Console.WriteLine($"   : {info.FullName}");
            }
        }

        private static void Act(object sender)
        {
            if (sender is FileSystemVisitor visitor)
            {
                Console.WriteLine("Press <Escape> to exit...");
                Console.WriteLine("Press <S> to skip...");
                Console.WriteLine("To continue, press any key...");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Escape:
                        visitor.IsStopped = true;
                        break;

                    case ConsoleKey.S:
                        visitor.IsExcluded = true;
                        break;

                    default:
                        visitor.IsExcluded = false;
                        break;
                }
            }
        }
    }
}
