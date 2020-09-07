using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Produktivkeller.CodeQualityAssurance.Editor.AsciiDoc;
using Produktivkeller.CodeQualityAssurance.Editor.History;
using Produktivkeller.CodeQualityAssurance.Editor.Logging;
using Produktivkeller.CodeQualityAssurance.Editor.Tabs;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.JqAssistant
{
    public class JqaExecutor
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly HistoryManager _historyManager;
        private readonly RuleSelector _ruleSelector;
        private Thread _historyWatcher;
        private bool _isScan;
        private Process _jqAssistantProcess;

        public JqaExecutor(HistoryManager historyManager, RuleSelector ruleSelector)
        {
            _ruleSelector = ruleSelector;
            _historyManager = historyManager;
        }

        public void ScanAssets()
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                Log.Error("Detected OS: {}. Only Windows is supported currently.", Application.platform);
                return;
            }

            _isScan = true;

            _jqAssistantProcess = new Process
            {
                StartInfo =
                {
                    FileName = JqaPaths.Instance.BuildJqaExecutablePath(),
                    Arguments = "scan -reset -f " + Application.dataPath + " -s " + JqaPaths.Instance.BuildJqaStorePath(),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            _jqAssistantProcess.OutputDataReceived += (s, e) => Log.Debug(e.Data);
            _jqAssistantProcess.ErrorDataReceived += (s, e) => Log.Debug(e.Data);

            _jqAssistantProcess.Start();
            _jqAssistantProcess.BeginOutputReadLine();
            _jqAssistantProcess.BeginErrorReadLine();

            _historyWatcher = new Thread(() =>
            {
                while (true)
                {
                    if (_jqAssistantProcess != null && _jqAssistantProcess.HasExited)
                    {
                        _jqAssistantProcess = null;
                        _historyManager.SaveLastSuccessfulAnalysis();
                        break;
                    }

                    Thread.Sleep(10);
                }
            });

            _historyWatcher.Start();
        }

        public bool IsScanProcessRunning()
        {
            return _isScan && _jqAssistantProcess != null && !_jqAssistantProcess.HasExited;
        }

        public bool IsReportProcessRunning()
        {
            return !_isScan && _jqAssistantProcess != null && !_jqAssistantProcess.HasExited;
        }

        public bool IsAnyProcessRunning()
        {
            return _jqAssistantProcess != null && !_jqAssistantProcess.HasExited;
        }

        public void StopProcess()
        {
            // FIXME: We need to kill the whole process tree.
            // This could be a starting point: https://stackoverflow.com/questions/5901679/kill-process-tree-programmatically-in-c-sharp
            //
            // _jqAssistantProcess.Kill();
        }

        public void CheckAndReport()
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                Log.Error("Detected OS: {}. Only Windows is supported currently.", Application.platform);
                return;
            }

            _isScan = false;

            AsciiDocTemplater asciiDocTemplater = new AsciiDocTemplater(
                _ruleSelector.GroupCheckboxes,
                _ruleSelector.RuleCheckboxes
            );

            Log.Debug("Filling templates and copying them to {}.", JqaPaths.Instance.BuildJqaHtmlFinishedTemplatesPath());
            asciiDocTemplater.PrepareAnalysis();

            string groupsForCommandLine = asciiDocTemplater.ReturnRelevantGroupsForCommandLine();

            _jqAssistantProcess = new Process
            {
                StartInfo =
                {
                    FileName = JqaPaths.Instance.BuildJqaExecutablePath(),
                    Arguments = "analyze" +
                                " -r " + JqaPaths.Instance.BuildJqaHtmlFinishedTemplatesPath() +
                                " -reportDirectory " + JqaPaths.Instance.BuildJqaReportPath() +
                                " -s " + JqaPaths.Instance.BuildJqaStorePath() +
                                " -executeAppliedConcepts" +
                                " -groups " + groupsForCommandLine,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            _jqAssistantProcess.OutputDataReceived += (s, e) => Log.Debug(e.Data);
            _jqAssistantProcess.ErrorDataReceived += (s, e) => Log.Debug(e.Data);

            _jqAssistantProcess.Start();
            _jqAssistantProcess.BeginOutputReadLine();
            _jqAssistantProcess.BeginErrorReadLine();
        }

        public bool DidFinishSuccessfullyOnce()
        {
            FileInfo historyFileInfo = new FileInfo(JqaPaths.Instance.BuildCqaHistoryPath());
            return historyFileInfo.Exists;
        }
    }
}