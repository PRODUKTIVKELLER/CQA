using System.Reflection;
using Editor._jqa;
using Editor._tabs;
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
        private InstallTab _installTab;
        private ScanAndReportTab _scanAndReportTab;
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
                    _installTab.OnGUI();
                    break;
                case 1:
                    _scanAndReportTab.OnGUI();
                    break;
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

            if (_scanAndReportTab == null)
            {
                _scanAndReportTab = new ScanAndReportTab(_jqaManager, _jqaExecutor);
            }

            if (_installTab == null)
            {
                _installTab = new InstallTab(_jqaManager);
            }
        }
    }
}