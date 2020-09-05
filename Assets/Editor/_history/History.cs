using System;
using UnityEngine;

namespace Editor._history
{
    public class History
    {
        [SerializeField]
        private long _lastScan;

        public DateTime GetLastScan()
        {
            return DateTime.FromFileTimeUtc(_lastScan);
        }

        public void SetLastScanToNow()
        {
            _lastScan = DateTime.Now.ToFileTimeUtc();
        }
    }
}