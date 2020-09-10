using MediatR;
using System;

namespace NSE.Core.Messages
{
    // evento fala de algo q já aconteceu (passado)
    public class Event : Message, INotification //interface de marcação
    {
        public DateTime Timestamp { get; private set; }

        public Event()
        {
            Timestamp = DateTime.Now;
        }
        
    }
}
