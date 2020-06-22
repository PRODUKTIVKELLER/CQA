using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Logging;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class CqaMainWindow : EditorWindow
    {
        private static readonly Queue<Action> ExecutionQueue = new Queue<Action>();

        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Process _jqAssistantProcess;

        private const string JqAssistantFolder = "Assets/_dependencies/jqassistant-commandline-neo4jv3-1.8.0/bin";
        private const string JqAssistantWindowsExecutable = "jqassistant.cmd";

        [MenuItem("CQA/Main Window")]
        public static void ShowWindow()
        {
            GetWindow(typeof(CqaMainWindow), true, "Code Quality Assurance");
        }

        private void OnGUI()
        {
            EditorGUI.BeginDisabledGroup(_jqAssistantProcess != null && !_jqAssistantProcess.HasExited);
            if (GUILayout.Button("Do it"))
            {
                RunJqAssistant();
            }

            EditorGUI.EndDisabledGroup();
        }

        private void Update()
        {
            lock (ExecutionQueue)
            {
                while (ExecutionQueue.Count > 0)
                {
                    ExecutionQueue.Dequeue().Invoke();
                }
            }
        }

        private void Enqueue(Action action)
        {
            lock (ExecutionQueue)
            {
                ExecutionQueue.Enqueue(action);
            }
        }

        private void RunJqAssistant()
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                Log.Error("Detected OS: {}. Only Windows is supported currently.", Application.platform);
                return;
            }

            string jQAssistantFullPath = Path.Combine(Environment.CurrentDirectory, JqAssistantFolder,
                JqAssistantWindowsExecutable);

            _jqAssistantProcess = new Process
            {
                StartInfo =
                {
                    FileName = jQAssistantFullPath,
                    Arguments = "scan -f Assets/_demo",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            _jqAssistantProcess.OutputDataReceived += (s, e) => Enqueue(() => Log.Debug(e.Data));
            _jqAssistantProcess.ErrorDataReceived += (s, e) => Enqueue(() => Log.Debug(e.Data));

            _jqAssistantProcess.Start();
            _jqAssistantProcess.BeginOutputReadLine();
            _jqAssistantProcess.BeginErrorReadLine();
        }
    }
}