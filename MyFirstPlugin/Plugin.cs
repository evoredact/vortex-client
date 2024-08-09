using System;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using UnityEngine.SceneManagement;
using HarmonyLib;
using System.Collections.Generic;
using VortexClient.Utils;
using UnityEngine;
using System.Reflection;

namespace VortexClient;

[BepInPlugin("org.redact.plugins.vortexclient", "VortexClient", "0.0.0.0")]
public class Plugin : BasePlugin {
    #region Round Started/Ended Event
    internal static bool IsRoundStarted = false;
    internal delegate void RoundStartedEventHandler();
    internal static event RoundStartedEventHandler OnRoundStart;
    internal static void TriggerRoundStart() {
        IsRoundStarted = true;
        CurrentGravity = Physics.gravity;
        foreach (var additionalPlayer in AdditionalPlayers.Values)
            if (additionalPlayer.IsAlive == false) {
                additionalPlayer.IsAlive = true;
                additionalPlayer.IsZombie = false;
            }
        OnRoundStart?.Invoke();
    }
    internal delegate void RoundEndedEventHandler();
    internal static event RoundEndedEventHandler OnRoundEnd;
    internal static void TriggerRoundEnd() {
        IsRoundStarted = false;
        OnRoundEnd?.Invoke();
    }
    #endregion

    #region GameLoad Event
    internal static bool IsGameLoaded = false;
    internal delegate void GameLoadedEventHandler();
    internal static event GameLoadedEventHandler OnGameLoad;
    internal static void TriggerGameLoad() {
        IsGameLoaded = true;
        OnGameLoad?.Invoke();
    }
    #endregion

    #region Authed Event
    internal static bool IsAuthed = false;
    internal delegate void AuthEventHandler();
    internal static event AuthEventHandler OnAuthenticate;
    internal static void TriggerAuthenticate() {
        IsAuthed = true;
        OnAuthenticate?.Invoke();
    }
    #endregion

    #region Players Control
    // LocalPlayer = Main.Player
    internal static List<Player.PlayerData> Players = new();

    internal class AdditionalPlayerData {
        public bool IsAlive = false;
        public bool IsZombie = false;
    }
    internal static Dictionary<Player.PlayerData, AdditionalPlayerData> AdditionalPlayers = new();
    //internal static Player.PlayerData MainPlayerData = null;
    #endregion

    internal static readonly Assembly CurrentAssembly = Assembly.GetExecutingAssembly();
    internal static readonly string CurrentAssemblyName = CurrentAssembly.FullName.Split(",")[0];
    internal static class Assemblies {
        public static readonly Type[] Threads = Utils.Types.GetTypesInNamespace(CurrentAssembly, $"{CurrentAssemblyName}.Threads");
        public static readonly Type[] Features = Utils.Types.GetTypesInNamespace(CurrentAssembly, $"{CurrentAssemblyName}.Features", false);
    }

    internal static class ProtectedHolders {
        public static readonly GameObject FeaturesHolder = World.CreateProtectedHolder();
        public static readonly GameObject ThreadsHolder = World.CreateProtectedHolder();
    }

    internal static Vector3 CurrentGravity = Physics.gravity;

    internal static List<int> Deathmatches = new() { 8, 2 };

    internal static new ManualLogSource Log;
    public override void Load() {
        Plugin.Log = base.Log;
        Log.LogWarning($"Plugin {MyPluginInfo.PLUGIN_GUID} proceed load event.");

        SceneManager.sceneLoaded += new Action<Scene, LoadSceneMode>(OnSceneLoad);
        SceneManager.sceneUnloaded += new Action<Scene>(OnSceneUnload);

        Harmony harmony = new("CoolPatchers");
        harmony.PatchAll();

        // just setup run
        Setup.Run();

        //OnRoundStart += OnRoundStarted;

        //World.CreateProtectedComponent<DearImGui.ImGuiComponent>();

        // requires auto for features
        //World.CreateProtectedComponent<Features.Chams>();

        foreach (Type feature in Assemblies.Features) {
            if (typeof(MonoBehaviour).IsAssignableFrom(feature))
                World.CreateProtectedComponent(feature, ProtectedHolders.FeaturesHolder);
        }
        foreach (Type thread in Assemblies.Threads) {
            if (typeof(MonoBehaviour).IsAssignableFrom(thread))
                World.CreateProtectedComponent(thread, ProtectedHolders.ThreadsHolder);
        }

        //World.CreateProtectedComponent<Features.Esp>();
        //World.CreateProtectedComponent<Features.Aim>();
        //World.CreateProtectedComponent<Features.FakeLag>();
        //World.CreateProtectedComponent<Features.LagFly>();
        //World.CreateProtectedComponent<Threads.WeaponControl>();

        Log.LogWarning($"Plugin {MyPluginInfo.PLUGIN_GUID} completely loaded load func.");
    }

    //private static async void FindMainPlayerData() {
    //    while (MainPlayerData != null) {
    //        for (int i = 0; i < Players.Count; i++)
    //            if (Players[i] != null && Players[i].IsMainPlayer) {
    //                MainPlayerData = Players[i];
    //                break;
    //            }
    //        await Task.Delay(1000);
    //    }
    //}
    //private void OnRoundStarted() {
    //    Il2CppThreadingHelper.StartThread(new Action(FindMainPlayerData));
    //}

    private static void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        Log.LogWarning($"[!] Loaded scene: {scene.name}, {mode.ToString()}");
        if (scene.name == "main") {
            TriggerGameLoad();
        }
    }
    private static void OnSceneUnload(Scene scene) {
        Log.LogWarning($"[!] Unloaded scene: {scene.name}");
    }
}