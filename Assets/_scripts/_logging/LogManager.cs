using System;
using System.Collections.Generic;

namespace Logging
{
    public static class LogManager
    {
        private static readonly LogLevel logLevel = LogLevel.DEBUG;
        private static readonly string dateFormatString = "yyyy.MM.dd HH:mm:ss,ffff";
        private static readonly string logFormatString = "{0} [{1}] {2}";
        private static readonly string placeholder = "{}";

        private static readonly IOutput output = new UnityLogOutput();
        private static readonly LogFormatter logFormatter = new LogFormatter(dateFormatString, logFormatString, placeholder);

        private static Dictionary<Type, ILog> loggers = new Dictionary<Type, ILog>();

        public static ILog GetLogger(Type type)
        {
            if (!loggers.ContainsKey(type))
            {
                loggers[type] = new Log(type, logLevel, output, logFormatter);
            }

            return loggers[type];
        }
    }
}
