using System;
using System.Collections.Generic;
using System.IO;
using Task3.EventArgs;

namespace Task3
{
    public class FileSystemVisitor
    {
        private readonly Func<FileSystemInfo, bool> _filter;
        private readonly IFileSystemFilter _fileSystemFilter = new FileSystemFilter();

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
                    switch (FilterOutFile(fileInfo))
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

                    switch (FilterOutDirectory(directoryInfo))
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

        private Action FilterOutFile(FileInfo fileInfo)
        {
            return _fileSystemFilter.FilterOut(fileInfo, FileFound, FilteredFileFound, _filter);
        }

        private Action FilterOutDirectory(DirectoryInfo directoryInfo)
        {
            return _fileSystemFilter.FilterOut(directoryInfo, DirectoryFound, FilteredDirectoryFound, _filter);
        }
       
        public event EventHandler<StartEventArgs> Start;
        public event EventHandler<FinishEventArgs> Finish;
        public event EventHandler<FindedEventArgs<FileInfo>> FileFound;
        public event EventHandler<FindedEventArgs<DirectoryInfo>> DirectoryFound;
        public event EventHandler<FindedEventArgs<FileInfo>> FilteredFileFound;
        public event EventHandler<FindedEventArgs<DirectoryInfo>> FilteredDirectoryFound;
    }
}
