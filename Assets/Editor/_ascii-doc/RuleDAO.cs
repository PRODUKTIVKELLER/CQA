using System;
using System.Collections.Generic;
using System.Linq;
using Editor._model;
using Editor._tabs;

namespace Editor
{
    public class RuleDao
    {
        public static RuleDao Instance => _instance ?? (_instance = new RuleDao());

        private static RuleDao _instance;

        private Dictionary<Group, List<Rule>> _builtInRulesByGroup;
        private List<Group> _localGroupList;
        private List<Group> _globalGroupList;

        private List<string> _allGroupKeys;
        private List<string> _allGroupNames;
        private List<Action> _onReloadActions;

        private RuleDao()
        {
            _allGroupKeys = new List<string>();
            _allGroupNames = new List<string>();
            _onReloadActions = new List<Action>();

            ReloadAll();
        }

        public void OnReload(Action onReload)
        {
            _onReloadActions.Add(onReload);
        }

        public void ReloadAll()
        {
            _allGroupKeys.Clear();
            _allGroupNames.Clear();

            _builtInRulesByGroup = RuleDetector.DetectBuiltInRules();
            _localGroupList = RuleDetector.DetectLocalRules();
            _globalGroupList = RuleDetector.DetectGlobalRules();

            foreach (Action onReload in _onReloadActions)
            {
                onReload();
            }
        }

        public List<string> GetAllGroupKeys()
        {
            if (_allGroupKeys.Count > 0)
            {
                return _allGroupKeys;
            }

            _allGroupKeys.AddRange(
                _builtInRulesByGroup.Keys.Select(x => x.key)
            );

            _allGroupKeys.AddRange(
                _localGroupList.Select(x => x.key)
            );

            _allGroupKeys.AddRange(
                _globalGroupList.Select(x => x.key)
            );

            return _allGroupKeys;
        }

        public List<string> GetAllGroupNames()
        {
            if (_allGroupNames.Count > 0)
            {
                return _allGroupNames;
            }

            _allGroupNames.AddRange(
                _builtInRulesByGroup.Keys.Select(x => x.name)
            );

            _allGroupNames.AddRange(
                _localGroupList.Select(x => x.name)
            );

            _allGroupNames.AddRange(
                _globalGroupList.Select(x => x.name)
            );

            return _allGroupNames;
        }

        public string[] GetAvailableGroups()
        {
            return _localGroupList
                .Select(x => x.name)
                .Union(
                    _globalGroupList.Select(x => x.name)
                ).ToArray();
        }

        public List<string> GetRuleKeysForGroup(string groupName)
        {
            // TODO: Built-In-Rules
            return _globalGroupList
                .Union(_localGroupList)
                .FirstOrDefault(group => group.name == groupName)?
                .rules.Select(rule => rule.key)
                .ToList();
        }

        public List<string> GetRuleDescriptionsForGroup(string groupName)
        {
            return _globalGroupList
                .Union(_localGroupList)
                .FirstOrDefault(group => group.name == groupName)?
                .rules.Select(rule => rule.description)
                .ToList();
        }

        public Group GetGroup(string groupKey)
        {
            return _globalGroupList
                .FirstOrDefault(group => group.key == groupKey);
        }

        public Group GetGroupByName(string groupName)
        {
            return _globalGroupList
                .Union(_localGroupList)
                .FirstOrDefault(group => group.name == groupName);
        }

        public DataScope GetDataScopeByGroupName(string groupName)
        {
            if (_globalGroupList
                .FirstOrDefault(group => group.name == groupName) != null)
            {
                return DataScope.Global;
            }

            return DataScope.Local;
        }
    }
}