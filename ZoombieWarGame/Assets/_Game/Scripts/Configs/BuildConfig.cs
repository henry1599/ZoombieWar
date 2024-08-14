using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Build Configs")]
public class BuildConfig : ScriptableObject
{
    public bool isCheat;
    public bool shouldExportProject;
    public bool isCleanCache;
    public bool isProfiler;
    public string bundleID;
    public string customName;
    public string bundleVersion;
    public string bundleCode;
    public List<string> Flags;
}