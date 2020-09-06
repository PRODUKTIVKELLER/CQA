﻿using System;
using System.Collections.Generic;
using System.Linq;
using Editor._ui;
using UnityEditor;

namespace Editor._forms
{
    public class EnumFormGroup<T> where T : struct, IConvertible
    {
        public bool Touched { get; private set; }

        public T Value => (T) Enum.Parse(typeof(T), _possibleValues[_selected]);

        private int _selected;
        private string _label;
        private string _description;
        private string[] _possibleValues;

        public void Show()
        {
            CqaLabel.FormLabel(_label);
            CqaLabel.FormDescription(_description);

            int newValue = EditorGUILayout.Popup(_selected, _possibleValues);
            
            CqaLabel.FormGroupSpacing();

            if (newValue != _selected)
            {
                Touched = true;
            }
            
            _selected = newValue;
        }

        public void Autofill(T value)
        {
            _selected = ConvertToInt(value);
            Touched = false;
        }

        public static EnumFormGroup<T> Build(string label, string description)
        {
            EnumFormGroup<T> enumFormGroup = new EnumFormGroup<T>();
            enumFormGroup._label = label;
            enumFormGroup._description = description;

            List<T> values = new List<T>(Enum.GetValues(typeof(T)).Cast<T>());
            enumFormGroup._possibleValues = values.Select(x => x.ToString()).ToArray();

            return enumFormGroup;
        }

        private int ConvertToInt(T value)
        {
            Enum test = Enum.Parse(typeof(T), value.ToString()) as Enum;
            return Convert.ToInt32(test);
        }
    }
}