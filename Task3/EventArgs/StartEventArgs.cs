using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3.EventArgs
{
    public class StartEventArgs : System.EventArgs
    {
        public string Message { get; }

        public StartEventArgs(string message)
        {
            Message = message;
        }
    }
}
