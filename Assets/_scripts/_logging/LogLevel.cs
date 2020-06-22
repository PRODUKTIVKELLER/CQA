
namespace Logging
{
    public enum LogLevel
    {
        DEBUG = 1,
        WARN = 2,
        ERROR = 3
    }

    public static class Extension
    {
        public static bool IsAtLeastAsHighAs(this LogLevel logLevel, LogLevel other)
        {
            return (int) logLevel >= (int) other;
        }
    }
}