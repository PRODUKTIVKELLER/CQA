using System.IO;
using System.Reflection;
using Logging;

namespace Editor._common
{
    public abstract class FileReader
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string TryToReadFile(FileInfo fileInfo)
        {
            StreamReader reader = new StreamReader(fileInfo.FullName);
            try
            {
                return reader.ReadToEnd();
            }
            catch (IOException e)
            {
                Log.Error("The file '{}' could not be read:\n\n{}", fileInfo.FullName, e.Message);
            }
            finally
            {
                reader.Close();
            }

            return "";
        }
    }
}