using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    class MyException
    {
        public MyException() : base() {}
        public MyException(string message) : base(message) {}
        public MyException(string message,Exception inner) : base(message, inner) {}
        string doubleException()
        {
            return "double";
        }
    }
}
