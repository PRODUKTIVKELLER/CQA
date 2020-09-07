using System;
using System.Collections.Generic;
using System.IO;
using Produktivkeller.CodeQualityAssurance.Editor.AsciiDoc.Model;
using Produktivkeller.CodeQualityAssurance.Editor.Common;
using Produktivkeller.CodeQualityAssurance.Editor.JqAssistant;
using UnityEditor;

namespace Produktivkeller.CodeQualityAssurance.Editor.AsciiDoc
{
    public class AsciiDocTemplater
    {
        private readonly List<Group> _groupList;
        private readonly Dictionary<Rule, bool> _ruleCheckboxes;
        private readonly Dictionary<Group, bool> _groupCheckboxes;

        private string _indexTemplate;
        private string _relevantGroupsForCommandLine;

        public AsciiDocTemplater(Dictionary<Group, bool> groupCheckboxes, Dictionary<Rule, bool> ruleCheckboxes)
        {
            _relevantGroupsForCommandLine = "";
            
            _ruleCheckboxes = ruleCheckboxes;
            _groupCheckboxes = groupCheckboxes;

            _groupList = RuleDao.Instance.GetAllGroups();
        }

        public string ReturnRelevantGroupsForCommandLine()
        {
            return _relevantGroupsForCommandLine;
        }

        private void AddRelevantGroupForCommandLine(string group)
        {
            if (_relevantGroupsForCommandLine.Length > 0)
            {
                _relevantGroupsForCommandLine += ",";
            }

            _relevantGroupsForCommandLine += group;
        }

        public void PrepareAnalysis()
        {
            RecreateOutputFolder();
            BuildIndexAdoc();
            WriteIndexAdoc();
        }

        private void PutRulesIntoTemplate()
        {
            string include = "";

            foreach (Group group in _groupList)
            {
                if (_groupCheckboxes[group])
                {
                    AddRelevantGroupForCommandLine(group.key);
                    
                    include += BuildGroupDeclaration(group);
                    include += BuildAllRulesOfGroup(group);
                    continue;
                }

                bool first = true;
                foreach (Rule rule in group.rules)
                {
                    if (_ruleCheckboxes[rule])
                    {
                        if (first)
                        {
                            AddRelevantGroupForCommandLine(group.key);
                            
                            include += BuildGroupDeclaration(group);
                            first = false;
                        }

                        include += BuildSingleRule(group, rule);
                    }
                }
            }

            _indexTemplate = _indexTemplate.Replace("{{INCLUDE}}", include);
        }

        private string BuildAllRulesOfGroup(Group group)
        {
            string include = "";

            foreach (Rule rule in group.rules)
            {
                include += BuildSingleRule(group, rule);
            }

            return include;
        }

        private string BuildSingleRule(Group group, Rule rule)
        {
            return "[[" + group.key + ":" + rule.key + "]]\n" +
                   "." + rule.description + "\n" +
                   "[source,cypher,role=" + (rule.type == RuleType.Concept ? "concept" : "constraint") +
                   ",rowCountMax=0]\n" +
                   "----\n" +
                   "" + rule.cypherQuery + "\n" +
                   "----\n" +
                   "\n";
        }

        private string BuildGroupDeclaration(Group group)
        {
            return "[[" + group.key + "]]\n" +
                   "." + group.name + "\n" +
                   "[role=group,includesConstraints=\"" + group.key + ":*\",includesConcepts=\"" + group.key + ":*\"]" +
                   "\n" +
                   "\n" +
                   "== " + group.name + "\n" +
                   "\n";
        }

        private void WriteIndexAdoc()
        {
            string path = Path.Combine(JqaPaths.Instance.BuildJqaHtmlFinishedTemplatesPath(), "index.adoc");

            File.WriteAllLines(path,
                _indexTemplate.Split(new string[] {"\n", "\r\n"}, StringSplitOptions.None));
        }

        private void BuildIndexAdoc()
        {
            _indexTemplate = FileReader.ReadIndexAsciidoc();

            ManageMetadata();
            PutRulesIntoTemplate();
        }

        private void ManageMetadata()
        {
            _indexTemplate = _indexTemplate.Replace("{{PROJECT}}", PlayerSettings.productName);
            _indexTemplate = _indexTemplate.Replace("{{DESCRIPTION}}", "Report generated at " + DateTime.Now + ".");
        }

        private static void RecreateOutputFolder()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(JqaPaths.Instance.BuildJqaHtmlFinishedTemplatesPath());
            if (directoryInfo.Exists)
            {
                Directory.Delete(directoryInfo.FullName, true);
            }

            Directory.CreateDirectory(directoryInfo.FullName);
        }
    }
}