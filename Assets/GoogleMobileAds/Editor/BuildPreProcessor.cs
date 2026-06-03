using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace GoogleMobileAds.Editor
{
    /// <summary>
    ///     Pre-processor that performs common setup tasks for all platforms before a build.
    /// </summary>
    public class BuildPreProcessor : IPreprocessBuildWithReport
    {
        private static readonly string _linkXmlAssetsPath =
            Path.Combine(Application.dataPath, "GoogleMobileAds", "link.xml");

        // Set the callback order to be before EDM4U.
        // https://github.com/googlesamples/unity-jar-resolver/blob/master/source/AndroidResolver/src/PlayServicesPreBuild.cs#L39
        public int callbackOrder => -1;

        public void OnPreprocessBuild(BuildReport report)
        {
            // Unity's managed code stripping process does not inherently process `link.xml` files
            // in UPM packages. This pre-processor copies the `link.xml` file from the UPM package
            // to the Unity project's `Assets/GoogleMobileAds` directory if it does not exist.
            if (!File.Exists(_linkXmlAssetsPath)) CopyLinkXml();
        }

        private static void CopyLinkXml()
        {
            if (!AssetDatabase.IsValidFolder(Path.Combine("Assets", "GoogleMobileAds")))
                AssetDatabase.CreateFolder("Assets", "GoogleMobileAds");
            var pathUtils = ScriptableObject.CreateInstance<EditorPathUtils>();
            if (pathUtils.IsPackageRootPath())
            {
                var parentDirectoryPath = pathUtils.GetParentDirectoryAssetPath();
                var linkXmlPackagePath = Path.Combine(parentDirectoryPath, "link.xml");
                if (string.IsNullOrEmpty(linkXmlPackagePath))
                {
                    Debug.LogWarning("link.xml not found in the package.");
                    return;
                }

                AssetDatabase.CopyAsset(linkXmlPackagePath, _linkXmlAssetsPath);
            }

            AssetDatabase.Refresh();
        }
    }
}