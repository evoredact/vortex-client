using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace VortexClient.Patches;

[HarmonyPatch]
internal class ClientControlling {
    #region Round End Hook
    [HarmonyPatch(typeof(HUD), nameof(HUD.SetMessage))]
    [HarmonyPrefix]
    private static void HUD_SetMessage(string msg, int sec) {
        if (msg == GUIPlay.sRoundLose) {
            //Plugin.Log.LogWarning("Round Lost");
            Plugin.TriggerRoundEnd();
        }
        else if (msg == GUIPlay.sRoundWon) {
            //Plugin.Log.LogWarning("Round Won");
            Plugin.TriggerRoundEnd();
        }
        else if (msg == GUIPlay.sHumansWin) {
            //Plugin.Log.LogWarning("Humans Win");
            Plugin.TriggerRoundEnd();
        }
        else if (msg == GUIPlay.sZombiesWin) {
            //Plugin.Log.LogWarning("Zombies Win");
            Plugin.TriggerRoundEnd();
        }
    }
    #endregion

    #region Always Perfect Reloading/Fast Reloading
    internal static bool LoopUntilReloadEnd = false;
    [HarmonyPatch(typeof(Controll), nameof(Controll.ReloadWeapon))]
    [HarmonyPostfix]
    private static void Controll_ReloadWeapon() {
        if (Controll.inReload && !LoopUntilReloadEnd) {
            LoopUntilReloadEnd = true;
            //Plugin.Log.LogWarning("Reload start");
            //_ = Task.Run(async () => {
            //    while (LoopUntilReloadEnd) {
            //        if (!Plugin.IsRoundStarted || UIAmmo.Instance == null || UIAmmo.Instance._reloadingProgress == null)
            //            break;
            //        Controll.reload_active = UIAmmo.Instance._reloadingProgress.fillAmount;
            //        await Task.Delay(50);
            //    }
            //});
        }
    }
    [HarmonyPatch(typeof(Controll), nameof(Controll.ReloadWeaponEnd))]
    [HarmonyPrefix]
    private static void Controll_ReloadWeaponEnd() {
        if (LoopUntilReloadEnd) {
            LoopUntilReloadEnd = false;
            //Plugin.Log.LogWarning("Reload end");
        }
    }
    #endregion

    //[HarmonyPatch(typeof(Main), nameof(Main.isSTEAM))]
    //[HarmonyPrefix]
    //private static bool Main_isSTEAM(ref bool __result) {
    //    __result = false;
    //    return false;
    //}
    //[HarmonyPatch(typeof(Main), nameof(Main.isPOKI))]
    //[HarmonyPrefix]
    //private static bool Main_isPOKI(ref bool __result) {
    //    __result = true;
    //    return false;
    //}

    //#region Aim Helper
    //[HarmonyPatch(typeof(Controll), nameof(Controll.UpdateLook))]
    //[HarmonyPrefix]
    //private static bool Controll_UpdateLook() {
    //    if (Features.Aim.enabled && Plugin.IsRoundStarted && Features.Aim.AimPosition != Vector3.zero && Controll.csCam != null) {
    //        Controll.csCam.transform.LookAt(Features.Aim.AimPosition);
    //        //Features.Aim.LeftClick();
    //        PLH.AttackDamage();
    //        //Plugin.Log.LogWarning($"RX: {Controll.rx}, {Controll.csCam.transform.rotation.x}, RY: {Controll.ry}, {Controll.csCam.transform.rotation.y}");
    //        //Controll.rx = Controll.csCam.transform.rotation.x;
    //        //Controll.ry = Controll.csCam.transform.rotation.y;
    //        return false;
    //    }
    //    return true;
    //}
    //#endregion

    //private static bool LoopCameraSetUntilSpecialAttackEnd = false;
    //[HarmonyPatch]
    //internal class Controll_UseSpecial {
    //    private static MethodBase TargetMethod() {
    //        return typeof(Controll).GetMethods().FirstOrDefault(method => method.Name == "UseSpecial");
    //    }
    //    private static void Prefix() {
    //        LoopCameraSetUntilSpecialAttackEnd = true;

    //        Player.PlayerData targetPlayer = null;
    //        int playersNearby = -1;
    //        for (int i = 0; i < Plugin.Players.Count; i++) {
    //            var player = Plugin.Players[i];
    //            if (player == Controll.pl || player.team == Controll.pl.team || player.go == null)
    //                continue;

    //            int thisPlayersNearby = 0;
    //            for (int i2 = 0; i2 < Plugin.Players.Count; i2++) {
    //                var player2 = Plugin.Players[i2];
    //                if (player2 == Controll.pl || player == player2 || player2.team == Controll.pl.team || player2.go == null)
    //                    continue;

    //                if (Vector3.Distance(player.go.transform.position, player2.go.transform.position) < 9f)
    //                    thisPlayersNearby++;
    //            }

    //            if (playersNearby < thisPlayersNearby) {
    //                targetPlayer = player;
    //                playersNearby = thisPlayersNearby;
    //            }
    //        }

    //        //if (targetPlayer != null)
    //        //    pos = targetPlayer.go.transform.position;

    //        if (targetPlayer != null)
    //            Task.Run(async () => {
    //                Vector3 prevCameraPos = Controll.csCam.transform.position;
    //                Quaternion prevCameraRot = Controll.csCam.transform.rotation;
    //                while (LoopCameraSetUntilSpecialAttackEnd) {
    //                    Controll.csCam.transform.position = targetPlayer.go.transform.position;
    //                    Controll.csCam.transform.rotation = Quaternion.Euler(90, 0, 0);
    //                    await Task.Delay(100);
    //                }
    //                Controll.csCam.transform.position = prevCameraPos;
    //                Controll.csCam.transform.rotation = prevCameraRot;
    //            });
    //    }
    //    private static void Postfix() {
    //        LoopCameraSetUntilSpecialAttackEnd = false;
    //    }
    //}
    //[HarmonyPatch(typeof(Client), nameof(Client.cs.send_attackthrow))]
    //[HarmonyPrefix]
    //private static void Client_send_attackthrow(ref Vector3 pos, ref Vector3 force, float rotation) {
    //    Plugin.Log.LogWarning($"Client send throw attack ({pos.x}, {pos.y}, {pos.z}), ({force.x}, {force.y}, {force.z}), {rotation}");
    //    force = Vector3.zeroVector;
    //    Player.PlayerData targetPlayer = null;
    //    int playersNearby = -1;
    //    for (int i = 0; i < Plugin.Players.Count; i++) {
    //        var player = Plugin.Players[i];
    //        if (player == Controll.pl || player.team == Controll.pl.team || player.go == null)
    //            continue;

    //        int thisPlayersNearby = 0;
    //        for (int i2 = 0; i2 < Plugin.Players.Count; i2++) {
    //            var player2 = Plugin.Players[i2];
    //            if (player2 == Controll.pl || player == player2 || player2.team == Controll.pl.team || player2.go == null)
    //                continue;
    //            if (Vector3.Distance(player.go.transform.position, player2.go.transform.position) < 9f)
    //                thisPlayersNearby++;
    //        }

    //        if (playersNearby < thisPlayersNearby) {
    //            targetPlayer = player;
    //            playersNearby = thisPlayersNearby;
    //        }
    //    }

    //    if (targetPlayer != null)
    //        pos = targetPlayer.go.transform.position;
    //}

    //[HarmonyPatch(typeof(Client), nameof(Client.recv_detonate))] // accepts detonate, useless can't dodge detonate
    //[HarmonyPrefix]
    //private static bool Client_recv_detonate() {
    //    Plugin.Log.LogWarning(nameof(Client.recv_detonate));
    //}
    //[HarmonyPatch(typeof(Client), nameof(Client.recv_attackspecial))] // accepts special
    //[HarmonyPrefix]
    //private static void Client_recv_attackspecial() {
    //    Plugin.Log.LogWarning(nameof(Client.recv_attackspecial));
    //}
    //[HarmonyPatch(typeof(CharAnimator), nameof(CharAnimator.UpdateBody))]
    //[HarmonyPrefix]
    //private static void MCharAnimator_AnimateChar(Player.PlayerData p) {
    //    if (p == Controll.pl) {
    //        //Plugin.Log.LogWarning($"a");
    //        CharAnimator.head_angle = 90f;
    //        //Plugin.Log.LogWarning($"a2");
    //        CharAnimator.default_angle = 90f;
    //        //Plugin.Log.LogWarning($"a3");
    //        CharAnimator.crouch_angle = 90f;
    //        //Plugin.Log.LogWarning($"a4");
    //        CharAnimator.crouchmove_angle = 90f;
    //        //Plugin.Log.LogWarning($"a5");
    //        if (p.cjHead != null)
    //            p.cjHead.axis = new(90f, 90f, 90f);
    //        p.goHead.transform.eulerAngles = new(90f, 90f, 90f);
    //        p.restoreHead = new(90f, 90f, 90f);
    //    }
    //}

    //private static int minLag = 400;
    //private static int maxLag = 800;
    //private static DateTime lagEnd = DateTime.Now;
    //private static DateTime lagRest = DateTime.Now;
    //private static int lagMS = 0;
    //[HarmonyPatch(typeof(Controll), nameof(Controll.UpdateSendPos))]
    //[HarmonyPrefix]
    //private static bool Controll_UpdateSendPos() {
    //    //if (Features.FakeLag.enabled) {
    //    //    Controll.Pos = Controll.csCam.transform.position + new Vector3(UnityEngine.Random.Range(-6f, 6f), 0, UnityEngine.Random.Range(-6f, 6f));
    //    //}
    //    if (Features.Fly.enabled) {
    //        return false;
    //    }
    //    return true;
    //    //DateTime currentTime = DateTime.Now;
    //    //if (currentTime >= lagEnd) {
    //    //    int lagMS = UnityEngine.Random.Range(minLag, maxLag);
    //    //    //int restMS = UnityEngine.Random.Range(minLag, maxLag);
    //    //    lagEnd = currentTime.AddMilliseconds(lagMS);
    //    //    //lagRest = currentTime.AddMilliseconds(lagMS + restMS);
    //    //    //Controll.pl.updatepos = true;
    //    //    return true;
    //    //}
    //    //Controll.pl.updatepos = false;
    //    //return false;
    //}
    //[HarmonyPatch]
    //internal class Controll_SetPos {
    //    private static MethodBase TargetMethod() {
    //        return typeof(Controll).GetMethods().FirstOrDefault(method => method.Name == "SetPos" &&
    //            method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(Vector3));
    //    }
    //    private static bool Prefix(Vector3 p) {
    //        DateTime currentTime = DateTime.Now;
    //        if (currentTime >= lagEnd)
    //            return true;
    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(CharAnimator), nameof(CharAnimator.UpdateBody))]
    //[HarmonyPrefix]
    //private static void CharAnimator_UpdateBody(Player.PlayerData p) {
    //    CharAnimator.head_angle = 50f;
    //    CharAnimator.crouchmove_angle = 50f;
    //}
}
