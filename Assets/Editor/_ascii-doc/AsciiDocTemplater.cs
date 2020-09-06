using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Editor._common;
using Editor._jqa;
using Editor._model;
using Logging;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class AsciiDocTemplater
    {
        // Example: include::code-style.adoc[]
        private const string FileIncludeTemplate = "include::{{GROUP}}.adoc[]\n";

        // Example: include::jQA:Rules[concepts="code-style:*",constraints="code-style:*"]
        private const string RuleIncludeTemplate =
            "include::jQA:Rules[concepts=\"{{CONCEPTS}}\",constraints=\"{{CONSTRAINTS}}\"]\n";

        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private List<FileInfo> _fileInfoList;
        private readonly Dictionary<Group, List<Rule>> _rulesByGroup;
        private readonly Dictionary<Rule, bool> _ruleCheckboxes;
        private readonly Dictionary<Group, bool> _groupCheckboxes;

        private string _indexTemplate;

        public AsciiDocTemplater(Dictionary<Group, bool> groupCheckboxes, Dictionary<Rule, bool> ruleCheckboxes,
            Dictionary<Group, List<Rule>> rulesByGroup)
        {
            _rulesByGroup = rulesByGroup;
            _ruleCheckboxes = ruleCheckboxes;
            _groupCheckboxes = groupCheckboxes;
        }

        public void PrepareAnalysis()
        {
            RecreateOutputFolder();
            ReadTemplateFiles();
            BuildIndexAdoc();
            WriteIndexAdoc();
            CopyFiles();
        }

        private void WriteIndexAdoc()
        {
            string path = Path.Combine(JqaPaths.BuildJqaHtmlFinishedTemplatesPath(), "index.adoc");

            File.WriteAllLines(path,
                _indexTemplate.Split(new string[] {"\n", "\r\n"}, StringSplitOptions.None));
        }

        private void CopyFiles()
        {
            foreach (Group group in _groupCheckboxes.Keys)
            {
                foreach (Rule rule in _rulesByGroup[group])
                {
                    if (group.key != "common" && _ruleCheckboxes[rule])
                    {
                        CopyFile(group.key + ".adoc");
                        break;
                    }
                }
            }
        }

        private void CopyFile(string fileName)
        {
            string sourcePath = Path.Combine(Application.dataPath, "_rules", fileName);
            string targetPath = Path.Combine(JqaPaths.BuildJqaHtmlFinishedTemplatesPath(), fileName);
            File.Copy(sourcePath, targetPath);
        }

        private void BuildIndexAdoc()
        {
            _indexTemplate = FileReader.ReadIndexAsciidoc();

            ManageMetadata();
            ManageRules();
            ManageIncludes();
        }

        private void ManageMetadata()
        {
            _indexTemplate = _indexTemplate.Replace("{{PROJECT}}", PlayerSettings.productName);
            _indexTemplate = _indexTemplate.Replace("{{DESCRIPTION}}", "Report generated at " + DateTime.Now + ".");
        }

        private void ManageRules()
        {
            string include = "";

            foreach (Group group in _groupCheckboxes.Keys)
            {
                if (_groupCheckboxes[group])
                {
                    string singleInclude = RuleIncludeTemplate.Replace("{{CONCEPTS}}", group.key + ":*");
                    singleInclude = singleInclude.Replace("{{CONSTRAINTS}}", group.key + ":*");

                    include += singleInclude;
                    continue;
                }

                string constraintsFilter = "";
                string conceptsFilter = "";
                foreach (Rule rule in _rulesByGroup[group])
                {
                    if (_ruleCheckboxes[rule])
                    {
                        if (rule.type == RuleType.Constraint)
                        {
                            if (constraintsFilter.Length > 0)
                            {
                                constraintsFilter += ",";
                            }

                            constraintsFilter += rule.key;
                        }
                        else
                        {
                            if (conceptsFilter.Length > 0)
                            {
                                conceptsFilter += ",";
                            }

                            conceptsFilter += rule.key;
                        }
                    }
                }

                string filterInclude = RuleIncludeTemplate.Replace(
                    "{{CONCEPTS}}", conceptsFilter
                );
                filterInclude = filterInclude.Replace(
                    "{{CONSTRAINTS}}", constraintsFilter
                );

                include += filterInclude;
            }

            _indexTemplate = _indexTemplate.Replace("{{RULES}}", include);
        }

        private void ManageIncludes()
        {
            string include = "";

            foreach (Group group in _groupCheckboxes.Keys)
            {
                if (group.key == "common")
                {
                    continue;
                }

                foreach (Rule rule in _rulesByGroup[group])
                {
                    if (_ruleCheckboxes[rule])
                    {
                        include += FileIncludeTemplate.Replace("{{GROUP}}", group.key);
                        break;
                    }
                }
            }

            _indexTemplate = _indexTemplate.Replace("{{INCLUDE}}", include);
        }


        private void ReadTemplateFiles()
        {
            _fileInfoList = FileReader.FindBuiltInAsciidocFiles();
        }

        private void RecreateOutputFolder()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(JqaPaths.BuildJqaHtmlFinishedTemplatesPath());
            if (directoryInfo.Exists)
            {
                Directory.Delete(directoryInfo.FullName, true);
            }

            Directory.CreateDirectory(directoryInfo.FullName);
        }
    }
}