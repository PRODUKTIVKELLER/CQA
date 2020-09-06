﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Produktivkeller.CodeQualityAssurance.Editor.AsciiDoc.Model;
using Produktivkeller.CodeQualityAssurance.Editor.Common;
using Produktivkeller.CodeQualityAssurance.Editor.JqAssistant;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.AsciiDoc
{
    public abstract class RuleDetector
    {
        private static List<Group> DetectRulesAt(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            return directoryInfo.EnumerateFiles("*.json").Select(
                fileInfo =>
                {
                    string text = FileReader.TryToReadFile(fileInfo);
                    return JsonUtility.FromJson<Group>(text);
                }
            ).ToList();
        }

        public static List<Group> DetectLocalRules()
        {
            return DetectRulesAt(JqaPaths.BuildLocalRulesPath());
        }

        public static List<Group> DetectGlobalRules()
        {
            return DetectRulesAt(JqaPaths.BuildGlobalRulesPath());
        }

        public static List<Group> DetectBuiltInRules()
        {
            return DetectRulesAt(JqaPaths.BuildBuiltInRulesPath());
        }
    }
}