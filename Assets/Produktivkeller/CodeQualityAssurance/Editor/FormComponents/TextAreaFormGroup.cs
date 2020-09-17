using Produktivkeller.CodeQualityAssurance.Editor.UiComponents;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.FormComponents
{
    public class TextAreaFormGroup
    {
        private string _description;

        private string _label;
        private string _value = "";
        private bool Touched { get; set; }
        public string Value => _value?.Trim();

        public void Show()
        {
            CqaLabel.FormLabel(_label);
            CqaLabel.FormDescription(_description);
            string newValue = GUILayout.TextArea(Value, GUILayout.MinHeight(100));

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

        public static TextAreaFormGroup Build(
            string label,
            string description,
            string initialValue
        )
        {
            TextAreaFormGroup stringFormGroup = new TextAreaFormGroup
            {
                _value = initialValue ?? "",
                _label = label,
                _description = description
            };
            return stringFormGroup;
        }

        public bool IsValid()
        {
            return Value.Length > 0;
        }
    }
}