using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Logging.Log4Net
{
    public interface ILoggerService
    {
        void Info(object logMessage);
        void Error(object logMessage);
        void Debug(object logMessage);
        void Warn(object logMessage);
        void Fatal(object logMessage);
    }
}
