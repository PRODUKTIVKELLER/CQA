using System.IO;
using JetBrains.Annotations;
using Produktivkeller.CodeQualityAssurance.Editor.Common;
using Produktivkeller.CodeQualityAssurance.Editor.JqAssistant;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.History
{
    public class HistoryManager
    {
        private readonly FileInfo _fileInfo;
        private History _history;

        internal HistoryManager()
        {
            _fileInfo = new FileInfo(JqaPaths.Instance.BuildCqaHistoryPath());

            LoadHistory();
        }

        [CanBeNull]
        public History LoadHistory()
        {
            if (_history != null)
            {
                return _history;
            }

            if (!_fileInfo.Exists)
            {
                return null;
            }

            string text = FileReader.TryToReadFile(_fileInfo);
            _history = JsonUtility.FromJson<History>(text);
            return _history;
        }

        public void SaveLastSuccessfulAnalysis()
        {
            if (_history == null)
            {
                _history = new History();
            }

            _history.SetLastScanToNow();
            string json = JsonUtility.ToJson(_history);

            if (_fileInfo.Exists)
            {
                _fileInfo.Delete();
            }
            
            File.WriteAllLines(_fileInfo.FullName, new[] {json});
        }
    }
}