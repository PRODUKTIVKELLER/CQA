using System;
using System.Collections.Generic;
using System.Linq;
using Produktivkeller.CodeQualityAssurance.Editor.UiComponents;
using UnityEditor;

namespace Produktivkeller.CodeQualityAssurance.Editor.FormComponents
{
    public class EnumFormGroup<T> where T : struct, IConvertible
    {
        private readonly string _description;
        private readonly string _label;
        private readonly string[] _possibleValues;

        private int _selected;

        public EnumFormGroup(string label, string description)
        {
            _label = label;
            _description = description;

            List<T> values = new List<T>(Enum.GetValues(typeof(T)).Cast<T>());
            _possibleValues = values.Select(x => x.ToString()).ToArray();
        }

        public T Value => (T) Enum.Parse(typeof(T), _possibleValues[_selected]);

        public void Show()
        {
            CqaLabel.FormLabel(_label);
            CqaLabel.FormDescription(_description);

            _selected = EditorGUILayout.Popup(_selected, _possibleValues);

            CqaLabel.FormGroupSpacing();
        }

        public void Autofill(T value)
        {
            _selected = ConvertToInt(value);
        }

        private static int ConvertToInt(T value)
        {
            Enum test = Enum.Parse(typeof(T), value.ToString()) as Enum;
            return Convert.ToInt32(test);
        }
    }
}