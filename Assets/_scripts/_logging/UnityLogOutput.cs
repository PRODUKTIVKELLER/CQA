
namespace Logging
{
    public class UnityLogOutput : IOutput
    {
        public void Write(LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.DEBUG:
                    UnityEngine.Debug.Log(message);
                    break;
                case LogLevel.WARN:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case LogLevel.ERROR:
                    UnityEngine.Debug.LogError(message);
                    break;
            }
        }
    }
}
