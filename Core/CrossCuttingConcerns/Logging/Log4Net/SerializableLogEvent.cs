using log4net.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Logging.Log4Net
{
    [Serializable]
    public class SerializableLogEvent
    {
        private LoggingEvent _loggingEvent;

        public SerializableLogEvent(LoggingEvent loggingEvent)
        {
            _loggingEvent = loggingEvent;
        }

        public object UserName => _loggingEvent.UserName;
        public object Identity => _loggingEvent.Identity;
        public object Message => _loggingEvent.MessageObject;
        public object Date => _loggingEvent.TimeStamp.ToString("G");
        public object Properties => _loggingEvent.Properties;


    }
}
