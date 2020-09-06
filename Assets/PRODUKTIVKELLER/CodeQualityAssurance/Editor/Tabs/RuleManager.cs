using System;
using System.Collections.Generic;
using System.Linq;
using Produktivkeller.CodeQualityAssurance.Editor.AsciiDoc;
using Produktivkeller.CodeQualityAssurance.Editor.AsciiDoc.Model;
using Produktivkeller.CodeQualityAssurance.Editor.UiComponents;
using Produktivkeller.CodeQualityAssurance.Editor.Windows;
using UnityEditor;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.Tabs
{
    public class RuleManager
    {
        private List<Group> _groupList;
        private readonly DataScope _dataScope;
        private Dictionary<Group, bool> _groupFoldout;

        public RuleManager(DataScope dataScope)
        {
            _dataScope = dataScope;

            OnReload();
            RuleDao.Instance.OnReload(OnReload);
        }

        private void OnReload()
        {
            _groupList = _dataScope == DataScope.Local
                ? RuleDetector.DetectLocalRules()
                : RuleDetector.DetectGlobalRules();

            _groupFoldout = new Dictionary<Group, bool>();

            foreach (Group group in _groupList)
            {
                _groupFoldout[group] = false;
            }
        }

        public void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rules:", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            List<Group> groupsSortedByDescription = _groupList.ToList();
            groupsSortedByDescription.Sort((k1, k2) => string.Compare(k1.key, k2.key, StringComparison.Ordinal));

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

            if (_groupList.Count > 0 && CqaButton.NormalButton("Create Rule"))
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

            _groupFoldout[group] = EditorGUILayout.Foldout(_groupFoldout[group], group.name);
            GUILayout.EndHorizontal();
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

                if (CqaButton.EditButton())
                {
                    CqaRuleEditWindow.Open(_dataScope, group, rule);
                }

                CqaLabel.FoldoutEntry(rule.description);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}