using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.JqAssistant
{
    public class JqaPaths
    {
        private const string CompanyFolderName = "produktivkeller";
        private const string ApplicationFolderName = "unity-code-quality-assurance";
        private const string VersionFolderName = "0.2";
        private static JqaPaths _instance;

        private readonly string _cqaFolderInAssets;

        private JqaPaths()
        {
            _cqaFolderInAssets = Directory.GetDirectories(
                Application.dataPath,
                Path.Combine("Produktivkeller", "CodeQualityAssurance"),
                SearchOption.AllDirectories
            )[0];
        }

        public static JqaPaths Instance => _instance ?? (_instance = new JqaPaths());

        private string BuildAppDataPath()
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

        public string BuildJqaPluginFolderPath()
        {
            return Path.Combine(BuildJqaPath(), "plugins");
        }

        private string BuildJqaDataPath()
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            string productFolderName = rgx.Replace(Application.productName.ToLower().Replace(" ", "-"), "");

            return Path.Combine(BuildAppDataPath(), "data", productFolderName);
        }

        public string BuildJqaStorePath()
        {
            return Path.Combine(BuildJqaDataPath(), "store");
        }

        public string BuildCqaHistoryPath()
        {
            return Path.Combine(BuildJqaStorePath(), "cqa.json");
        }

        public string BuildJqaReportPath()
        {
            return Path.Combine(BuildJqaDataPath(), "report");
        }

        public string BuildJqaHtmlReportPath()
        {
            return Path.Combine(BuildJqaReportPath(), "asciidoc", "index.html");
        }

        public string BuildJqaHtmlFinishedTemplatesPath()
        {
            return Path.Combine(BuildJqaDataPath(), "finished-templates");
        }

        private string BuildAssetsRulesPath()
        {
            return Path.Combine(_cqaFolderInAssets, "Rules");
        }

        public string BuildBuiltInRulesPath()
        {
            return Path.Combine(BuildAssetsRulesPath(), "BuiltInRules");
        }

        public string BuildLocalRulesPath()
        {
            return Path.Combine(BuildAssetsRulesPath(), "CustomRules");
        }

        public string BuildGlobalRulesPath()
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

        public string BuildIndexAdocTemplatePath()
        {
            return Path.Combine(_cqaFolderInAssets, "AsciiDocTemplate", "index.adoc");
        }
    }
}