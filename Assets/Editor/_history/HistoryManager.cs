using System;
using System.IO;
using Editor._common;
using Editor._jqa;
using JetBrains.Annotations;
using UnityEngine;

namespace Editor._history
{
    public class HistoryManager
    {
        private static readonly JqaPaths JqaPaths = new JqaPaths();

        private History _history;
        private FileInfo _fileInfo;

        internal HistoryManager()
        {
            _fileInfo = new FileInfo(JqaPaths.BuildCqaHistoryPath());

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