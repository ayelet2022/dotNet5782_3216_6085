using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DO
{
    [Serializable]
    public class ExistsException : Exception
    {
        public ExistsException() : base() { }
        public ExistsException(string message) : base(message) { }
        public ExistsException(string message, Exception inner) : base(message, inner) { }
    }
    public class DoesNotExistException : Exception
    {
        public DoesNotExistException() : base() { }
        public DoesNotExistException(string message) : base(message) { }
        public DoesNotExistException(string message, Exception inner) : base(message, inner) { }
    }
    public class NotFoundInputException : Exception
    {
        public NotFoundInputException() : base() { }
        public NotFoundInputException(string message) : base(message) { }
        public NotFoundInputException(string message, Exception inner) : base(message, inner) { }
    }

    public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }

        public override string ToString()
        {
            return base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
        }
    }
}

