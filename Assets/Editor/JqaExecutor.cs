using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Logging;
using UnityEngine;

namespace Editor
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
                    Arguments = "scan -f " + Application.dataPath,
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

        public bool IsScanRunning()
        {
            return _jqAssistantProcess != null && !_jqAssistantProcess.HasExited;
        }

        public void StopScan()
        {
            _jqAssistantProcess.Kill();
        }
    }
}