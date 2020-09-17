using System.IO;
using Produktivkeller.CodeQualityAssurance.Editor.History;
using Produktivkeller.CodeQualityAssurance.Editor.JqAssistant;
using Produktivkeller.CodeQualityAssurance.Editor.UiComponents;
using UnityEditor;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.Tabs
{
    public class ScanAndReportTab
    {
        private readonly HistoryManager _historyManager;
        private readonly JqaExecutor _jqaExecutor;
        private readonly JqaManager _jqaManager;
        private readonly RuleSelector _ruleSelector;

        public ScanAndReportTab(JqaManager jqaManager, JqaExecutor jqaExecutor, HistoryManager historyManager,
            RuleSelector ruleSelector)
        {
            _jqaManager = jqaManager;
            _jqaExecutor = jqaExecutor;
            _historyManager = historyManager;
            _ruleSelector = ruleSelector;
        }

        public void Show()
        {
            CqaLabel.Heading1("Scan");

            if (!JqaManager.CheckIfJqaIsInstalled())
            {
                CqaLabel.Error("CQA is currently not installed.");
                return;
            }

            CqaLabel.Normal("CQA scans all scripts inside your Assets folder.");

            History.History history = _historyManager.LoadHistory();

            if (history != null)
            {
                CqaLabel.Normal("The last scan was at " + history.GetLastScan() + ".");
            }

            EditorGUI.BeginDisabledGroup(_jqaExecutor.IsAnyProcessRunning());
            if (CqaButton.NormalButton("Scan Assets"))
            {
                _jqaExecutor.ScanAssets();
            }

            if (Directory.Exists(JqaPaths.Instance.BuildJqaStorePath()) && CqaButton.NormalButton("Start Server"))
            {
                _jqaExecutor.StartServer();
            }

            EditorGUI.EndDisabledGroup();

            if (_jqaExecutor.IsScanProcessRunning())
            {
                CqaLabel.Bold("Status: Running ...");

                // FIXME: Stopping the process does currently not work.
                //
                // if (CqaButton.NormalButton("Stop Process"))
                // {
                //     _jqaExecutor.StopProcess();
                // }
            }

            GUILayout.Space(30);
            CqaLine.DrawHorizontalLine();

            CqaLabel.Heading1("Report");

            if (!JqaExecutor.DidFinishSuccessfullyOnce())
            {
                CqaLabel.Error("You must scan your project before you can create a report.");
                return;
            }

            CqaLabel.Normal("Select rules that you want to be checked in the report.");

            if (_jqaExecutor.IsReportProcessRunning())
            {
                CqaLabel.Bold("Status: Running ...");

                // FIXME: Stopping the process does currently not work.
                //
                // if (CqaButton.NormalButton("Stop Process"))
                // {
                //     _jqaExecutor.StopProcess();
                // }
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

            FileInfo reportFileInfo = new FileInfo(JqaPaths.Instance.BuildJqaHtmlReportPath());
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