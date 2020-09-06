using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Logging;
using UnityEngine;

namespace Editor._common
{
    public abstract class FileReader
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static List<FileInfo> FindBuiltInAsciidocFiles()
        {
            return FindAsciidocFiles(Path.Combine(Application.dataPath, "_rules"));
        }
        
        public static List<FileInfo> FindAsciidocFiles(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            List<FileInfo> fileInfos = new List<FileInfo>();
            try
            {
                fileInfos = directoryInfo.GetFiles("*.adoc").ToList();
            }
            catch (UnauthorizedAccessException e)
            {
                Log.Error(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Log.Error(e.Message);
            }

            return fileInfos;
        }

        public static string ReadIndexAsciidoc()
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(Application.dataPath, "_rules", "index.adoc"));
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