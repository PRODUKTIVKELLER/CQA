using System.IO;
using Editor._history;
using Editor._jqa;
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
        private readonly RuleSelector _ruleSelector;

        public ScanAndReportTab(JqaManager jqaManager, JqaExecutor jqaExecutor, HistoryManager historyManager,
            JqaPaths jqaPaths, RuleSelector ruleSelector)
        {
            _jqaManager = jqaManager;
            _jqaExecutor = jqaExecutor;
            _historyManager = historyManager;
            _jqaPaths = jqaPaths;
            _ruleSelector = ruleSelector;
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
            CqaLine.DrawHorizontalLine();

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

            _ruleSelector.OnGUI();
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
    }
}