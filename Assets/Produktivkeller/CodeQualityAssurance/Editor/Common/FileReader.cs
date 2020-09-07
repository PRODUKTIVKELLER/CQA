using System.IO;
using System.Reflection;
using Produktivkeller.CodeQualityAssurance.Editor.JqAssistant;
using Produktivkeller.CodeQualityAssurance.Editor.Logging;

namespace Produktivkeller.CodeQualityAssurance.Editor.Common
{
    public abstract class FileReader
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string ReadIndexAsciidoc()
        {
            FileInfo fileInfo = new FileInfo(JqaPaths.Instance.BuildIndexAdocTemplatePath());
            return TryToReadFile(fileInfo);
        }

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