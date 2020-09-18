using System.Collections.Generic;
using Produktivkeller.CodeQualityAssurance.Editor.AsciiDoc;
using Produktivkeller.CodeQualityAssurance.Editor.AsciiDoc.Model;
using Produktivkeller.CodeQualityAssurance.Editor.FormComponents;
using Produktivkeller.CodeQualityAssurance.Editor.Tabs;
using Produktivkeller.CodeQualityAssurance.Editor.UiComponents;
using UnityEditor;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.Windows
{
    public class CqaRuleEditWindow : EditorWindow
    {
        private TextAreaFormGroup _cypherQueryTextAreaFormGroup;
        private StringFormGroup _descriptionStringFormGroup;
        private DropdownFormGroup _groupDropdownFormGroup;

        private StringFormGroup _keyStringFormGroup;
        private EnumFormGroup<RuleType> _ruleTypeFormGroup;
        private string DialogTitle { get; set; }
        private DataScope DataScope { get; set; }
        private Group OldGroup { get; set; }
        private Rule OldRule { get; set; }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            GUILayout.BeginVertical();
            ShowForm();
            GUILayout.EndVertical();

            GUILayout.Space(20);
            GUILayout.EndHorizontal();
        }

        private void ShowForm()
        {
            CqaLabel.Heading2(DialogTitle);

            _groupDropdownFormGroup.Show();
            _keyStringFormGroup.Show();
            _ruleTypeFormGroup.Show();
            _descriptionStringFormGroup.Show();
            _cypherQueryTextAreaFormGroup.Show();

            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(!Valid());
            if (CqaButton.SmallButton("Save"))
            {
                Save();
            }

            EditorGUI.EndDisabledGroup();

            if (OldRule != null && CqaButton.SmallButton("Delete"))
            {
                Delete();
            }

            GUILayout.EndHorizontal();

            if (CqaButton.SmallButton("Abort"))
            {
                Close();
            }

            GUILayout.Space(10);
        }

        private void Delete()
        {
            GroupFileDao.Delete(DataScope, OldGroup.key, OldRule.key);
            Close();
        }


        private void Save()
        {
            if (OldRule != null)
            {
                GroupFileDao.Delete(DataScope, OldGroup.key, OldRule.key);
            }

            Rule rule = new Rule
            {
                key = _keyStringFormGroup.Value,
                description = _descriptionStringFormGroup.Value,
                cypherQuery = _cypherQueryTextAreaFormGroup.Value,
                type = _ruleTypeFormGroup.Value
            };

            string groupName = _groupDropdownFormGroup.Value;
            DataScope dataScope = RuleDao.Instance.GetDataScopeByGroupName(groupName);
            GroupFileDao.Save(dataScope, groupName, rule);

            Close();
        }

        private bool Valid()
        {
            return _keyStringFormGroup.IsValid() &&
                   _descriptionStringFormGroup.IsValid() &&
                   _cypherQueryTextAreaFormGroup.IsValid();
        }

        public static void Open(DataScope dataScope, Group group = null, Rule rule = null)
        {
            string dialogTitle = BuildTitle(rule);

            CqaRuleEditWindow cqaRuleEditWindow =
                GetWindow<CqaRuleEditWindow>(true, dialogTitle);
            
            cqaRuleEditWindow.minSize = new Vector2(500, 700);
            cqaRuleEditWindow.DialogTitle = dialogTitle;
            cqaRuleEditWindow.OldRule = rule;
            cqaRuleEditWindow.OldGroup = group;
            cqaRuleEditWindow.DataScope = dataScope;

            cqaRuleEditWindow.InitializeForm();
        }

        private List<string> NonAllowedRuleKeys()
        {
            string groupName = _groupDropdownFormGroup.Value;
            return RuleDao.Instance.GetRuleKeysForGroup(groupName);
        }


        private List<string> NonAllowedRuleDescriptions()
        {
            string groupName = _groupDropdownFormGroup.Value;
            return RuleDao.Instance.GetRuleDescriptionsForGroup(groupName);
        }

        private void InitializeForm()
        {
            _keyStringFormGroup = new StringFormGroup(
                "*Key:",
                "Specify a unique key for the rule.",
                OldRule?.key,
                NonAllowedRuleDescriptions
            );

            _descriptionStringFormGroup = new StringFormGroup(
                "*Description:",
                "The description is used in the rule selection and in the report.",
                OldRule?.description,
                NonAllowedRuleKeys
            );

            _groupDropdownFormGroup = new DropdownFormGroup(
                "Group:",
                "Select to which group this rule should belong.",
                RuleDao.Instance.GetAvailableGroups(),
                OldGroup?.name
            );

            _ruleTypeFormGroup = new EnumFormGroup<RuleType>(
                "Type:",
                "Select if this rule is a constraint or a concept."
            );

            _cypherQueryTextAreaFormGroup = new TextAreaFormGroup(
                "*Cypher Query:",
                "Enter the Cypher query for your rule.",
                OldRule?.cypherQuery
            );
        }

        private static string BuildTitle(Rule rule)
        {
            return (rule == null ? "Create " : "Edit ") + "Rule";
        }
    }
}