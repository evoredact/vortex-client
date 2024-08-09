using HarmonyLib;
using System.Collections.Generic;

namespace VortexClient.Patches;

[HarmonyPatch]
internal class PlayerControlling {
    #region Player Spawn/Kill/Join/Leave/Clearing Hooks
    [HarmonyPatch(typeof(PLH), nameof(PLH.Spawn))]
    [HarmonyPrefix]
    private static void PLH_Spawn(int id, int x, int y, int z, float ry, int weaponset) {
        Plugin.Log.LogWarning("Spawn called");
        for (int i = 0; i < Plugin.Players.Count; i++) {
            var player = Plugin.Players[i];
            if (player.idx == id) {
                if (!Plugin.IsRoundStarted) {
                    //Plugin.Log.LogWarning("Round Start");
                    Plugin.TriggerRoundStart();
                }
                var additionalPlayer = Plugin.AdditionalPlayers.GetValueOrDefault(player);
                if (additionalPlayer != null) {
                    additionalPlayer.IsAlive = true;
                    additionalPlayer.IsZombie = false;
                }
                Plugin.Log.LogWarning($"Player spawned: {player.name}, {additionalPlayer != null} = {additionalPlayer != null && additionalPlayer.IsAlive}");
                break;
            }
        }
    }

    [HarmonyPatch(typeof(PLH), nameof(PLH.Kill))]
    [HarmonyPrefix]
    private static void PLH_Kill(int id, int aid, int hitbox) {
        Plugin.Log.LogWarning("Kill called");
        for (int i = 0; i < Plugin.Players.Count; i++) {
            var player = Plugin.Players[i];
            if (player.idx == id) {
                var additionalPlayer = Plugin.AdditionalPlayers.GetValueOrDefault(player);
                if (additionalPlayer != null) {
                    additionalPlayer.IsAlive = false;
                    additionalPlayer.IsZombie = false;
                }
                Plugin.Log.LogWarning($"Player killed: {player.name}, {additionalPlayer != null} = {additionalPlayer != null && additionalPlayer.IsAlive}");
                break;
            }
        }
    }


    [HarmonyPatch(typeof(PLH), nameof(PLH.Add))]
    [HarmonyPostfix]
    private static void PLH_Add(int id, string name, int team, int cp0, int cp1, int cp2, int cp3, int cp4, int cp5, int cp6, int cp7, int cp8, int cp9, int pexp, int cid, string cname) {
        for (var i = 0; i < PLH.player.Length; i++) {
            var player = PLH.player[i];
            // for some reason player might be null
            if (player != null && player.name == name) {
                //if (!Plugin.IsRoundStarted) {
                //    //Plugin.Log.LogWarning("Round Start");
                //    Plugin.TriggerRoundStart();
                //}

                var additionalPlayer = new Plugin.AdditionalPlayerData();
                Plugin.AdditionalPlayers.Add(player, additionalPlayer);

                Plugin.Log.LogWarning($"Player added: {name}");
                Plugin.Players.Add(player);
                break;
            }
        }
    }

    [HarmonyPatch(typeof(PLH), nameof(PLH.Delete))]
    [HarmonyPrefix]
    private static void PLH_Delete(int id) {
        for (var i = 0; i < PLH.player.Length; i++) {
            var player = PLH.player[i];
            // for some reason player might be null
            if (player != null && player.idx == id) {
                Plugin.Log.LogWarning($"Player removed: {player.name}");
                Plugin.Players.Remove(player);
                Plugin.AdditionalPlayers.Remove(player);
                break;
            }
        }
    }

    [HarmonyPatch(typeof(PLH), nameof(PLH.Clear))]
    [HarmonyPrefix]
    private static void PLH_Clear() {
        //for (var i = 0; i < Plugin.Players.Count; i++) {
        //    var player = Plugin.Players[i];
        //    // for some reason player might be null
        //    if (player != null)
        //        Plugin.Log.LogWarning($"Player removed: {player.name}");
        //    else
        //        Plugin.Log.LogWarning($"Player removed: <UNKNOWN_PLAYER>");
        //}
        Plugin.Log.LogWarning("Players cleared");
        Plugin.Players.Clear();
        Plugin.AdditionalPlayers.Clear();
    }
    #endregion

    [HarmonyPatch(typeof(PLH), nameof(PLH.SetZombie))]
    [HarmonyPrefix]
    private static void PLH_SetZombie(int id) {
        for (int i = 0; i < Plugin.Players.Count; i++) {
            var player = Plugin.Players[i];
            if (player.idx == id) {
                var additionalPlayer = Plugin.AdditionalPlayers.GetValueOrDefault(player);
                if (additionalPlayer != null)
                    additionalPlayer.IsZombie = true;
            }
        }
    }
}