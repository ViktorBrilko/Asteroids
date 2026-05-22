using Firebase;
using Firebase.Extensions;
using UnityEngine;
using Zenject;

namespace Analytics.Firebase
{
    public class FirebaseInitializer : IInitializable
    {
        private readonly IAnalyticsService _analyticsService;

        public FirebaseInitializer(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        
        public void Initialize()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => 
            {
                if (task.Result == DependencyStatus.Available) 
                {
                    if (_analyticsService is FirebaseAnalyticsService fbAnalytics)
                    {
                        fbAnalytics.Enable();
                        Debug.Log("Firebase Analytics Enabled");
                    }
                }
            });
        }
    }
}