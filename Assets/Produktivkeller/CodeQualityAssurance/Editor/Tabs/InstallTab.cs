using System.Threading;
using Produktivkeller.CodeQualityAssurance.Editor.JqAssistant;
using Produktivkeller.CodeQualityAssurance.Editor.UiComponents;
using UnityEditor;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.Tabs
{
    public class InstallTab
    {
        private readonly JqaManager _jqaManager;

        public InstallTab(JqaManager jqaManager)
        {
            _jqaManager = jqaManager;
        }

        public void Show()
        {
            CqaLabel.Heading1("Install & Uninstall");

            if (_jqaManager.CheckIfJqaIsInstalling())
            {
                CqaLabel.Bold("Status: Installing ...");
            }
            else if (JqaManager.CheckIfJqaIsInstalled())
            {
                CqaLabel.Success("Status: Installed");
            }
            else
            {
                CqaLabel.Error("Status: Not Installed");
            }

            if (!JqaManager.CheckIfJqaIsInstalled())
            {
                GUILayout.Label("CQA is not installed yet.");
                GUILayout.Label("To install it we need to download files from jqassistant.org and github.com.");
                GUILayout.Label("If you agree click the button below.");

                EditorGUILayout.Space();

                if (CqaButton.NormalButton("Install"))
                {
                    _jqaManager.InstallJqa();
                    Thread.Sleep(10);
                }

                return;
            }

            if (_jqaManager.CheckIfJqaIsInstalling())
            {
                GUILayout.Label("For more information have a look at the Unity console.");
                EditorGUILayout.Space();

                if (CqaButton.NormalButton("Abort Installation"))
                {
                    _jqaManager.AbortInstallation();

                    while (_jqaManager.CheckIfJqaIsInstalling())
                    {
                        Thread.Sleep(0);
                    }

                    JqaManager.UninstallJqa();
                }

                return;
            }

            EditorGUILayout.Space();
            if (CqaButton.NormalButton("Uninstall"))
            {
                JqaManager.UninstallJqa();
            }
        }
    }
}