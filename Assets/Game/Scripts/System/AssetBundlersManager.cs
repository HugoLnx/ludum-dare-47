using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundlersManager : MonoBehaviour
{
    public static AssetBundlersManager Instance { get; private set; }
    public bool AreAllReady
    {
        get
        {
            foreach(var bundleName in bundleNames)
            {
                if (!IsReady(bundleName)) return false;
            }
            return true;
        }
    }

    public string[] bundleNames;
    private Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            StartCoroutine(DownloadBundles());
        } else
        {
            LoadBundlesFromFile();
        }
    }

    public bool IsReady(string bundleName) => bundles.ContainsKey(bundleName);
    public T Get<T>(string bundleName, string path) where T : UnityEngine.Object
    {
        var bundle = bundles[bundleName];
        return bundle.LoadAsset<T>(path);
    }
    public UnityEngine.Object[] GetAll(string bundleName)
    {
        var bundle = bundles[bundleName];
        return bundle.LoadAllAssets();
    }
    
    private void LoadBundlesFromFile()
    {
        foreach (var bundleName in bundleNames)
        {
            var path = System.IO.Path.Combine(BundleFolderPath, bundleName);
            bundles[bundleName] = AssetBundle.LoadFromFile(path);
        }
    }

    private IEnumerator DownloadBundles()
    {
        foreach(var bundleName in bundleNames)
        {
            var url = System.IO.Path.Combine(BundleFolderPath, bundleName);
            Debug.Log("Bundle URL: " + url);
            using (var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET))
            {
                uwr.downloadHandler = new DownloadHandlerAssetBundle(url, 0);
                yield return uwr.SendWebRequest();
                bundles[bundleName] = DownloadHandlerAssetBundle.GetContent(uwr);
            }
        }
    }

    public static string BundleFolderPath => System.IO.Path.Combine(Application.streamingAssetsPath, "AssetBundles", PlatformFolderName);
    public static string PlatformFolderName => BuildPlatform.ToString();

public static string BuildPlatform {
        get {
#if UNITY_EDITOR
            return EditorUserBuildSettings.activeBuildTarget.ToString();
#elif UNITY_SWITCH
            return "Switch";
#elif UNITY_PS4
            return "PS4";
#elif UNITY_XBOXONE
            return "XboxOne";
#elif UNITY_WEBGL
            return "WebGL";
#elif UNITY_STANDALONE_WIN && UNITY_64
            return "StandaloneWindows64";
#elif UNITY_STANDALONE_LINUX && UNITY_64
            return "StandaloneLinux64";
#elif UNITY_STANDALONE_WIN
            return "StandaloneWindows";
#elif UNITY_STANDALONE_LINUX
            return "StandaloneLinux";
#elif UNITY_STANDALONE_OSX
            return "StandaloneOSX";
#elif UNITY_ANDROID
            return "Android";
#elif UNITY_IOS
            return "iOS";
#else
            return "NoTarget";
#endif
        }
    }

}
