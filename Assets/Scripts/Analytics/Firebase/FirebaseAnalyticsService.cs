using Firebase.Analytics;
using UnityEngine;

namespace Analytics.Firebase
{
    public class FirebaseAnalyticsService : IAnalyticsService
    {
        private bool _isReady;

        public void LogEvent(string eventName)
        {
            if (!_isReady) return;

            FirebaseAnalytics.LogEvent(eventName);
            Debug.Log(eventName);
        }

        public void Enable()
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            _isReady = true;
        }
    }
}