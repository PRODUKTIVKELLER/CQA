using Produktivkeller.CodeQualityAssurance.Editor.UiComponents;
using UnityEditor;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.FormComponents
{
    public class TextAreaFormGroup
    {
        private readonly string _description;
        private readonly string _label;
        private Vector2 _scroll;
        private string _value;

        public TextAreaFormGroup(
            string label,
            string description,
            string initialValue
        )
        {
            _value = initialValue ?? "\n\n\n\n";
            _label = label;
            _description = description;
        }

        private bool Touched { get; set; }
        public string Value => _value?.Trim();


        public void Show()
        {
            CqaLabel.FormLabel(_label);
            CqaLabel.FormDescription(_description);

            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            string newValue = GUILayout.TextArea(_value);
            EditorGUILayout.EndScrollView();

            if (newValue != Value)
            {
                Touched = true;
            }

            _value = newValue;

            if (Touched && newValue.Length == 0)
            {
                CqaLabel.FormError("This field can not be empty.");
            }

            CqaLabel.FormGroupSpacing();
        }

        public bool IsValid()
        {
            return Value.Length > 0;
        }
    }
}