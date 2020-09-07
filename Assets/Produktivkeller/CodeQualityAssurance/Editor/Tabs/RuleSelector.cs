using System.Collections.Generic;
using Produktivkeller.CodeQualityAssurance.Editor.AsciiDoc;
using Produktivkeller.CodeQualityAssurance.Editor.AsciiDoc.Model;
using UnityEditor;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.Tabs
{
    public class RuleSelector
    {
        private List<Group> _builtInGroupList;

        private Dictionary<Group, bool> _groupFoldout;
        public Dictionary<Group, bool> GroupCheckboxes;
        public Dictionary<Rule, bool> RuleCheckboxes;

        internal RuleSelector()
        {
            OnReload();

            RuleDao.Instance.OnReload(OnReload);
        }

        private void OnReload()
        {
            RuleCheckboxes = new Dictionary<Rule, bool>();
            GroupCheckboxes = new Dictionary<Group, bool>();

            _builtInGroupList = RuleDao.Instance.GetAllGroups();
            _groupFoldout = new Dictionary<Group, bool>();

            foreach (Group group in _builtInGroupList)
            {
                GroupCheckboxes.Add(group, true);
                _groupFoldout[group] = false;

                foreach (Rule rule in group.rules)
                {
                    RuleCheckboxes[rule] = true;
                }
            }
        }

        public void OnGUI()
        {
            ListRulesWithCheckboxes();
        }

        private void ListRulesWithCheckboxes()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rules:", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            foreach (Group group in _builtInGroupList)
            {
                CreateGroupCheckboxAndFoldout(group);
                CreateRuleCheckboxesForGroup(group);
            }
        }

        private void CreateGroupCheckboxAndFoldout(Group group)
        {
            EditorGUI.BeginDisabledGroup(group.key == "common");
            EditorGUILayout.BeginHorizontal();
            bool newGroupValue = EditorGUILayout.Toggle(GroupCheckboxes[group], GUILayout.Width(15));

            if (newGroupValue != GroupCheckboxes[group])
            {
                foreach (Rule rule in group.rules)
                {
                    RuleCheckboxes[rule] = newGroupValue;
                }

                GroupCheckboxes[group] = newGroupValue;
            }

            _groupFoldout[group] = EditorGUILayout.Foldout(_groupFoldout[group],
                new GUIContent(group.name, group.key == "common" ? "This must be part of the report." : null));
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }


        private void CreateRuleCheckboxesForGroup(Group group)
        {
            if (!_groupFoldout[group])
            {
                return;
            }

            foreach (Rule rule in group.rules)
            {
                // Set a horizontal offset.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20);
                EditorGUI.BeginDisabledGroup(group.key == "common");
                bool newValue = EditorGUILayout.ToggleLeft(
                    new GUIContent(rule.description,
                        group.key == "common" ? "This must be part of the report." : null), RuleCheckboxes[rule]);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();

                UpdateGroupAccordingToContainingRules(group, rule, newValue);
                RuleCheckboxes[rule] = newValue;
            }
        }

        private void UpdateGroupAccordingToContainingRules(Group group, Rule rule, bool newValue)
        {
            if (!newValue)
            {
                GroupCheckboxes[group] = false;
                return;
            }

            foreach (Rule other in group.rules)
            {
                if (other != rule && !RuleCheckboxes[other])
                {
                    return;
                }
            }

            GroupCheckboxes[group] = true;
        }
    }
}