using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3.EventArgs
{
    class FinishEventArgs : System.EventArgs
    {
        public string Message { get; }

        public FinishEventArgs(string message)
        {
            Message = message;
        }
    }
}
