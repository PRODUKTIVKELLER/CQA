using System;
using System.Collections.Generic;

namespace Produktivkeller.CodeQualityAssurance.Editor.AsciiDoc.Model
{
    [Serializable]
    public class Group
    {
        public string key;
        public string name;
        public List<Rule> rules;

        public Group()
        {
            rules = new List<Rule>();
        }
    }
}