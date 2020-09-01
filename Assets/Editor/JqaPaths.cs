﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Editor
{
    public class JqaPaths
    {
        private const string CompanyFolderName = "produktivkeller";
        private const string ApplicationFolderName = "unity-code-quality-assurance";
        private const string VersionFolderName = "0.0.1";


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

        public string BuildJqaPath()
        {
            return Path.Combine(BuildJqaInstallationPath(), "jqassistant-commandline-neo4jv3-1.8.0");
        }

        public string BuildJqaDataPath()
        {
            return Path.Combine(BuildAppDataPath(), "data", Application.productName);
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
    }
}