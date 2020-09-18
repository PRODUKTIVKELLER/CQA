using Produktivkeller.CodeQualityAssurance.Editor.UiComponents;
using UnityEditor;

namespace Produktivkeller.CodeQualityAssurance.Editor.FormComponents
{
    public class DropdownFormGroup
    {
        private readonly string _description;
        private readonly string _label;
        private readonly string[] _possibleValues;

        private int _selectedValue;

        public DropdownFormGroup(
            string label,
            string description,
            string[] possibleValues,
            string initialValue = null
        )
        {
            _label = label;
            _description = description;
            _possibleValues = possibleValues;

            if (initialValue == null)
            {
                return;
            }

            for (int i = 0; i < possibleValues.Length; i++)
            {
                if (possibleValues[i] == initialValue)
                {
                    _selectedValue = i;
                    return;
                }
            }
        }

        public string Value => _possibleValues[_selectedValue];

        public void Show()
        {
            CqaLabel.FormLabel(_label);
            CqaLabel.FormDescription(_description);
            _selectedValue = EditorGUILayout.Popup(_selectedValue, _possibleValues);
            CqaLabel.FormGroupSpacing();
        }
    }
}