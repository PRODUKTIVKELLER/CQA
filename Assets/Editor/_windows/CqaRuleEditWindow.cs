using System.Collections.Generic;
using Editor._forms;
using Editor._model;
using Editor._tabs;
using Editor._ui;
using UnityEditor;
using UnityEngine;

namespace Editor._windows
{
    public class CqaRuleEditWindow : EditorWindow
    {
        private string DialogTitle { get; set; }
        public DataScope DataScope { get; set; }
        private Group OldGroup { get; set; }
        private Rule OldRule { get; set; }

        private StringFormGroup _keyStringFormGroup;
        private StringFormGroup _descriptionStringFormGroup;
        private TextAreaFormGroup _cypherQueryTextAreaFormGroup;
        private EnumFormGroup<RuleType> _ruleTypeFormGroup;
        private DropdownFormGroup _groupDropdownFormGroup;

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

            Rule rule = new Rule();
            rule.key = _keyStringFormGroup.Value;
            rule.description = _descriptionStringFormGroup.Value;
            rule.cypherQuery = _cypherQueryTextAreaFormGroup.Value;
            rule.type = _ruleTypeFormGroup.Value;

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
            string title = BuildTitle(rule);

            CqaRuleEditWindow cqaRuleEditWindow =
                GetWindow<CqaRuleEditWindow>(true, title);

            cqaRuleEditWindow.maxSize = new Vector2(500, 700);
            cqaRuleEditWindow.minSize = new Vector2(300, 700);

            cqaRuleEditWindow.Init(dataScope, title, rule, group);
        }

        private void Init(DataScope dataScope, string dialogTitle, Rule rule, Group group)
        {
            DialogTitle = dialogTitle;
            OldRule = rule;
            OldGroup = group;
            DataScope = dataScope;

            InitializeForm();
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
            _keyStringFormGroup = StringFormGroup.Build(
                "*Key:",
                "Specify a unique key for the rule.",
                OldRule?.key,
                NonAllowedRuleDescriptions
            );

            _descriptionStringFormGroup = StringFormGroup.Build(
                "*Description:",
                "The description is used in the rule selection and in the report.",
                OldRule?.description,
                NonAllowedRuleKeys
            );

            _groupDropdownFormGroup = DropdownFormGroup.Build(
                "Group:",
                "Select to which group this rule should belong.",
                RuleDao.Instance.GetAvailableGroups()
            );

            _ruleTypeFormGroup = EnumFormGroup<RuleType>.Build(
                "Type:",
                "Select if this rule is a constraint or a concept."
            );

            _cypherQueryTextAreaFormGroup = TextAreaFormGroup.Build(
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