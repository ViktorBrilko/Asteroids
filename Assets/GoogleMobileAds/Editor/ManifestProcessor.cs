// Copyright (C) 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#if UNITY_ANDROID

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using GoogleMobileAds.Editor;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif

#if UNITY_2018_1_OR_NEWER
public class ManifestProcessor : IPreprocessBuildWithReport
#else
public class ManifestProcessor : IPreprocessBuild
#endif
{
    private const string MANIFEST_RELATIVE_PATH =
        "Plugins/Android/GoogleMobileAdsPlugin.androidlib/AndroidManifest.xml";

    private const string PROPERTIES_RELATIVE_PATH =
        "Plugins/Android/GoogleMobileAdsPlugin.androidlib/project.properties";

    private const string METADATA_APPLICATION_ID =
        "com.google.android.gms.ads.APPLICATION_ID";

    private const string METADATA_DELAY_APP_MEASUREMENT_INIT =
        "com.google.android.gms.ads.DELAY_APP_MEASUREMENT_INIT";

    private const string METADATA_OPTIMIZE_INITIALIZATION =
        "com.google.android.gms.ads.flag.OPTIMIZE_INITIALIZATION";

    private const string METADATA_OPTIMIZE_AD_LOADING =
        "com.google.android.gms.ads.flag.OPTIMIZE_AD_LOADING";

    // LINT.IfChange
    private const string METADATA_UNITY_VERSION = "com.google.unity.ads.UNITY_VERSION";
    // LINT.ThenChange(//depot/google3/javatests/com/google/android/gmscore/integ/modules/admob/tests/robolectric/src/com/google/android/gms/ads/nonagon/signals/StaticDeviceSignalSourceTest.java)

    private readonly XNamespace ns = "http://schemas.android.com/apk/res/android";

    public int callbackOrder => 0;

#if UNITY_2018_1_OR_NEWER
    public void OnPreprocessBuild(BuildReport report)
#else
    public void OnPreprocessBuild(BuildTarget target, string path)
#endif
    {
        var manifestPath = Path.Combine(Application.dataPath, MANIFEST_RELATIVE_PATH);
        var propertiesPath = Path.Combine(Application.dataPath, PROPERTIES_RELATIVE_PATH);

        /*
         * Handle importing GMA via Unity Package Manager.
         */
        var pathUtils =
            ScriptableObject.CreateInstance<EditorPathUtils>();
        if (pathUtils.IsPackageRootPath())
        {
            // pathUtils.GetParentDirectoryAssetPath() returns "Packages/.../GoogleMobileAds" but
            // Plugins is at the same level of GoogleMobileAds so we go up one directory before
            // appending MANIFEST_RELATIVE_PATH.
            var packagesPathPrefix =
                Path.GetDirectoryName(pathUtils.GetParentDirectoryAssetPath());
            manifestPath = Path.Combine(packagesPathPrefix, MANIFEST_RELATIVE_PATH);
            propertiesPath = Path.Combine(packagesPathPrefix, PROPERTIES_RELATIVE_PATH);
        }

        if (AssetDatabase.IsValidFolder("Packages/com.google.ads.mobile"))
            manifestPath = Path.Combine("Packages/com.google.ads.mobile", MANIFEST_RELATIVE_PATH);

        if (!File.Exists(manifestPath))
            manifestPath = Path.Combine(Path.GetDirectoryName(manifestPath), "src", "main",
                "AndroidManifest.xml");

        XDocument manifest = null;
        try
        {
            manifest = XDocument.Load(manifestPath);
        }
#pragma warning disable 0168
        catch (IOException e)
#pragma warning restore 0168
        {
            StopBuildWithMessage("AndroidManifest.xml is missing. Try re-importing the plugin.");
        }

        var elemManifest = manifest.Element("manifest");
        if (elemManifest == null)
            StopBuildWithMessage("AndroidManifest.xml is not valid. Try re-importing the plugin.");

        var elemApplication = elemManifest.Element("application");
        if (elemApplication == null)
            StopBuildWithMessage("AndroidManifest.xml is not valid. Try re-importing the plugin.");

        var instance = GoogleMobileAdsSettings.LoadInstance();
        var appId = instance.GoogleMobileAdsAndroidAppId.Trim();

        if (appId.Length == 0)
            StopBuildWithMessage(
                "Android Google Mobile Ads app ID is empty. Please enter a valid app ID to run ads properly.");

        var metas = elemApplication.Descendants()
            .Where(elem => elem.Name.LocalName.Equals("meta-data"));

        SetMetadataElement(elemApplication,
            metas,
            METADATA_APPLICATION_ID,
            appId);

        SetMetadataElement(elemApplication,
            metas,
            METADATA_OPTIMIZE_INITIALIZATION,
            !instance.DisableOptimizeInitialization,
            true);

        SetMetadataElement(elemApplication,
            metas,
            METADATA_OPTIMIZE_AD_LOADING,
            !instance.DisableOptimizeAdLoading,
            true);

        SetMetadataElement(elemApplication,
            metas,
            METADATA_UNITY_VERSION,
            Application.unityVersion);

        elemManifest.Save(manifestPath);
    }

    private XElement CreateMetaElement(string name, object value)
    {
        return new XElement("meta-data",
            new XAttribute(ns + "name", name), new XAttribute(ns + "value", value));
    }

    private XElement GetMetaElement(IEnumerable<XElement> metas, string metaName)
    {
        foreach (var elem in metas)
        {
            var attrs = elem.Attributes();
            foreach (var attr in attrs)
                if (attr.Name.Namespace.Equals(ns)
                    && attr.Name.LocalName.Equals("name") && attr.Value.Equals(metaName))
                    return elem;
        }

        return null;
    }

    /// <summary>
    ///     Utility for setting a metadata element
    /// </summary>
    /// <param name="elemApplication">application element</param>
    /// <param name="metas">all metadata elements</param>
    /// <param name="metadataName">name of the element to set</param>
    /// <param name="metadataValue">value to set</param>
    private void SetMetadataElement(XElement elemApplication,
        IEnumerable<XElement> metas,
        string metadataName,
        string metadataValue)
    {
        var element = GetMetaElement(metas, metadataName);
        if (element == null)
            elemApplication.Add(CreateMetaElement(metadataName, metadataValue));
        else
            element.SetAttributeValue(ns + "value", metadataValue);
    }

    /// <summary>
    ///     Utility for setting a metadata element
    /// </summary>
    /// <param name="elemApplication">application element</param>
    /// <param name="metas">all metadata elements</param>
    /// <param name="metadataName">name of the element to set</param>
    /// <param name="metadataValue">value to set</param>
    /// <param name="defaultValue">If metadataValue is default, node will be removed.</param>
    private void SetMetadataElement(XElement elemApplication,
        IEnumerable<XElement> metas,
        string metadataName,
        bool metadataValue,
        bool defaultValue = false)
    {
        var element = GetMetaElement(metas, metadataName);
        if (metadataValue != defaultValue)
        {
            if (element == null)
                elemApplication.Add(CreateMetaElement(metadataName, metadataValue));
            else
                element.SetAttributeValue(ns + "value", metadataValue);
        }
        else
        {
            if (element != null) element.Remove();
        }
    }

    private void StopBuildWithMessage(string message)
    {
        var prefix = "[GoogleMobileAds] ";
#if UNITY_2017_1_OR_NEWER
        throw new BuildPlayerWindow.BuildMethodException(prefix + message);
#else
        throw new OperationCanceledException(prefix + message);
#endif
    }
}
#endif