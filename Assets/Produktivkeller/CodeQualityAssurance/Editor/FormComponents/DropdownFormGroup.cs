using Produktivkeller.CodeQualityAssurance.Editor.UiComponents;
using UnityEditor;

namespace Produktivkeller.CodeQualityAssurance.Editor.FormComponents
{
    public class DropdownFormGroup
    {
        private string _description;
        private string _label;
        private string[] _possibleValues;

        private int _selectedValue;
        public string Value => _possibleValues[_selectedValue];

        public void Show()
        {
            CqaLabel.FormLabel(_label);
            CqaLabel.FormDescription(_description);
            _selectedValue = EditorGUILayout.Popup(_selectedValue, _possibleValues);
            CqaLabel.FormGroupSpacing();
        }

        public static DropdownFormGroup Build(
            string label,
            string description,
            string[] possibleValues,
            string initialValue = null
        )
        {
            DropdownFormGroup stringFormGroup = new DropdownFormGroup();
            stringFormGroup._label = label;
            stringFormGroup._description = description;
            stringFormGroup._possibleValues = possibleValues;

            if (initialValue != null)
            {
                for (int i = 0; i < possibleValues.Length; i++)
                {
                    if (possibleValues[i] == initialValue)
                    {
                        stringFormGroup._selectedValue = i;
                        break;
                    }
                }
            }

            return stringFormGroup;
        }
    }
}