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
    public class FailedFreeADroneFromeTheChargerException : Exception
    {
        public FailedFreeADroneFromeTheChargerException() : base() { }
        public FailedFreeADroneFromeTheChargerException(string message) : base(message) { }
        public FailedFreeADroneFromeTheChargerException(string message, Exception inner) : base(message, inner) { }
    }
    public class FailedToChargeDroneException : Exception
    {
        public FailedToChargeDroneException() : base() { }
        public FailedToChargeDroneException(string message) : base(message) { }
        public FailedToChargeDroneException(string message, Exception inner) : base(message, inner) { }
    }
    public class FailedToPickUpParcelException : Exception
    {
        public FailedToPickUpParcelException() : base() { }
        public FailedToPickUpParcelException(string message) : base(message) { }
        public FailedToPickUpParcelException(string message, Exception inner) : base(message, inner) { }
    }
    public class FailedToDelieverParcelException : Exception
    {
        public FailedToDelieverParcelException() : base() { }
        public FailedToDelieverParcelException(string message) : base(message) { }
        public FailedToDelieverParcelException(string message, Exception inner) : base(message, inner) { }
    }
    public class FailedToScheduledAParcelToADroneException : Exception
    {
        public FailedToScheduledAParcelToADroneException() : base() { }
        public FailedToScheduledAParcelToADroneException(string message) : base(message) { }
        public FailedToScheduledAParcelToADroneException(string message, Exception inner) : base(message, inner) { }
    }
    public class FailToUpdateException : Exception
    {
        public FailToUpdateException() : base() { }
        public FailToUpdateException(string message) : base(message) { }
        public FailToUpdateException(string message, Exception inner) : base(message, inner) { }
    }
}
