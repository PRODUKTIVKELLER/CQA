using Editor._ui;
using Microsoft.SqlServer.Server;
using UnityEngine;

namespace Editor
{
    public class StringFormGroup
    {
        public bool Touched { get; private set; }
        public string Value { get; private set; } = "";

        private string _label;
        private string _description;

        public void Show()
        {
            CqaLabel.FormLabel(_label);
            CqaLabel.FormDescription(_description);
            string newValue = GUILayout.TextField(Value);
            CqaLabel.FormGroupSpacing();

            if (newValue != Value)
            {
                Touched = true;
            }

            Value = newValue;
        }

        public void Autofill(string value)
        {
            Value = value;
            Touched = false;
        }

        public static StringFormGroup Build(string label, string description)
        {
            StringFormGroup stringFormGroup = new StringFormGroup();
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