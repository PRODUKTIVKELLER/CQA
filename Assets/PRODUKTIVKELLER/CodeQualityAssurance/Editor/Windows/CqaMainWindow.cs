using Produktivkeller.CodeQualityAssurance.Editor.History;
using Produktivkeller.CodeQualityAssurance.Editor.JqAssistant;
using Produktivkeller.CodeQualityAssurance.Editor.Tabs;
using Produktivkeller.CodeQualityAssurance.Editor.UiComponents;
using Produktivkeller.CodeQualityAssurance.Editor.UiStyles;
using UnityEditor;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.Windows
{
    public class CqaMainWindow : EditorWindow
    {
        private HistoryManager _historyManager;
        private JqaManager _jqaManager;
        private JqaExecutor _jqaExecutor;
        private InstallTab _installTab;
        private ScanAndReportTab _scanAndReportTab;
        private ManageCustomRulesTab _manageCustomRulesTab;
        private RuleSelector _ruleSelector;
        private int _selectedTab;
        private GUIStyle _navigationButtonStyle;

        [MenuItem("CQA/Main Window")]
        public static void ShowWindow()
        {
            GetWindow(typeof(CqaMainWindow), true, "Code Quality Assurance");
        }

        private void OnGUI()
        {
            InitializeFieldsIfNecessary();

            EditorGUILayout.BeginHorizontal();

            _selectedTab = GUILayout.SelectionGrid(_selectedTab,
                new[] {"Install & Uninstall", "Scan & Report", "Manage Custom Rules", "Tutorial"}, 1,
                _navigationButtonStyle);

            CqaLine.DrawNavigationLine(position.height);

            EditorGUILayout.BeginVertical();
            switch (_selectedTab)
            {
                case 0:
                    _installTab.Show();
                    break;
                case 1:
                    _scanAndReportTab.Show();
                    break;
                case 2:
                    _manageCustomRulesTab.Show();
                    break;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        // FIXME: Remove null checks.
        private void InitializeFieldsIfNecessary()
        {
            JqaPaths jqaPaths = new JqaPaths();

            if (_jqaManager == null)
            {
                _jqaManager = new JqaManager(jqaPaths);
            }

            if (_historyManager == null)
            {
                _historyManager = new HistoryManager();
            }

            if (_ruleSelector == null)
            {
                _ruleSelector = new RuleSelector();
            }

            if (_jqaExecutor == null)
            {
                _jqaExecutor = new JqaExecutor(jqaPaths, _historyManager, _ruleSelector);
            }

            if (_scanAndReportTab == null)
            {
                _scanAndReportTab =
                    new ScanAndReportTab(_jqaManager, _jqaExecutor, _historyManager, jqaPaths, _ruleSelector);
            }

            if (_installTab == null)
            {
                _installTab = new InstallTab(_jqaManager);
            }

            if (_manageCustomRulesTab == null)
            {
                _manageCustomRulesTab = new ManageCustomRulesTab();
            }

            if (_navigationButtonStyle == null)
            {
                _navigationButtonStyle = NavigationButtonStyleProvider.Provide();
            }
        }
    }
}