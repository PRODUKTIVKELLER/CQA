using System;
using System.Collections.Generic;

namespace Produktivkeller.CodeQualityAssurance.Editor.Logging
{
    public class LogFormatter
    {
        private readonly string _dateFormatString;
        private readonly string _logFormatString;
        private readonly string _placeholder;

        public LogFormatter(string dateFormatString, string logFormatString, string placeholder)
        {
            _dateFormatString = dateFormatString;
            _logFormatString = logFormatString;
            _placeholder = placeholder;
        }

        public string Format(string logMessage, params object[] args)
        {
            int placeholderCount = DeterminePlaceholderCount(logMessage);

            if (placeholderCount != args.Length)
            {
                throw new ArgumentException(
                    $"The number of passed parameters [{args.Length}] does not match the number of placeholders [{placeholderCount}].");
            }

            List<string> messageParts = new List<string>();

            for (int i = 0; i < placeholderCount; i++)
            {
                int placeholderPosition = logMessage.IndexOf(_placeholder, StringComparison.Ordinal);

                messageParts.Add(logMessage.Substring(0, placeholderPosition));

                messageParts.Add(Format(args[i]));

                logMessage = logMessage.Substring(placeholderPosition + _placeholder.Length);
            }

            messageParts.Add(logMessage);

            return String.Join(String.Empty, messageParts.ToArray());
        }

        private static string Format(object arg)
        {
            bool isMonoBehaviour = arg.GetType().IsSubclassOf(typeof(UnityEngine.MonoBehaviour));

            string prefix = isMonoBehaviour ? "'" : "[";
            string suffix = isMonoBehaviour ? "'" : "]";
            
            return prefix + arg + suffix;
        }

        private int DeterminePlaceholderCount(string logMessage)
        {
            return logMessage.Split(new[] { _placeholder }, StringSplitOptions.None).Length - 1;
        }

        public string ApplyTimestampAndClassname(string logMessage, string className)
        {
            string timestampAsString = DateTime.Now.ToString(_dateFormatString);
            return string.Format(_logFormatString, timestampAsString, className, logMessage);
        }
    }
}
