using Produktivkeller.CodeQualityAssurance.Editor.UiComponents;
using Produktivkeller.CodeQualityAssurance.Editor.Windows;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.Tabs
{
    public class ManageCustomRulesTab
    {
        private readonly RuleManager _globalRuleManager;
        private readonly RuleManager _localRuleManager;

        public ManageCustomRulesTab()
        {
            _localRuleManager = new RuleManager(DataScope.Local);
            _globalRuleManager = new RuleManager(DataScope.Global);
        }

        public void Show()
        {
            CqaLabel.Heading1("Manage Custom Rules");

            ShowGlobalRuleEditor();
            ShowLocalRuleEditor();

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            if (CqaButton.NormalButton("Create Group"))
            {
                CqaGroupEditWindow.Open(DataScope.Local);
            }

            if ((_localRuleManager.DoGroupsExist() || _globalRuleManager.DoGroupsExist()) &&
                CqaButton.NormalButton("Create Rule"))
            {
                CqaRuleEditWindow.Open(DataScope.Local);
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(30);
        }

        private void ShowLocalRuleEditor()
        {
            _localRuleManager.OnGUI();
        }

        private void ShowGlobalRuleEditor()
        {
            _globalRuleManager.OnGUI();
        }
    }
}