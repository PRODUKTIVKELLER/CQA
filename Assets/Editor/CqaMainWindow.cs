﻿using System.Reflection;
using Editor._history;
using Editor._jqa;
using Editor._tabs;
using Logging;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class CqaMainWindow : EditorWindow
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private HistoryManager _historyManager;
        private JqaManager _jqaManager;
        private JqaExecutor _jqaExecutor;
        private InstallTab _installTab;
        private ScanAndReportTab _scanAndReportTab;
        private int _selectedTab;
        private GUIStyle _navigationButtonStyle;

        [MenuItem("CQA/Main Window")]
        public static void ShowWindow()
        {
            GetWindow(typeof(CqaMainWindow), true, "Code Quality Assurance");
        }

        private void OnGUI()
        {
            InitializeFieldsIfNecessary();

            EditorGUILayout.BeginHorizontal();

            _selectedTab = GUILayout.SelectionGrid(_selectedTab,
                new[] {"Install & Uninstall", "Scan & Report", "Tutorial"}, 1, _navigationButtonStyle);

            EditorGUI.DrawRect(
                new Rect(
                    200,
                    0,
                    1,
                    position.height
                ), new Color(15 / 255f, 25 / 255f, 37 / 255f));

            EditorGUILayout.BeginVertical();
            switch (_selectedTab)
            {
                case 0:
                    _installTab.OnGUI();
                    break;
                case 1:
                    _scanAndReportTab.OnGUI();
                    break;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void InitializeFieldsIfNecessary()
        {
            JqaPaths jqaPaths = new JqaPaths();

            if (_jqaManager == null)
            {
                _jqaManager = new JqaManager(jqaPaths);
            }

            if (_historyManager == null)
            {
                _historyManager = new HistoryManager();
            }

            if (_jqaExecutor == null)
            {
                _jqaExecutor = new JqaExecutor(jqaPaths, _historyManager);
            }

            if (_scanAndReportTab == null)
            {
                _scanAndReportTab = new ScanAndReportTab(_jqaManager, _jqaExecutor, _historyManager, jqaPaths);
            }

            if (_installTab == null)
            {
                _installTab = new InstallTab(_jqaManager);
            }

            if (_navigationButtonStyle == null)
            {
                _navigationButtonStyle = NavigationButtonStyleProvider.Provide();
            }
        }
    }
}