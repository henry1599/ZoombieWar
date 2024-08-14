using System;
using HenryDev;
using UnityEngine;

public static class PlayerEvents
{
    public static Action<float> OnExpCollected;
    public static Action OnMapGenerated;


    // * UI Subscriber
    public static Action<float> OnExpGain;
    public static Action<int, float> OnLevelUp;
    public static Action<IChangeableValue> OnHealthInit;
    public static Action<IChangeableValue> OnExpInit;
}