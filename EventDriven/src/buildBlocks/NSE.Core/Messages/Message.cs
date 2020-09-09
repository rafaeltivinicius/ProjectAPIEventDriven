using System;

namespace NSE.Core.Messages
{
    public abstract class Message
    {
        public string MessageType { get; protected set; }
        public Guid AggregateId { get; protected set; } //raiz de agregação 

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
