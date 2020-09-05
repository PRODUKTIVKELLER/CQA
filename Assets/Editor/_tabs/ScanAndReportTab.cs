using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Editor._history;
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
        private readonly JqaPaths _jqaPaths;
        private readonly HistoryManager _historyManager;

        private readonly Dictionary<Group, List<Rule>> _rulesByGroup;
        private readonly Dictionary<Rule, bool> _ruleCheckboxes;
        private readonly Dictionary<Group, bool> _groupCheckboxes;
        private readonly Dictionary<Group, bool> _groupFoldout;

        public ScanAndReportTab(JqaManager jqaManager, JqaExecutor jqaExecutor, HistoryManager historyManager,
            JqaPaths jqaPaths)
        {
            _jqaManager = jqaManager;
            _jqaExecutor = jqaExecutor;
            _historyManager = historyManager;
            _jqaPaths = jqaPaths;

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
            CqaLabel.Heading1("Scan");

            if (!_jqaManager.CheckIfJqaIsInstalled())
            {
                CqaLabel.Error("CQA is currently not installed.");
                return;
            }

            CqaLabel.Normal("CQA scans all scripts inside your Assets folder.");

            History history = _historyManager.LoadHistory();

            if (history != null)
            {
                CqaLabel.Normal("The last scan was at " + history.GetLastScan() + ".");
            }

            EditorGUI.BeginDisabledGroup(_jqaExecutor.IsAnyProcessRunning());
            if (CqaButton.NormalButton("Scan Assets"))
            {
                _jqaExecutor.ScanAssets();
            }
            EditorGUI.EndDisabledGroup();

            if (_jqaExecutor.IsScanProcessRunning())
            {
                CqaLabel.Bold("Status: Running ...");

                if (CqaButton.NormalButton("Stop Process"))
                {
                    _jqaExecutor.StopProcess();
                }
            }

            GUILayout.Space(30);
            DrawLine();

            CqaLabel.Heading1("Report");

            if (!_jqaExecutor.DidFinishSuccessfullyOnce())
            {
                CqaLabel.Error("You must scan your project before you can create a report.");
                return;
            }

            CqaLabel.Normal("Select rules that you want to be checked in the report.");

            if (_jqaExecutor.IsReportProcessRunning())
            {
                CqaLabel.Bold("Status: Running ...");

                if (CqaButton.NormalButton("Stop Process"))
                {
                    _jqaExecutor.StopProcess();
                }
            }

            ListRulesWithCheckboxes();
            ShowReportButtons();
        }

        private void ShowReportButtons()
        {
            EditorGUI.BeginDisabledGroup(_jqaExecutor.IsAnyProcessRunning());
            GUILayout.Space(15);

            if (CqaButton.NormalButton("Create Report"))
            {
                _jqaExecutor.CheckAndReport();
            }

            FileInfo reportFileInfo = new FileInfo(_jqaPaths.BuildJqaHtmlReportPath());
            EditorGUI.BeginDisabledGroup(!reportFileInfo.Exists);
            if (CqaButton.NormalButton("Open Report in Browser"))
            {
                Application.OpenURL(reportFileInfo.FullName);
            }

            EditorGUI.EndDisabledGroup();

            EditorGUI.EndDisabledGroup();
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

        private static void DrawLine()
        {
            const float padding = 0;
            const float thickness = 1;

            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2f;
            r.x -= 22;
            r.width += 26;
            EditorGUI.DrawRect(r, Color.grey);
        }
    }
}