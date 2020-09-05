using System.Threading;
using Editor._jqa;
using Editor._ui;
using UnityEditor;
using UnityEngine;

namespace Editor._tabs
{
    public class InstallTab : Tab
    {
        private readonly JqaManager _jqaManager;

        public InstallTab(JqaManager jqaManager)
        {
            _jqaManager = jqaManager;
        }

        public void OnGUI()
        {
            string status = "";

            if (_jqaManager.CheckIfJqaIsInstalling())
            {
                status = "Installing ...";
            }
            else if (_jqaManager.CheckIfJqaIsInstalled())
            {
                status = "Installed";
            }
            else
            {
                status = "Not Installed";
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Status: " + status, EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (!_jqaManager.CheckIfJqaIsInstalled())
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

                    _jqaManager.UninstallJqa();
                }

                return;
            }

            if (CqaButton.NormalButton("Uninstall"))
            {
                _jqaManager.UninstallJqa();
            }
        }
    }
}