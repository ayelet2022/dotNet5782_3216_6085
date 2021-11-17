using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IDAL.DO
{
    [Serializable]
    public class ExistsException : Exception
    {
        public ExistsException() : base() {}
        public ExistsException(string message) : base(message) {}
        public ExistsException(string message,Exception inner) : base(message, inner) {}
    }
}
