using System;

namespace Produktivkeller.CodeQualityAssurance.Editor.AsciiDoc.Model
{
    [Serializable]
    public class Rule
    {
        public string cypherQuery;
        public string description;
        public string key;
        public RuleType type;
    }
}