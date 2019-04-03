using System.IO;

namespace Task3.EventArgs
{
    public class FindedEventArgs<T>: System.EventArgs where T: FileSystemInfo
    {
        public Action Action { get; set; }
        public FileSystemInfo Info { get; set; }

        public FindedEventArgs(FileSystemInfo info)
        {
            Action = Action.Next;
            Info = info;
        }
    }
}
