using System.Diagnostics;
using System.Threading;

namespace GameEngineConcept.Components
{
    //abstract base class for component messages
    public abstract class Message
    {
        public StackTrace StackTrace {get; private set;}
        public Thread SenderThread {get; private set; }

        public Message()
        {
            StackTrace = new StackTrace(1, true);
            SenderThread = Thread.CurrentThread;
        }
    }
}
