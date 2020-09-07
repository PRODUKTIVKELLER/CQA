using System;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.History
{
    [Serializable]
    public class History
    {
        [SerializeField]
        private long lastScan;

        public DateTime GetLastScan()
        {
            return DateTime.FromFileTimeUtc(lastScan);
        }

        public void SetLastScanToNow()
        {
            lastScan = DateTime.Now.ToFileTimeUtc();
        }
    }
}