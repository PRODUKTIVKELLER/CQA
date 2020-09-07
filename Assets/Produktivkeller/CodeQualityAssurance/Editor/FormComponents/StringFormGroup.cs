using System;
using System.Collections.Generic;
using Produktivkeller.CodeQualityAssurance.Editor.UiComponents;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.FormComponents
{
    public class StringFormGroup
    {
        private string _description;

        private string _initialValue;
        private string _label;
        private Func<List<string>> _providePotentialClashes;
        public bool Touched { get; private set; }
        public string Value { get; private set; } = "";

        public void Show()
        {
            CqaLabel.FormLabel(_label);
            CqaLabel.FormDescription(_description);
            string newValue = GUILayout.TextField(Value);

            if (newValue != Value)
            {
                Touched = true;
            }

            Value = newValue;

            if (Touched && newValue.Length == 0)
            {
                CqaLabel.FormError("This field can not be empty.");
            }
            else if (!HasInitialValue() && newValue != _initialValue && _providePotentialClashes().Contains(Value))
            {
                CqaLabel.FormError("This name is already taken.");
            }

            CqaLabel.FormGroupSpacing();
        }

        public void Autofill(string value)
        {
            Value = value;
            Touched = false;
        }

        private bool HasInitialValue()
        {
            if (_initialValue == null)
            {
                return false;
            }

            return Value == _initialValue;
        }

        public static StringFormGroup Build(
            string label,
            string description,
            string initialValue,
            Func<List<string>> providePotentialClashes
        )
        {
            StringFormGroup stringFormGroup = new StringFormGroup();
            stringFormGroup._initialValue = initialValue;
            stringFormGroup.Value = initialValue;
            stringFormGroup._label = label;
            stringFormGroup._description = description;
            stringFormGroup._providePotentialClashes = providePotentialClashes;
            return stringFormGroup;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Value) && !_providePotentialClashes().Contains(Value);
        }
    }
}