using Editor._jqa;
using Editor._ui;
using UnityEditor;

namespace Editor._tabs
{
    public class ScanAndReportTab : Tab
    {
        private readonly JqaManager _jqaManager;
        private readonly JqaExecutor _jqaExecutor;

        public ScanAndReportTab(JqaManager jqaManager, JqaExecutor jqaExecutor)
        {
            _jqaManager = jqaManager;
            _jqaExecutor = jqaExecutor;
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

            if (!_jqaExecutor.IsProcessRunning())
            {
                if (CqaButton.NormalButton("Check & Report"))
                {
                    _jqaExecutor.CheckAndReport();
                }
            }

            if (_jqaExecutor.IsProcessRunning())
            {
                if (CqaButton.NormalButton("Stop Process"))
                {
                    _jqaExecutor.StopProcess();
                }
            }
        }
    }
}