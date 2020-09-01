using System.Reflection;
using System.Threading;
using Logging;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class CqaMainWindow : EditorWindow
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private JqaManager _jqaManager;
        private JqaExecutor _jqaExecutor;
        private Thread _installThread;
        private int _selectedTab;

        [MenuItem("CQA/Main Window")]
        public static void ShowWindow()
        {
            GetWindow(typeof(CqaMainWindow), true, "Code Quality Assurance");
        }

        private void OnGUI()
        {
            InitializeFieldsIfNecessary();

            _selectedTab = GUILayout.Toolbar(_selectedTab, new[] {"Install & Uninstall", "Scan & Report"},
                GUILayout.Width(500));
            switch (_selectedTab)
            {
                case 0:
                    ShowInstallTab();
                    break;
                case 1:
                    ShowScanAndReportTab();
                    break;
            }
        }

        private void ShowInstallTab()
        {
            string status = "";

            if (_installThread != null && _installThread.IsAlive)
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

                if (NormalButton("Install"))
                {
                    _installThread = new Thread(
                        () =>
                        {
                            Thread.CurrentThread.IsBackground = true;
                            _jqaManager.InstallJqa();
                        }
                    );
                    _installThread.Start();
                    Thread.Sleep(10);
                }

                return;
            }

            if (_installThread != null && _installThread.IsAlive)
            {
                GUILayout.Label("For more information have a look at the Unity console.");
                EditorGUILayout.Space();

                if (NormalButton("Abort Installation"))
                {
                    _installThread.Abort();

                    while (_installThread.IsAlive)
                    {
                        Thread.Sleep(0);
                    }
                    _jqaManager.UninstallJqa();
                }

                return;
            }

            if (NormalButton("Uninstall"))
            {
                _jqaManager.UninstallJqa();
            }
        }

        private void ShowScanAndReportTab()
        {
            if (!_jqaManager.CheckIfJqaIsInstalled() || (_installThread != null && _installThread.IsAlive))
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("CQA is currently not installed.");
                EditorGUILayout.Space();
                return;
            }

            EditorGUI.BeginDisabledGroup(_jqaExecutor.IsScanRunning());
            if (NormalButton("Scan Assets"))
            {
                _jqaExecutor.ScanAssets();
            }

            EditorGUI.EndDisabledGroup();

            if (_jqaExecutor.IsScanRunning())
            {
                if (NormalButton("Stop Scan"))
                {
                    _jqaExecutor.StopScan();
                }
            }
        }

        private void InitializeFieldsIfNecessary()
        {
            JqaPaths jqaPaths = new JqaPaths();

            if (_jqaManager == null)
            {
                _jqaManager = new JqaManager(jqaPaths);
            }

            if (_jqaExecutor == null)
            {
                _jqaExecutor = new JqaExecutor(jqaPaths);
            }
        }

        private bool NormalButton(string text)
        {
            return GUILayout.Button(text, GUILayout.Width(150));
        }
    }
}