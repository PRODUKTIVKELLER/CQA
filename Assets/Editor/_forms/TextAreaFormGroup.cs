using Editor._ui;
using UnityEngine;

namespace Editor._forms
{
    public class TextAreaFormGroup
    {
        public bool Touched { get; private set; }
        public string Value { get; private set; } = "";

        private string _label;
        private string _description;

        public void Show()
        {
            CqaLabel.FormLabel(_label);
            CqaLabel.FormDescription(_description);
            string newValue = GUILayout.TextArea(Value, GUILayout.MinHeight(100));
            
            if (newValue != Value)
            {
                Touched = true;
            }

            Value = newValue;

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
            TextAreaFormGroup stringFormGroup = new TextAreaFormGroup();
            stringFormGroup.Value = initialValue?? "";
            stringFormGroup._label = label;
            stringFormGroup._description = description;
            return stringFormGroup;
        }

        public bool IsValid()
        {
            return Value.Length > 0;
        }
    }
}