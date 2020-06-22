using System;
using System.Collections.Generic;

namespace Logging
{
    public class LogFormatter
    {
        private string dateFormatString;
        private string logFormatString;
        private readonly string placeholder;

        public LogFormatter(string dateFormatString, string logFormatString, string placeholder)
        {
            this.dateFormatString = dateFormatString;
            this.logFormatString = logFormatString;
            this.placeholder = placeholder;
        }

        public string Format(string logMessage, params object[] args)
        {
            int placeholderCount = DeterminePlaceholderCount(logMessage);

            if (placeholderCount != args.Length)
            {
                throw new ArgumentException(String.Format("The number of passed parameters [{0}] does not match the number of placeholders [{1}].", args.Length, placeholderCount));
            }

            List<string> messageParts = new List<string>();

            for (int i = 0; i < placeholderCount; i++)
            {
                int placeholderPosition = logMessage.IndexOf(this.placeholder);

                messageParts.Add(logMessage.Substring(0, placeholderPosition));

                messageParts.Add(Format(args[i]));

                logMessage = logMessage.Substring(placeholderPosition + this.placeholder.Length);
            }

            messageParts.Add(logMessage);

            return String.Join(String.Empty, messageParts.ToArray());
        }

        private string Format(object arg)
        {
            bool isMonoBehaviour = arg.GetType().IsSubclassOf(typeof(UnityEngine.MonoBehaviour));

            string prefix = isMonoBehaviour ? "'" : "[";
            string suffix = isMonoBehaviour ? "'" : "]";
            
            return prefix + arg.ToString() + suffix;
        }

        private int DeterminePlaceholderCount(string logMessage)
        {
            return logMessage.Split(new string[] { this.placeholder }, StringSplitOptions.None).Length - 1;
        }

        public string ApplyTimestampAndClassname(string logMessage, DateTime timestamp, string className)
        {
            string timestampAsString = DateTime.Now.ToString(this.dateFormatString);
            return String.Format(this.logFormatString, timestampAsString, className, logMessage);
        }
    }
}
