using System;

namespace Produktivkeller.CodeQualityAssurance.Editor.Logging
{
    public class Log : ILog
    {
        private readonly string _className;
        private readonly LogLevel _configuredLogLevel;
        private readonly LogFormatter _logFormatter;
        private readonly IOutput _output;

        public Log(Type type, LogLevel logLevel, IOutput output, LogFormatter logFormatter)
        {
            _className = type.FullName;

            _configuredLogLevel = logLevel;
            _output = output;
            _logFormatter = logFormatter;
        }

        public void Debug(string message, params object[] args)
        {
            Write(LogLevel.Debug, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            Write(LogLevel.Warn, message, args);
        }

        public void Error(string message, params object[] args)
        {
            Write(LogLevel.Error, message, args);
        }

        private void Write(LogLevel logLevelOfMessage, string logMessage, params object[] args)
        {
            if (!logLevelOfMessage.IsAtLeastAsHighAs(_configuredLogLevel))
            {
                return;
            }

            if (logMessage == null)
            {
                return;
            }

            logMessage = _logFormatter.Format(logMessage, args);
            logMessage = _logFormatter.ApplyTimestampAndClassname(logMessage, _className);

            _output.Write(logLevelOfMessage, logMessage);
        }
    }
}