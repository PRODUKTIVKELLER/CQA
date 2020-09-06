namespace Produktivkeller.CodeQualityAssurance.Editor.Logging
{
    public enum LogLevel
    {
        Debug = 1,
        Warn = 2,
        Error = 3
    }

    public static class Extension
    {
        public static bool IsAtLeastAsHighAs(this LogLevel logLevel, LogLevel other)
        {
            return (int) logLevel >= (int) other;
        }
    }
}