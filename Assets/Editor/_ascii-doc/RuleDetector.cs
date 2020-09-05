using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Editor._common;
using Editor._model;
using Logging;
using UnityEngine;

namespace Editor
{
    public class RuleDetector
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Dictionary<Group, List<Rule>> DetectRules()
        {
            List<FileInfo> fileInfoList =
                TryToFindRulesIn(new DirectoryInfo(Path.Combine(Application.dataPath, "_rules")));

            if (fileInfoList.Count == 0)
            {
                return new Dictionary<Group, List<Rule>>();
            }

            Dictionary<Group, List<Rule>> result = new Dictionary<Group, List<Rule>>();

            foreach (FileInfo fileInfo in fileInfoList)
            {
                string text = FileReader.TryToReadFile(fileInfo);

                IEnumerable<int> ruleOrGroupIndexes = IndexOfAll(text, "[[");

                Group currentGroup = null;

                foreach (int index in ruleOrGroupIndexes)
                {
                    int declarationStartIndex = index + 2;
                    int declarationEndIndex = text.IndexOf("]]", declarationStartIndex, StringComparison.Ordinal);
                    int declarationLength = declarationEndIndex - declarationStartIndex;

                    string declaration =
                        text.Substring(
                            declarationStartIndex,
                            declarationLength
                        );

                    int descriptionStartIndex = text.IndexOf(".", declarationEndIndex, StringComparison.Ordinal) + 1;
                    int descriptionEndIndex = text.IndexOf("\n", descriptionStartIndex, StringComparison.Ordinal);
                    int descriptionLength = descriptionEndIndex - descriptionStartIndex;

                    string description =
                        text.Substring(
                            descriptionStartIndex,
                            descriptionLength
                        );

                    if (declaration.Contains(":"))
                    {
                        result[currentGroup].Add(new Rule
                        {
                            Name = declaration,
                            Description = description
                        });
                    }
                    else
                    {
                        currentGroup = new Group()
                        {
                            Name = declaration,
                            Description = description
                        };

                        result[currentGroup] = new List<Rule>();
                    }
                }
            }

            return result;
        }


        private static List<FileInfo> TryToFindRulesIn(DirectoryInfo directoryInfo)
        {
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


        private static IEnumerable<int> IndexOfAll(string source, string subString)
        {
            List<int> indexes = new List<int>();
            int index = 0;

            while (true)
            {
                index = source.IndexOf(subString, index, StringComparison.Ordinal);

                if (index == -1)
                {
                    break;
                }

                indexes.Add(index);
                index++;
            }

            return indexes;
        }
    }
}