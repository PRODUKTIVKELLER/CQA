using System.Text.RegularExpressions;
using Editor._forms;
using Editor._tabs;
using Editor._ui;
using UnityEditor;
using UnityEngine;
using Group = Editor._model.Group;

namespace Editor._windows
{
    public class CqaGroupEditWindow : EditorWindow
    {
        private string DialogTitle { get; set; }
        private DataScope DataScope { get; set; }
        private Group OldGroup { get; set; }

        private bool _specificKey;

        private StringFormGroup _nameStringFormGroup;
        private StringFormGroup _keyStringFormGroup;
        private EnumFormGroup<DataScope> _scopeEnumFormGroup;

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            GUILayout.BeginVertical();
            ShowForm();
            GUILayout.EndVertical();

            GUILayout.Space(20);
            GUILayout.EndHorizontal();
        }

        private void ShowForm()
        {
            CqaLabel.Heading2(DialogTitle);

            _scopeEnumFormGroup.Show();
            _nameStringFormGroup.Show();

            UpdateKeyFromNameIfUserDidNotSetIt();

            _keyStringFormGroup.Show();


            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(!Valid());
            if (CqaButton.SmallButton("Save"))
            {
                Save();
            }

            EditorGUI.EndDisabledGroup();

            if (OldGroup != null && CqaButton.SmallButton("Delete"))
            {
                Delete();
            }

            GUILayout.EndHorizontal();
        }

        private void Delete()
        {
            GroupFileDao.Delete(DataScope, OldGroup.key);
            Close();
        }

        private void UpdateKeyFromNameIfUserDidNotSetIt()
        {
            if (_nameStringFormGroup.Value != null && (!_keyStringFormGroup.Touched || _keyStringFormGroup.Value.Length == 0))
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
            Group group = new Group();
            group.key = _keyStringFormGroup.Value;
            group.name = _nameStringFormGroup.Value;

            if (OldGroup != null && OldGroup.key != group.key)
            {
                GroupFileDao.Delete(DataScope, OldGroup.key);
            }

            GroupFileDao.Save(_scopeEnumFormGroup.Value, group);

            Close();
        }

        public static void Open(DataScope dataScope, Group group = null)
        {
            string title = BuildTitle(group);

            CqaGroupEditWindow cqaGroupEditWindow =
                GetWindow<CqaGroupEditWindow>(true, title);

            cqaGroupEditWindow.maxSize = new Vector2(300, 420);
            cqaGroupEditWindow.minSize = new Vector2(300, 420);

            cqaGroupEditWindow.Init(dataScope, title, group);
        }

        private void Init(DataScope dataScope, string dialogTitle, Group group)
        {
            DataScope = dataScope;
            DialogTitle = dialogTitle;
            OldGroup = group;

            InitializeForm();

            if (group != null)
            {
                _keyStringFormGroup.Autofill(group.key);
                _nameStringFormGroup.Autofill(group.name);
            }

            _scopeEnumFormGroup.Autofill(dataScope);
        }

        private static string BuildTitle(Group group)
        {
            return (group == null ? "Create " : "Edit ") + "Group";
        }

        private void InitializeForm()
        {
            if (_nameStringFormGroup == null)
            {
                _nameStringFormGroup = StringFormGroup.Build(
                    "*Name:",
                    "Specify a name for the group.",
                    OldGroup?.name,
                    RuleDao.Instance.GetAllGroupNames
                );
            }

            if (_keyStringFormGroup == null)
            {
                _keyStringFormGroup = StringFormGroup.Build(
                    "*Key:",
                    "Specify a unique key for the group.",
                    OldGroup?.key,
                    RuleDao.Instance.GetAllGroupKeys
                );
            }

            if (_scopeEnumFormGroup == null)
            {
                _scopeEnumFormGroup = EnumFormGroup<DataScope>.Build(
                    "Scope:",
                    "Select if this group should be available just for this or for all projects."
                );
            }
        }
    }
}