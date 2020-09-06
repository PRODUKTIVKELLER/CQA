using System;

namespace Produktivkeller.CodeQualityAssurance.Editor.AsciiDoc.Model
{
    [Serializable]
    public class Rule
    {
        public string key;
        public string description;
        public string cypherQuery;
        public RuleType type;
    }
}