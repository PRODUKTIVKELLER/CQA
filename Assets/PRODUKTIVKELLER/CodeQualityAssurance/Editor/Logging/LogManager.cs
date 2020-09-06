using System;
using System.Collections.Generic;

namespace Produktivkeller.CodeQualityAssurance.Editor.Logging
{
    public static class LogManager
    {
        private const LogLevel LogLevel = Logging.LogLevel.Debug;
        private const string DateFormatString = "yyyy.MM.dd HH:mm:ss,ffff";
        private const string LogFormatString = "{0} [{1}] {2}";
        private const string Placeholder = "{}";

        private static readonly IOutput Output = new UnityLogOutput();

        private static readonly LogFormatter LogFormatter =
            new LogFormatter(DateFormatString, LogFormatString, Placeholder);

        private static readonly Dictionary<Type, ILog> Loggers = new Dictionary<Type, ILog>();

        public static ILog GetLogger(Type type)
        {
            if (!Loggers.ContainsKey(type))
            {
                Loggers[type] = new Log(type, LogLevel, Output, LogFormatter);
            }

            return Loggers[type];
        }
    }
}