using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace GameEngineConcept.Components
{
    //abstract base class for component messages
    public abstract class Message
    {
        public StackTrace senderStackTrace = new StackTrace(1, true);
        public Thread senderThread = Thread.CurrentThread;
    }
}
