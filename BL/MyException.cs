using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    [Serializable]
    public class InvalidInputException : Exception
    {
        public InvalidInputException() : base() { }
        public InvalidInputException(string message) : base(message) { }
        public InvalidInputException(string message, Exception inner) : base(message, inner) { }
    }
    public class NotFoundInputException : Exception
    {
        public NotFoundInputException() : base() { }
        public NotFoundInputException(string message) : base(message) { }
        public NotFoundInputException(string message, Exception inner) : base(message, inner) { }
    }
    public class FailedToAddException : Exception
    {
        public FailedToAddException() : base() { }
        public FailedToAddException(string message) : base(message) { }
        public FailedToAddException(string message, Exception inner) : base(message, inner) { }
    }
    public class FailedFreeADroneFromeTheCharger : Exception
    {
        public FailedFreeADroneFromeTheCharger() : base() { }
        public FailedFreeADroneFromeTheCharger(string message) : base(message) { }
        public FailedFreeADroneFromeTheCharger(string message, Exception inner) : base(message, inner) { }
    }
}
