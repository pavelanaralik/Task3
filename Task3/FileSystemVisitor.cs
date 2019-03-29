using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Task3.EventArgs;

namespace Task3
{
    class FileSystemVisitor/*: IEnumerable<FileSystemInfo>*/
    {
        private readonly DirectoryInfo _startDirectoryInfo;
        private readonly Func<FileSystemInfo, bool> _filter;

        public bool IsStopped { get; set; }
        public bool IsExcluded { get; set; }

        public FileSystemVisitor(DirectoryInfo startDirectoryInfo)
        {
            _startDirectoryInfo = startDirectoryInfo;
            IsStopped = false;
            IsExcluded = false;
        }

        public FileSystemVisitor(DirectoryInfo startDirectoryInfo, Func<FileSystemInfo, bool> filter) : this (startDirectoryInfo)
        {
            _filter = filter;
        }

        public IEnumerable<FileSystemInfo> GetFileSystemInfoSequence()
        {
            if(!_startDirectoryInfo.Exists)
                throw new DirectoryNotFoundException();

            Start?.Invoke(this, new StartEventArgs("Start search"));

            foreach (var item in GetSequence(_startDirectoryInfo))
            {
                yield return item;
            }

            Finish?.Invoke(this, new FinishEventArgs("Finish search"));
        }

        private IEnumerable<FileSystemInfo> GetSequence(DirectoryInfo itemDirectoryInfo)
        {
            var fileSystemInfos = itemDirectoryInfo.EnumerateFileSystemInfos(); //TODO: Message if directory is empty

            foreach (var fileSystemInfo in fileSystemInfos)
            {
                if (fileSystemInfo is FileInfo fileInfo)
                {
                    if (Filter(fileInfo))            
                        yield return fileInfo;                        
                }

                if (fileSystemInfo is DirectoryInfo directoryInfo)
                {
                    if (Filter(directoryInfo))
                        yield return directoryInfo;
                        
                    foreach (var item in GetSequence(directoryInfo))
                    {
                        yield return item;
                    }
                }

                if (IsStopped)
                    yield break;
            }
        }

        private bool Filter<T>(T info) where T: FileSystemInfo
        {
            if (typeof(T) == typeof(DirectoryInfo))
                DirectoryFinded?.Invoke(this, new FindedEventArgs<DirectoryInfo>(info));

            if (typeof(T) == typeof(FileInfo))
                FileFinded?.Invoke(this, new FindedEventArgs<FileInfo>(info));

            var result = _filter?.Invoke(info) ?? true;

            if (result)
            {
                if (typeof(T) == typeof(DirectoryInfo))
                    FilteredDirectoryFinded?.Invoke(this, new FindedEventArgs<DirectoryInfo>(info));

                if (typeof(T) == typeof(FileInfo))
                    FilteredFileFinded?.Invoke(this, new FindedEventArgs<FileInfo>(info));

                result = !IsExcluded;
            }

            return result;
        }

        //private bool Filter(DirectoryInfo directoryInfo)
        //{
        //    DirectoryFinded?.Invoke(this, new FindedEventArgs<DirectoryInfo>(directoryInfo));

        //    var result = _filter?.Invoke(directoryInfo) ?? true;

        //    if (result)
        //    {
        //        FilteredDirectoryFinded?.Invoke(this, new FindedEventArgs<DirectoryInfo>(directoryInfo));
        //    }

        //    return result;
        //}

        public event EventHandler<StartEventArgs> Start;
        public event EventHandler<FinishEventArgs> Finish;
        public event EventHandler<FindedEventArgs<FileInfo>> FileFinded;
        public event EventHandler<FindedEventArgs<DirectoryInfo>> DirectoryFinded;
        public event EventHandler<FindedEventArgs<FileInfo>> FilteredFileFinded;
        public event EventHandler<FindedEventArgs<DirectoryInfo>> FilteredDirectoryFinded;
    }
}
