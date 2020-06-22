using System;

namespace Logging
{
    public class Log : ILog
    {
        private string className;
        private LogLevel configuredLogLevel;
        private IOutput output;
        private LogFormatter logFormatter;

        public Log(Type type, LogLevel logLevel, IOutput output, LogFormatter logFormatter)
        {
            this.className = type.FullName;

            this.configuredLogLevel = logLevel;
            this.output = output;
            this.logFormatter = logFormatter;
        }

        public void Debug(string message, params object[] args)
        {
            Write(LogLevel.DEBUG, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            Write(LogLevel.WARN, message, args);
        }

        public void Error(string message, params object[] args)
        {
            Write(LogLevel.ERROR, message, args);
        }

        private void Write(LogLevel logLevelOfMessage, string logMessage, params object[] args)
        {
            if (!logLevelOfMessage.IsAtLeastAsHighAs(this.configuredLogLevel))
            {
                return;
            }

            if (logMessage == null)
            {
                return;
            }

            logMessage = logFormatter.Format(logMessage, args);
            logMessage = logFormatter.ApplyTimestampAndClassname(logMessage, DateTime.Now, this.className);

            this.output.Write(logLevelOfMessage, logMessage);
        }
    }
}