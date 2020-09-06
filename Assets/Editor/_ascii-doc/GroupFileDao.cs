using System.IO;
using System.Linq;
using System.Reflection;
using Editor._jqa;
using Editor._model;
using Editor._tabs;
using Logging;
using UnityEngine;

namespace Editor
{
    public abstract class GroupFileDao
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Delete(DataScope dataScope, string groupKey)
        {
            string fileName = groupKey+ ".json";
            string path = dataScope == DataScope.Local
                ? JqaPaths.BuildLocalRulesPath()
                : JqaPaths.BuildGlobalRulesPath();

            File.Delete(Path.Combine(path, fileName));

            RuleDao.Instance.ReloadAll();
        }

        public static void Save(DataScope dataScope, Group group)
        {
            string path = dataScope == DataScope.Local
                ? JqaPaths.BuildLocalRulesPath()
                : JqaPaths.BuildGlobalRulesPath();

            FileInfo fileInfo = new FileInfo(Path.Combine(path, group.key + ".json"));
            string json = JsonUtility.ToJson(group);

            Log.Debug("Saving group to {} with JSON {}.", fileInfo.FullName, json);
            File.WriteAllText(fileInfo.FullName, json);

            RuleDao.Instance.ReloadAll();
        }

        public static void Delete(DataScope dataScope, string groupKey, string ruleKey)
        {
            Group group = RuleDao.Instance.GetGroup(groupKey);
            Rule toBeRemoved = group.rules.FirstOrDefault(rule => rule.key == ruleKey);
            group.rules.Remove(toBeRemoved);

            Delete(dataScope, groupKey);
            Save(dataScope, group);
        }
        
        public static void Save(DataScope dataScope, string groupName, Rule rule)
        {
            Group group = RuleDao.Instance.GetGroupByName(groupName);
            group.rules.Add(rule);
            
            Save(dataScope, group);
        }
    }
}