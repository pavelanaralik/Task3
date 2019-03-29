using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3.EventArgs
{
    public class FindedEventArgs<T>: System.EventArgs where T: FileSystemInfo
    {
        public string Message { get; }

        public FindedEventArgs(FileSystemInfo fileSystemInfo)
        {
            if (typeof(T) == typeof(DirectoryInfo))
            {
                Message = $"Directory \"{fileSystemInfo.Name}\" was found.";
            }
            if (typeof(T) == typeof(FileInfo))
            {
                Message = $"File \"{fileSystemInfo.Name}\" was found.";
            }
        }
    }
}
