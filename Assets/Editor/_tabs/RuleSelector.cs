using System;
using System.Collections.Generic;
using System.Linq;
using Editor._model;
using UnityEditor;
using UnityEngine;

namespace Editor._tabs
{
    public class RuleSelector
    {
        public readonly Dictionary<Group, List<Rule>> RulesByGroup;
        public readonly Dictionary<Rule, bool> RuleCheckboxes;
        public readonly Dictionary<Group, bool> GroupCheckboxes;
        private readonly Dictionary<Group, bool> _groupFoldout;

        internal RuleSelector()
        {
            RulesByGroup = RuleDetector.DetectBuiltInRules();
            RuleCheckboxes = new Dictionary<Rule, bool>();
            GroupCheckboxes = new Dictionary<Group, bool>();
            _groupFoldout = new Dictionary<Group, bool>();

            foreach (Group group in RulesByGroup.Keys)
            {
                GroupCheckboxes.Add(group, true);
                _groupFoldout[group] = false;

                foreach (Rule rule in RulesByGroup[group])
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

            List<Group> groupsSortedByDescription = RulesByGroup.Keys.ToList();
            groupsSortedByDescription.Sort((k1, k2) => string.Compare(k1.Name, k2.Name, StringComparison.Ordinal));

            foreach (Group group in groupsSortedByDescription)
            {
                CreateGroupCheckboxAndFoldout(group);
                CreateRuleCheckboxesForGroup(group);
            }
        }

        private void CreateGroupCheckboxAndFoldout(Group group)
        {
            EditorGUI.BeginDisabledGroup(group.Name == "common");
            EditorGUILayout.BeginHorizontal();
            bool newGroupValue = EditorGUILayout.Toggle(GroupCheckboxes[group], GUILayout.Width(15));

            if (newGroupValue != GroupCheckboxes[group])
            {
                foreach (Rule rule in RulesByGroup[group])
                {
                    RuleCheckboxes[rule] = newGroupValue;
                }

                GroupCheckboxes[group] = newGroupValue;
            }

            _groupFoldout[group] = EditorGUILayout.Foldout(_groupFoldout[group],
                new GUIContent(group.Description, group.Name == "common" ? "This must be part of the report." : null));
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }


        private void CreateRuleCheckboxesForGroup(Group group)
        {
            if (!_groupFoldout[group])
            {
                return;
            }

            foreach (Rule rule in RulesByGroup[group])
            {
                // Set a horizontal offset.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20);
                EditorGUI.BeginDisabledGroup(group.Name == "common");
                bool newValue = EditorGUILayout.ToggleLeft(
                    new GUIContent(rule.Description,
                        group.Name == "common" ? "This must be part of the report." : null), RuleCheckboxes[rule]);
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

            foreach (Rule other in RulesByGroup[group])
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