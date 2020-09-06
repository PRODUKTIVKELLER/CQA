using System;

namespace Editor._model
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