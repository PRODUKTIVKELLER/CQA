using System;
using System.Collections.Generic;

namespace Editor._model
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