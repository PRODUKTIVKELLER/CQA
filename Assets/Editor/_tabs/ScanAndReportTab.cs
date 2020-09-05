using System;
using System.Collections.Generic;
using System.Linq;
using Editor._jqa;
using Editor._model;
using Editor._ui;
using UnityEditor;
using UnityEngine;

namespace Editor._tabs
{
    public class ScanAndReportTab : Tab
    {
        private readonly JqaManager _jqaManager;
        private readonly JqaExecutor _jqaExecutor;

        private readonly Dictionary<Group, List<Rule>> _rulesByGroup;
        private readonly Dictionary<Rule, bool> _ruleCheckboxes;
        private readonly Dictionary<Group, bool> _groupCheckboxes;
        private readonly Dictionary<Group, bool> _groupFoldout;


        public ScanAndReportTab(JqaManager jqaManager, JqaExecutor jqaExecutor)
        {
            _jqaManager = jqaManager;
            _jqaExecutor = jqaExecutor;

            _rulesByGroup = new RuleDetector().DetectRules();
            _ruleCheckboxes = new Dictionary<Rule, bool>();
            _groupCheckboxes = new Dictionary<Group, bool>();
            _groupFoldout = new Dictionary<Group, bool>();

            foreach (Group group in _rulesByGroup.Keys)
            {
                _groupCheckboxes.Add(group, true);
                _groupFoldout[group] = false;

                foreach (Rule rule in _rulesByGroup[group])
                {
                    _ruleCheckboxes[rule] = true;
                }
            }
        }

        public void OnGUI()
        {
            if (!_jqaManager.CheckIfJqaIsInstalled())
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("CQA is currently not installed.");
                EditorGUILayout.Space();
                return;
            }

            if (!_jqaExecutor.IsProcessRunning())
            {
                if (CqaButton.NormalButton("Scan Assets"))
                {
                    _jqaExecutor.ScanAssets();
                }
            }

            if (_jqaExecutor.IsProcessRunning())
            {
                if (CqaButton.NormalButton("Stop Process"))
                {
                    _jqaExecutor.StopProcess();
                }
            }
            
            ListRulesWithCheckboxes();
            
            if (!_jqaExecutor.IsProcessRunning())
            {
                EditorGUILayout.Space();
                if (CqaButton.NormalButton("Check & Report"))
                {
                    _jqaExecutor.CheckAndReport();
                }
            }
        }

        private void ListRulesWithCheckboxes()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rules:", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            List<Group> groupsSortedByDescription = _rulesByGroup.Keys.ToList();
            groupsSortedByDescription.Sort((k1, k2) => string.Compare(k1.Name, k2.Name, StringComparison.Ordinal));

            foreach (Group group in groupsSortedByDescription)
            {
                CreateGroupCheckboxAndFoldout(group);
                CreateRuleForGroup(group);
            }
        }

        private void CreateRuleForGroup(Group group)
        {
            if (!_groupFoldout[group])
            {
                return;
            }

            float originalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 400;

            foreach (Rule rule in _rulesByGroup[group])
            {
                // Set a horizontal offset.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20);
                _ruleCheckboxes[rule] = EditorGUILayout.ToggleLeft(rule.Description, _ruleCheckboxes[rule]);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUIUtility.labelWidth = originalLabelWidth;
        }

        private void CreateGroupCheckboxAndFoldout(Group group)
        {
            EditorGUILayout.BeginHorizontal();
            bool newGroupValue = EditorGUILayout.Toggle(_groupCheckboxes[group], GUILayout.Width(15));

            if (newGroupValue != _groupCheckboxes[group])
            {
                foreach (Rule rule in _rulesByGroup[group])
                {
                    _ruleCheckboxes[rule] = newGroupValue;
                }

                _groupCheckboxes[group] = newGroupValue;
            }

            _groupFoldout[group] = EditorGUILayout.Foldout(_groupFoldout[group], group.Description);
            EditorGUILayout.EndHorizontal();
        }
    }
}