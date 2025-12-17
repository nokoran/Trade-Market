using System;

namespace Business.Validation
{
    [Serializable]
    public class MarketException : Exception
    { 
        public MarketException() : base() { }
        public MarketException(string message) : base(message) { }
        public MarketException(string message, Exception inner) : base(message, inner) { }
        protected MarketException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}