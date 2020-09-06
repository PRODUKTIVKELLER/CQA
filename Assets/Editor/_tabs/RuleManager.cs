using System;
using System.Collections.Generic;
using System.Linq;
using Editor._model;
using Editor._ui;
using UnityEditor;
using UnityEngine;

namespace Editor._tabs
{
    public class RuleManager
    {
        private readonly Dictionary<Group, List<Rule>> _rulesByGroup;
        private readonly DataScope _dataScope;
        private readonly Dictionary<Group, bool> _groupFoldout;

        public RuleManager(DataScope dataScope)
        {
            _dataScope = dataScope;

            _rulesByGroup = _dataScope == DataScope.Local
                ? RuleDetector.DetectLocalRules()
                : RuleDetector.DetectGlobalRules();

            _groupFoldout = new Dictionary<Group, bool>();

            foreach (Group group in _rulesByGroup.Keys)
            {
                _groupFoldout[group] = false;
            }
        }

        public void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rules:", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            List<Group> groupsSortedByDescription = _rulesByGroup.Keys.ToList();
            groupsSortedByDescription.Sort((k1, k2) => string.Compare(k1.Name, k2.Name, StringComparison.Ordinal));

            if (groupsSortedByDescription.Count == 0)
            {
                CqaLabel.Slim("No rules found.");
            }

            foreach (Group group in groupsSortedByDescription)
            {
                CreateGroupCheckboxAndFoldout(group);
                CreateRuleCheckboxesForGroup(group);
            }

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            if (CqaButton.NormalButton("Create Group"))
            {
                CqaGroupEditWindow.Open(_dataScope);
            }

            if (_rulesByGroup.Keys.Count > 0 && CqaButton.NormalButton("Create Rule"))
            {
                CqaRuleEditWindow.Open(_dataScope);
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(30);
        }


        private void CreateGroupCheckboxAndFoldout(Group group)
        {
            GUILayout.BeginHorizontal();
            if (CqaButton.EditButton())
            {
                CqaGroupEditWindow.Open(_dataScope, group);
            }

            _groupFoldout[group] = EditorGUILayout.Foldout(_groupFoldout[group], group.Description);
            GUILayout.EndHorizontal();
        }


        private void CreateRuleCheckboxesForGroup(Group group)
        {
            if (!_groupFoldout[group])
            {
                return;
            }

            foreach (Rule rule in _rulesByGroup[group])
            {
                // Set a horizontal offset.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20);

                if (CqaButton.EditButton())
                {
                    CqaRuleEditWindow.Open(_dataScope, rule);
                }

                CqaLabel.FoldoutEntry(rule.Description);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}