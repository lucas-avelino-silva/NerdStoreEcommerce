using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNE.Core.Messages
{
    public abstract class Message
    {
        public Message()
        {
            MessageType = GetType().Name;
        }

        public string MessageType { get; protected set; }

        public Guid AggregateId { get; protected set; }
    }
}
