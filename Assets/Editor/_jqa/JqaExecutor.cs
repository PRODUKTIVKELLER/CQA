using System.Diagnostics;
using System.IO;
using System.Reflection;
using Logging;
using UnityEngine;

namespace Editor._jqa
{
    public class JqaExecutor
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private JqaPaths _jqaPaths;
        private Process _jqAssistantProcess;

        public JqaExecutor(JqaPaths jqaPaths)
        {
            _jqaPaths = jqaPaths;
        }

        public void ScanAssets()
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                Log.Error("Detected OS: {}. Only Windows is supported currently.", Application.platform);
                return;
            }

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
        }

        public bool IsProcessRunning()
        {
            return _jqAssistantProcess != null && !_jqAssistantProcess.HasExited;
        }

        public void StopProcess()
        {
            _jqAssistantProcess.Kill();
        }

        public void CheckAndReport()
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                Log.Error("Detected OS: {}. Only Windows is supported currently.", Application.platform);
                return;
            }

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
    }
}