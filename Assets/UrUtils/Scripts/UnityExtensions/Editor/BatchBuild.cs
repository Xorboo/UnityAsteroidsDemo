//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;


public class BatchBuild
{
    const string MenuRoot = "Tools/Build/";

    static string GameName { get { return Application.productName; } }

    static string ProjectFolder { get { return Directory.GetParent(Application.dataPath).FullName; } }
    static string BuildLocation { get { return Path.Combine(ProjectFolder, "Build"); } }

    static string AndroidLocation { get { return Path.Combine(BuildLocation, "Android"); } }
    static string AndroidKeystoreFile { get { return Path.Combine(AndroidLocation, "keystore.json"); } }


    #region Android
    [MenuItem(MenuRoot + "Android")]
    public static void BuildnAndroid()
    {
        bool run = EditorUtility.DisplayDialog("Run", "Run after build?", "Yes", "No");
        BuildAndroid(AndroidTargetDevice.ARMv7);
        BuildAndroid(AndroidTargetDevice.x86);
        if (run)
            BuildAndroid(AndroidTargetDevice.FAT, BuildOptions.AutoRunPlayer);
        EditorUtility.RevealInFinder(AndroidLocation);
    }

    public static void BuildAndroid(AndroidTargetDevice target, BuildOptions options = BuildOptions.None)
    {
        Debug.LogFormat("Building for android, target: {0}, options: {1}", target, options);
        Directory.CreateDirectory(AndroidLocation);

        if (!UpdateKeystoreData())
        {
            Debug.LogError("Couldn't complete build");
            return;
        }

        // https://developer.android.com/google/play/publishing/multiple-apks.html
        // Using a version code scheme
        var version = new Version(PlayerSettings.bundleVersion);
        string bundleString = String.Format("{0}{1:00}{2:00}{3:00}", version.Major, version.Minor, version.Build, (int)target);
        int bundle = Convert.ToInt32(bundleString);
        PlayerSettings.Android.bundleVersionCode = bundle;


        PlayerSettings.Android.targetDevice = target;
        
        string file = string.Format("{0}_{1}.apk", GameName, target);
        string fullName = Path.Combine(AndroidLocation, file);

        BuildPipeline.BuildPlayer(GetScenes(), fullName, BuildTarget.Android, options);
        Debug.Log("Build completed");
    }
    #endregion


    #region Android keystore
    static bool UpdateKeystoreData()
    {
        var keystore = GetKeystoreFile();
        if (keystore == null)
            return false;

        PlayerSettings.Android.keystoreName = keystore.Path;
        PlayerSettings.Android.keystorePass = keystore.Password;
        PlayerSettings.Android.keyaliasName = keystore.KeyName;
        PlayerSettings.Android.keyaliasPass = keystore.KeyPassword;
        return true;
    }
    static AndroidKeystoreData GetKeystoreFile()
    {
        if (File.Exists(AndroidKeystoreFile))
        {
            string json = File.ReadAllText(AndroidKeystoreFile);
            return JsonUtility.FromJson<AndroidKeystoreData>(json);
        }
        else
        {
            Debug.LogErrorFormat("Add android keystore file to '{0}'", AndroidKeystoreFile);
            return null;
        }
    }
    class AndroidKeystoreData
    {
        public string Path = "";
        public string Password = "";
        public string KeyName = "";
        public string KeyPassword = "";
    }
    [MenuItem(MenuRoot + "Create Android Keystore")]
    public static void CreateAndroidKeystore()
    {
        if (File.Exists(AndroidKeystoreFile))
        {
            Debug.LogErrorFormat("Android keystore file already exists in '{0}'", AndroidKeystoreFile);
        }
        else
        {
            Directory.CreateDirectory(AndroidLocation);

            var data = new AndroidKeystoreData();
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(AndroidKeystoreFile, json);
            Debug.LogFormat("Android keystore kreated in '{0}'", AndroidKeystoreFile);
        }
    }
    #endregion


    #region Misc
    static string[] GetScenes()
    {
        return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
    }
    #endregion
}
