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
        private InstallTab _installTab;
        private JqaExecutor _jqaExecutor;
        private JqaManager _jqaManager;
        private ManageCustomRulesTab _manageCustomRulesTab;
        private GUIStyle _navigationButtonStyle;
        private RuleSelector _ruleSelector;
        private ScanAndReportTab _scanAndReportTab;
        private int _selectedTab;

        [MenuItem("CQA/Main Window")]
        public static void ShowWindow()
        {
            CqaMainWindow cqaMainWindow =
                (CqaMainWindow) GetWindow(typeof(CqaMainWindow), true, "Code Quality Assurance");
            cqaMainWindow.minSize = new Vector2(960, 540);
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
            if (_jqaManager == null)
            {
                _jqaManager = new JqaManager();
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
                _jqaExecutor = new JqaExecutor(_historyManager, _ruleSelector);
            }

            if (_scanAndReportTab == null)
            {
                _scanAndReportTab =
                    new ScanAndReportTab(_jqaManager, _jqaExecutor, _historyManager, _ruleSelector);
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