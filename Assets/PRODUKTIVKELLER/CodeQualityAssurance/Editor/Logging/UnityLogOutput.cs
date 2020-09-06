
using System;

namespace Produktivkeller.CodeQualityAssurance.Editor.Logging
{
    public class UnityLogOutput : IOutput
    {
        public void Write(LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    UnityEngine.Debug.Log(message);
                    break;
                case LogLevel.Warn:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case LogLevel.Error:
                    UnityEngine.Debug.LogError(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }
    }
}
