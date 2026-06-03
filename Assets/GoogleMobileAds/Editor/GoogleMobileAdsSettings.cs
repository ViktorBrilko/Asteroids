using System.IO;
using UnityEditor;
using UnityEngine;

namespace GoogleMobileAds.Editor
{
    internal class GoogleMobileAdsSettings : ScriptableObject
    {
        public enum GmaAndroidSdk
        {
            Standard = 0,
            NextGen = 1
        }

        private const string MobileAdsSettingsResDir = "Assets/GoogleMobileAds/Resources";

        private const string MobileAdsSettingsFile = "GoogleMobileAdsSettings";

        private const string MobileAdsSettingsFileExtension = ".asset";

        [SerializeField] private string adMobAndroidAppId = string.Empty;

        [SerializeField] private string adMobIOSAppId = string.Empty;

        [SerializeField] private bool enableKotlinXCoroutinesPackagingOption = true;

        [SerializeField] private bool enableGradleBuildPreProcessor = true;

        [SerializeField] private bool disableOptimizeInitialization;

        [SerializeField] private bool disableOptimizeAdLoading;

        [SerializeField] private string userTrackingUsageDescription;

        [SerializeField] private string userLanguage = "en";

        [SerializeField] private bool overrideDefaultGmaAndroidSdk;

        [SerializeField] private int selectedGmaAndroidSdk;

        public string GoogleMobileAdsAndroidAppId
        {
            get => adMobAndroidAppId;

            set => adMobAndroidAppId = value;
        }

        public bool EnableGradleBuildPreProcessor
        {
            get => enableGradleBuildPreProcessor;

            set => enableGradleBuildPreProcessor = value;
        }

        public bool EnableKotlinXCoroutinesPackagingOption
        {
            get => enableKotlinXCoroutinesPackagingOption;

            set => enableKotlinXCoroutinesPackagingOption = value;
        }

        public string GoogleMobileAdsIOSAppId
        {
            get => adMobIOSAppId;

            set => adMobIOSAppId = value;
        }

        public bool DisableOptimizeInitialization
        {
            get => disableOptimizeInitialization;

            set => disableOptimizeInitialization = value;
        }

        public bool DisableOptimizeAdLoading
        {
            get => disableOptimizeAdLoading;

            set => disableOptimizeAdLoading = value;
        }

        public string UserTrackingUsageDescription
        {
            get => userTrackingUsageDescription;

            set => userTrackingUsageDescription = value;
        }

        public string UserLanguage
        {
            get => userLanguage;

            set => userLanguage = value;
        }

        public bool OverrideDefaultGmaAndroidSdk
        {
            get => overrideDefaultGmaAndroidSdk;

            set => overrideDefaultGmaAndroidSdk = value;
        }

        public int SelectedGmaAndroidSdk
        {
            get => selectedGmaAndroidSdk;

            set => selectedGmaAndroidSdk = value;
        }

        /// <summary>
        ///     Returns the active GMA Android SDK architecture.
        ///     This property is decoupled from the stored value to allow easily switching the default
        ///     to Next-Gen in the next phase of migration for users who haven't overridden the default.
        /// </summary>
        public GmaAndroidSdk EffectiveGmaAndroidSdk
        {
            get
            {
                if (overrideDefaultGmaAndroidSdk) return (GmaAndroidSdk)selectedGmaAndroidSdk;
                return GmaAndroidSdk.Standard;
            }
        }

        internal static GoogleMobileAdsSettings LoadInstance()
        {
            // Read from resources.
            var instance = Resources.Load<GoogleMobileAdsSettings>(MobileAdsSettingsFile);

            // Create instance if null.
            if (instance == null)
            {
                Directory.CreateDirectory(MobileAdsSettingsResDir);
                instance = CreateInstance<GoogleMobileAdsSettings>();
                var assetPath = Path.Combine(MobileAdsSettingsResDir,
                    MobileAdsSettingsFile + MobileAdsSettingsFileExtension);
                AssetDatabase.CreateAsset(instance, assetPath);
                AssetDatabase.SaveAssets();
            }

            return instance;
        }
    }
}