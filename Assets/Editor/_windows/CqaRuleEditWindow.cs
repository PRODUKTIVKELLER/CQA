using System.Collections.Generic;
using System.Linq;
using Editor._model;
using Editor._tabs;
using Editor._ui;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class CqaRuleEditWindow : EditorWindow
    {
        private string Title { get; set; }
        private DataScope DataScope { get; set; }
        private Rule OldRule { get; set; }

        private string _name;
        private string _description;
        private string _cypherQuery;
        private RuleType _ruleType;
        private RuleScope _ruleScope;
        private int _selectedGroupIndex;

        private Dictionary<Group, List<Rule>> _localRulesByGroup;
        private Dictionary<Group, List<Rule>> _globalRulesByGroup;

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
            CqaLabel.Heading2(Title);

            CqaLabel.FormLabel("Scope:");
            _ruleScope = (RuleScope) EditorGUILayout.EnumPopup(_ruleScope);

            CqaLabel.FormLabel("Group:");
            
            string[] groups = GetAvailableGroups();
            if (groups.Length > 0)
            {
                _selectedGroupIndex = EditorGUILayout.Popup(_selectedGroupIndex, GetAvailableGroups());
            }

            CqaLabel.FormLabel("Name:");
            _name = GUILayout.TextField(_name);

            CqaLabel.FormLabel("Type:");
            _ruleType = (RuleType) EditorGUILayout.EnumPopup(_ruleType);


            CqaLabel.FormLabel("Description:");
            _description = GUILayout.TextArea(_description);

            CqaLabel.FormLabel("Cypher Query:");
            _cypherQuery = GUILayout.TextArea(_cypherQuery);
        }

        private string[] GetAvailableGroups()
        {
            if (_localRulesByGroup == null || _globalRulesByGroup == null)
            {
                return new string[] { };
            }
            
            List<Group> groups = _ruleScope == RuleScope.Local
                ? _localRulesByGroup.Keys.ToList()
                : _globalRulesByGroup.Keys.ToList();
            return groups.Select((g) => g.Description).ToArray();
        }

        public static void Open(DataScope dataScope, Rule rule = null)
        {
            string title = BuildTitle(rule);

            CqaRuleEditWindow cqaRuleEditWindow =
                GetWindow<CqaRuleEditWindow>(true, title);

            cqaRuleEditWindow.Init(dataScope, title, rule);
        }

        private void Init(DataScope dataScope, string title, Rule rule)
        {
            DataScope = dataScope;
            Title = title;
            OldRule = rule;

            _localRulesByGroup = RuleDetector.DetectLocalRules();
            _globalRulesByGroup = RuleDetector.DetectGlobalRules();
        }

        private static string BuildTitle(Rule rule)
        {
            return (rule == null ? "Create " : "Edit ") + "Rule";
        }
    }
}