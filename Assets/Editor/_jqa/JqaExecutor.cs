using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Editor._history;
using Logging;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Editor._jqa
{
    public class JqaExecutor
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private HistoryManager _historyManager;
        private JqaPaths _jqaPaths;
        private Process _jqAssistantProcess;
        private Thread _historyWatcher;
        private bool _isScan;

        public JqaExecutor(JqaPaths jqaPaths, HistoryManager historyManager)
        {
            _historyManager = historyManager;
            _jqaPaths = jqaPaths;
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
                    FileName = _jqaPaths.BuildJqaExecutablePath(),
                    Arguments = "scan -reset -f " + Application.dataPath + " -s " + _jqaPaths.BuildJqaStorePath(),
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

            _jqAssistantProcess = new Process
            {
                StartInfo =
                {
                    FileName = _jqaPaths.BuildJqaExecutablePath(),
                    Arguments = "analyze" +
                                " -r " + Path.Combine(Application.dataPath, "_rules") +
                                " -reportDirectory " + _jqaPaths.BuildJqaReportPath() +
                                " -s " + _jqaPaths.BuildJqaStorePath() +
                                " -executeAppliedConcepts" +
                                " -groups common,code-style",
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
            FileInfo historyFileInfo = new FileInfo(_jqaPaths.BuildCqaHistoryPath());
            return historyFileInfo.Exists;
        }
    }
}