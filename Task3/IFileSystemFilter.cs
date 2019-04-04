using System;
using System.IO;
using Task3.EventArgs;

namespace Task3
{
    public interface IFileSystemFilter
    {
        Action FilterOut<T>(T fileSystemInfo, EventHandler<FindedEventArgs<T>> itemFound,
            EventHandler<FindedEventArgs<T>> filteredItemFound, Func<FileSystemInfo, bool> filter)
            where T : FileSystemInfo;
    }
}
