using System;
using System.IO;
using Task3.EventArgs;

namespace Task3
{
    public class FileSystemFilter : IFileSystemFilter
    {
        public Action FilterOut<T>(T fileSystemInfo, EventHandler<FindedEventArgs<T>> itemFound, EventHandler<FindedEventArgs<T>> filteredItemFound, Func<FileSystemInfo, bool> filter) where T : FileSystemInfo
        {
            var e = new FindedEventArgs<T>(fileSystemInfo);

            if (itemFound != null)
            {
                itemFound.Invoke(fileSystemInfo, e);
                if (e.Action == Action.Exclude || e.Action == Action.StopSearch)
                    return e.Action;
            }

            if (!(filter?.Invoke(fileSystemInfo) ?? true))
                return Action.Exclude;

            if (filter != null && filteredItemFound != null)
            {
                filteredItemFound.Invoke(fileSystemInfo, e);
                if (e.Action == Action.Exclude || e.Action == Action.StopSearch)
                    return e.Action;
            }

            return Action.Next;
        }
    }
}
