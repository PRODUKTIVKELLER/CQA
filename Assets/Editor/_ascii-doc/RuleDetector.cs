using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Editor._common;
using Editor._jqa;
using Editor._model;

namespace Editor
{
    public abstract class RuleDetector
    {
        public static Dictionary<Group, List<Rule>> DetectBuiltInRules()
        {
            List<FileInfo> fileInfoList = FileReader.FindBuiltInAsciidocFiles();
            return DetectRules(fileInfoList);
        }

        public static Dictionary<Group, List<Rule>> DetectLocalRules()
        {
            string path = JqaPaths.BuildLocalRulesPath();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            List<FileInfo> fileInfoList = FileReader.FindAsciidocFiles(path);
            return DetectRules(fileInfoList);
        }

        public static Dictionary<Group, List<Rule>> DetectGlobalRules()
        {
            string path = JqaPaths.BuildGlobalRulesPath();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            List<FileInfo> fileInfoList = FileReader.FindAsciidocFiles(path);
            return DetectRules(fileInfoList);
        }

        public static Dictionary<Group, List<Rule>> DetectRules(List<FileInfo> fileInfoList)
        {
            if (fileInfoList.Count == 0)
            {
                return new Dictionary<Group, List<Rule>>();
            }

            Dictionary<Group, List<Rule>> result = new Dictionary<Group, List<Rule>>();

            foreach (FileInfo fileInfo in fileInfoList)
            {
                string text = FileReader.TryToReadFile(fileInfo);

                List<int> ruleOrGroupIndexes = IndexOfAll(text, "[[").ToList();

                Group currentGroup = null;

                for (int i = 0; i < ruleOrGroupIndexes.Count; i++)
                {
                    int declarationStartIndex = ruleOrGroupIndexes[i] + 2;
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

                    int nextIndex = i < ruleOrGroupIndexes.Count - 1 ? ruleOrGroupIndexes[i + 1] : text.Length - 1;

                    bool isConstraint = text
                        .Substring(declarationStartIndex, nextIndex - declarationStartIndex)
                        .Contains("role=concept");

                    if (declaration.Contains(":"))
                    {
                        result[currentGroup].Add(new Rule
                        {
                            Name = declaration,
                            Description = description,
                            IsConstraint = isConstraint
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