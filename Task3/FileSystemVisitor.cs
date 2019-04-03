using System;
using System.Collections.Generic;
using System.IO;
using Task3.EventArgs;

namespace Task3
{
    class FileSystemVisitor
    {
        private readonly Func<FileSystemInfo, bool> _filter;

        public FileSystemVisitor()
        {
        }

        public FileSystemVisitor(Func<FileSystemInfo, bool> filter) : this ()
        {
            _filter = filter;
        }

        public IEnumerable<FileSystemInfo> GetFileSystemInfoSequence(DirectoryInfo startDirectoryInfo)
        {
            if(!startDirectoryInfo.Exists)
                throw new DirectoryNotFoundException();

            Start?.Invoke(this, new StartEventArgs());

            foreach (var item in GetSequence(startDirectoryInfo))
            {
                yield return item;
            }

            Finish?.Invoke(this, new FinishEventArgs());
        }

        private IEnumerable<FileSystemInfo> GetSequence(DirectoryInfo itemDirectoryInfo)
        {
            var fileSystemInfos = itemDirectoryInfo.EnumerateFileSystemInfos();

            foreach (var fileSystemInfo in fileSystemInfos)
            {
                if (fileSystemInfo is FileInfo fileInfo)
                {
                    switch (VisitFile(fileInfo))
                    {
                        case Action.Next:
                            yield return fileSystemInfo;
                            break;
                        case Action.Exclude:
                            continue;
                        case Action.StopSearch:
                            yield break;
                    }
                }
                                                                     
                if (fileSystemInfo is DirectoryInfo directoryInfo)
                {
                    foreach (var item in GetSequence(directoryInfo))
                    {
                        yield return item;
                    }

                    switch (VisitDirectory(directoryInfo))
                    {            
                        case Action.Next:
                            yield return fileSystemInfo;
                            break;
                        case Action.Exclude:
                            continue;
                        case Action.StopSearch:
                            yield break;
                    }
                }
            }
        }

        private Action VisitFile(FileInfo fileInfo)
        {
            return Visit(fileInfo, FileFound, FilteredFileFound, _filter);
        }

        private Action VisitDirectory(DirectoryInfo directoryInfo)
        {
            return Visit(directoryInfo, DirectoryFound, FilteredDirectoryFound, _filter);
        }

        public Action Visit<T>(T fileSystemInfo, EventHandler<FindedEventArgs<T>> itemFound, EventHandler<FindedEventArgs<T>> filteredItemFound, Func<FileSystemInfo, bool> filter) where T : FileSystemInfo
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

        public event EventHandler<StartEventArgs> Start;
        public event EventHandler<FinishEventArgs> Finish;
        public event EventHandler<FindedEventArgs<FileInfo>> FileFound;
        public event EventHandler<FindedEventArgs<DirectoryInfo>> DirectoryFound;
        public event EventHandler<FindedEventArgs<FileInfo>> FilteredFileFound;
        public event EventHandler<FindedEventArgs<DirectoryInfo>> FilteredDirectoryFound;
    }
}
