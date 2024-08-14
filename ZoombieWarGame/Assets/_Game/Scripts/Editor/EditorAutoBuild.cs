using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EditorAutoBuild : MonoBehaviour
{
    public enum eBuildType
    {
        Cheat,
        Release
    }
    [MenuItem("Build/Android/Cheat")]
    public static void BuildAndroid_Cheat()
    {
        Build(BuildTarget.Android, eBuildType.Cheat);
    }
    [MenuItem("Build/Android/Release")]
    public static void BuildAndroid_Release()
    {
        Build(BuildTarget.Android, eBuildType.Release);
    }
    [MenuItem("Build/Windows/Cheat")]
    public static void BuildWindows_Cheat()
    {
        Build(BuildTarget.StandaloneWindows64, eBuildType.Cheat);
    }
    [MenuItem("Build/Windows/Release")]
    public static void BuildWindows_Release()
    {
        Build(BuildTarget.StandaloneWindows64, eBuildType.Release);
    }
    private static void Build(BuildTarget target, eBuildType buildType)
    {
        switch (target)
        {
            case BuildTarget.Android:
                BuildAndroid(buildType);
                break;
            case BuildTarget.StandaloneWindows64:
                BuildWindows(buildType);
                break;
        }
    }

    private static void BuildWindows(eBuildType buildType)
    {
        string path = "Builds/Windows/";
        string name = "Windows";
        string extension = ".exe";
        string fullPath = path + name + extension;
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
        buildPlayerOptions.locationPathName = fullPath;
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;
        switch (buildType)
        {
            case eBuildType.Cheat:
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "CHEAT");
                break;
            case eBuildType.Release:
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "");
                break;
        }
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    private static void BuildAndroid(eBuildType buildType)
    {
        UpdateKeyStore();

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        List<string> sceneArrays = new List<string>();

        int count = EditorBuildSettings.scenes.Length;
        for (int i = 0; i < count; i++)
        {
            string path = EditorBuildSettings.scenes[i].path;
            if (EditorBuildSettings.scenes[i].enabled)
            {
                sceneArrays.Add(path);
            }
        }

        string appName = PlayerSettings.productName;

        string customName = "";
        List<string> customScriptingDefine = new List<string>();

        string productName = appName + "_" + customName + PlayerSettings.bundleVersion.Replace(".", "") + "-" + PlayerSettings.Android.bundleVersionCode.ToString() + "_Cheat";

        Debug.Log("Product Name: " + productName);

        string outFolder = "./Builds/android";

        CreateFolder(outFolder);
        string savedPath = outFolder + "/" + productName + ".apk";
        buildPlayerOptions.scenes = sceneArrays.ToArray();
        buildPlayerOptions.locationPathName = savedPath;
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.targetGroup = BuildTargetGroup.Android;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        EditorUtility.RevealInFinder(savedPath);
    }
    static void CreateFolder(string path)
    {
        Directory.CreateDirectory(path);
    }
    static void UpdateKeyStore()
    {
        PlayerSettings.Android.useCustomKeystore = true;
        string fullPath = Application.dataPath + "/" + "Plugins/Android/Keystore/user.keystore";
        try
        {
            DirectoryInfo dir = new DirectoryInfo(fullPath);

            PlayerSettings.Android.keystoreName = dir.FullName.Replace("\\", "/");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Key store not found at" + fullPath + " \n" + ex.StackTrace);
        }

        PlayerSettings.Android.keyaliasName = "cs01";
        PlayerSettings.Android.keystorePass = "SPiAi2001";
        PlayerSettings.Android.keyaliasPass = "SPiAi2001";
    }
}
