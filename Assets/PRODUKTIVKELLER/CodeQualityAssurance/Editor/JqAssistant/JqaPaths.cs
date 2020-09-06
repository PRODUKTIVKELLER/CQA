﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.JqAssistant
{
    // FIXME: Support flexible folder position in project, and either make all methods static or not.
    public class JqaPaths
    {
        private const string CompanyFolderName = "produktivkeller";
        private const string ApplicationFolderName = "unity-code-quality-assurance";
        private const string VersionFolderName = "0.0.1";


        private static string BuildAppDataPath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(
                appDataPath,
                CompanyFolderName,
                ApplicationFolderName,
                VersionFolderName
            );
        }

        public string BuildJqaInstallationPath()
        {
            return Path.Combine(BuildAppDataPath(), "command-line-distribution");
        }

        private string BuildJqaPath()
        {
            return Path.Combine(BuildJqaInstallationPath(), "jqassistant-commandline-neo4jv3-1.8.0");
        }

        private static string BuildJqaDataPath()
        {
            return Path.Combine(BuildAppDataPath(), "data", Application.productName);
        }

        public string BuildJqaStorePath()
        {
            return Path.Combine(BuildJqaDataPath(), "store");
        }

        public string BuildJqaReportPath()
        {
            return Path.Combine(BuildJqaDataPath(), "report");
        }

        public string BuildCqaHistoryPath()
        {
            return Path.Combine(BuildJqaStorePath(), "cqa.json");
        }

        public string BuildJqaHtmlReportPath()
        {
            return Path.Combine(BuildJqaReportPath(), "asciidoc", "index.html");
        }

        public static string BuildJqaHtmlFinishedTemplatesPath()
        {
            return Path.Combine(BuildJqaDataPath(), "finished-templates");
        }

        public static string BuildLocalRulesPath()
        {
            return Path.Combine(Application.dataPath, "CQA Rules");
        }

        public static string BuildGlobalRulesPath()
        {
            return Path.Combine(BuildJqaDataPath(), "custom-rules");
        }

        public string BuildJqaExecutablePath()
        {
            string platformSpecificCommand = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "jqassistant.cmd"
                : "jqassistant.sh";
            return Path.Combine(BuildJqaPath(), "bin", platformSpecificCommand);
        }

        public string BuildJqaPluginFolderPath()
        {
            return Path.Combine(BuildJqaPath(), "plugins");
        }

        public static string BuildBuiltInRulesPath()
        {
            return Path.Combine(Application.dataPath, "_rules");
        }
    }
}