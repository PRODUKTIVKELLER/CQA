using Produktivkeller.CodeQualityAssurance.Editor.UiComponents;
using UnityEditor;

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
            CqaLabel.Heading1("Manage Global Rules");
            CqaLabel.Normal(
                "Global rules are available for all Unity projects of your user. They are not located in the Assets folder.");

            ShowGlobalRuleEditor();

            CqaLine.DrawHorizontalLine();

            CqaLabel.Heading1("Manage " + PlayerSettings.productName + " Rules");
            CqaLabel.Normal(
                "Local rules are only available for the current project. They are located in the Assets folder and can be shared via VCS.");

            ShowLocalRuleEditor();
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