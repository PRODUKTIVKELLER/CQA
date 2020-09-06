using System.Text.RegularExpressions;
using Editor._tabs;
using Editor._ui;
using UnityEditor;
using UnityEngine;
using Group = Editor._model.Group;

namespace Editor
{
    public class CqaGroupEditWindow : EditorWindow
    {
        private string DialogTitle { get; set; }
        private DataScope DataScope { get; set; }
        private Group OldGroup { get; set; }

        private bool _specificKey;

        private DataScope _dataScope;
        private StringFormGroup _nameStringFormGroup;
        private StringFormGroup _keyStringFormGroup;
        private EnumFormGroup<DataScope> _scopeEnumFormGroup;

        private void OnGUI()
        {
            InitializeIfNecessary();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            GUILayout.BeginVertical();
            ShowForm();
            GUILayout.EndVertical();

            GUILayout.Space(20);
            GUILayout.EndHorizontal();
        }

        private void InitializeIfNecessary()
        {
            if (_nameStringFormGroup == null)
            {
                _nameStringFormGroup = StringFormGroup.Build("Name:", "Specify a name for the group.");
            }

            if (_keyStringFormGroup == null)
            {
                _keyStringFormGroup = StringFormGroup.Build("Key:", "Specify a unique key for the group.");
            }

            if (_scopeEnumFormGroup == null)
            {
                _scopeEnumFormGroup = EnumFormGroup<DataScope>.Build("Scope:",
                    "Select if this group should be available just for this or for all projects.");
            }
        }

        private void ShowForm()
        {
            CqaLabel.Heading2(DialogTitle);

            _scopeEnumFormGroup.Show();
            _nameStringFormGroup.Show();

            UpdateKeyFromNameIfUserDidNotSetIt();

            _keyStringFormGroup.Show();


            EditorGUI.BeginDisabledGroup(!Valid());
            if (CqaButton.NormalButton("Save"))
            {
                Save();
            }

            EditorGUI.EndDisabledGroup();
        }

        private void UpdateKeyFromNameIfUserDidNotSetIt()
        {
            if (!_keyStringFormGroup.Touched || _keyStringFormGroup.Value.Length == 0)
            {
                Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                string key = rgx.Replace(_nameStringFormGroup.Value.ToLower().Replace(" ", "-"), "");

                _keyStringFormGroup.Autofill(key);
            }
        }

        private bool Valid()
        {
            return _nameStringFormGroup.IsValid() && _keyStringFormGroup.IsValid();
        }

        private void Save()
        {
        }

        public static void Open(DataScope dataScope, Group group = null)
        {
            string title = BuildTitle(group);

            CqaGroupEditWindow cqaRuleEditWindow =
                GetWindow<CqaGroupEditWindow>(true, title);

            cqaRuleEditWindow.Init(dataScope, title, group);
        }

        private void Init(DataScope dataScope, string dialogTitle, Group group)
        {
            DataScope = dataScope;
            DialogTitle = dialogTitle;
            OldGroup = group;

            if (group != null)
            {
                _keyStringFormGroup.Autofill(group.Name);
                _nameStringFormGroup.Autofill(group.Description);
                _dataScope = dataScope;
            }
        }

        private static string BuildTitle(Group group)
        {
            return (group == null ? "Create " : "Edit ") + "Group";
        }
    }
}