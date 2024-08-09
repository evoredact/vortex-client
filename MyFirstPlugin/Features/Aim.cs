using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VortexClient.Features;

internal class Aim : MonoBehaviour {
    // Bugs:
    //  #FIXED random shoots through walls to player position - FIX rayhit name detects name of bodypart of enemy/teammate and triggers
    //  players not removing when died, .Kill function might be not called?
    //   it happens for .Spawn when player joins after me, just somewhy not uses function .Spawn | might assimilar to lower bug
    //  AdditionalPlayer.IsAlive somewhy is false when player is alive
    internal static new bool enabled = false;

    private static int voxelResolution = 3;

    //private static int minRest = 100;
    //private static int maxRest = 190;
    //private static DateTime shootRest = DateTime.Now;
    //private static System.Random random = new();

    private static readonly List<string> bodyPartPriority = new List<string> {
        "head", "body", "RightArmUp", "RightArmDown",
        "LeftArmUp", "LeftArmDown", "RightLegUp",
        "RightLegDown", "RightLegBoot", "LeftLegUp",
        "LeftLegDown", "LeftLegBoot"
    };

    private void OnGUI()
    {
        if (!Plugin.IsRoundStarted || !enabled || Controll.pl == null || Controll.csCam == null || Controll.pl.currweapon == null)
            return;

        float minPlayerDist = -1f;
        Player.PlayerData targetPlayer = null;
        GameObject targetBodyPart = null;
        Vector3 targetHitPoint = Vector3.zero;

        var localAdditionalPlayer = Plugin.AdditionalPlayers.FirstOrDefault(playerData => playerData.Key.idx == Controll.pl.idx).Value;
        for (int i = 0; i < Plugin.Players.Count; i++)
        {
            var player = Plugin.Players[i];
            var additionalPlayer = Plugin.AdditionalPlayers.GetValueOrDefault(player);

            if (additionalPlayer == null || localAdditionalPlayer == null || !additionalPlayer.IsAlive || player.IsMainPlayer ||
                (Controll.gamemode == 5 && player.team == Controll.pl.team && additionalPlayer.IsZombie == localAdditionalPlayer.IsZombie) ||
                (Controll.gamemode != 5 && !Plugin.Deathmatches.Contains(Controll.gamemode) && player.team == Controll.pl.team) ||
                player.spawnprotect || Controll.pl.go == null || player.go == null || Controll.pl.currweapon == null)
                continue;

            float playerToTargetDistance = Vector3.Distance(player.go.transform.position, Controll.pl.go.transform.position);

            foreach (var partName in bodyPartPriority)
            {
                GameObject bodyPart = partName switch
                {
                    "head" => player.goHead,
                    "body" => player.goBody,
                    "RightArmUp" => player.goRArm[0],
                    "RightArmDown" => player.goRArm[1],
                    "LeftArmUp" => player.goLArm[0],
                    "LeftArmDown" => player.goLArm[1],
                    "RightLegUp" => player.goRLeg[0],
                    "RightLegDown" => player.goRLeg[1],
                    "RightLegBoot" => player.goRLeg[2],
                    "LeftLegUp" => player.goLLeg[0],
                    "LeftLegDown" => player.goLLeg[1],
                    "LeftLegBoot" => player.goLLeg[2],
                    _ => null
                };

                if (bodyPart == null)
                    continue;

                if (IsBodyPartVisible(Controll.csCam.transform.position, bodyPart, out Vector3 hitPoint))
                {
                    int hitPriority = bodyPartPriority.IndexOf(partName);

                    if (targetPlayer == null || (minPlayerDist > 0f && playerToTargetDistance < minPlayerDist) ||
                        (minPlayerDist == playerToTargetDistance && hitPriority < bodyPartPriority.IndexOf(targetBodyPart.name)))
                    {
                        minPlayerDist = playerToTargetDistance;
                        targetPlayer = player;
                        targetBodyPart = bodyPart;
                        targetHitPoint = hitPoint;
                        continue;
                    }
                }
            }
        }

        if (targetPlayer != null && targetBodyPart != null)
        {
            Controll.csCam.transform.LookAt(targetHitPoint);

            var hitData = targetBodyPart.GetComponent<HitData>();
            Il2CppSystem.Collections.Generic.List<GameClass.AttackData> attackList = new();
            attackList.Add(new GameClass.AttackData(targetPlayer.idx, hitData.box, targetHitPoint));
            Client.cs.send_attack(Controll.csCam.transform.position, (uint)Time.time, attackList);
        }
    }

    //Client.cs.send_reload();
    //Client.cs.send_reloadactive();
    bool IsBodyPartVisible(Vector3 cameraPosition, GameObject bodyPart, out Vector3 hitPoint) {
        hitPoint = Vector3.zero;
        Bounds bounds = bodyPart.GetComponent<Renderer>().bounds;
        Vector3[] samplePoints = GetSamplePoints(bounds, voxelResolution);

        foreach (var point in samplePoints)
        {
            Ray ray = new Ray(cameraPosition, point - cameraPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity))
            {
                if (hit.collider.gameObject == bodyPart)
                {
                    hitPoint = hit.point;
                    return true;
                }
            }
        }

        return false;
    }

    Vector3[] GetSamplePoints(Bounds bounds, int resolution) {
        List<Vector3> samplePoints = new List<Vector3>();

        float stepX = bounds.size.x / resolution;
        float stepY = bounds.size.y / resolution;
        float stepZ = bounds.size.z / resolution;

        for (int x = 0; x <= resolution; x += 2)
            for (int y = 0; y <= resolution; y += 2)
                for (int z = 0; z <= resolution; z += 2)
                    samplePoints.Add(bounds.min + new Vector3(x * stepX, y * stepY, z * stepZ));

        return samplePoints.ToArray();
    }

    //void OnGUI() {
    //    if (!Plugin.IsRoundStarted || !enabled || Controll.pl == null || Controll.csCam == null || Controll.pl.currweapon == null)
    //        return;

    //    float minPlayerDist = -1f;
    //    Player.PlayerData targetPlayer = null;

    //    for (int i = 0; i < Plugin.Players.Count; i++) {
    //        var player = Plugin.Players[i];

    //        var additionalPlayer = Plugin.AdditionalPlayers.GetValueOrDefault(player);

    //        if (additionalPlayer == null || additionalPlayer.IsAlive == false || player.IsMainPlayer ||
    //            (!Plugin.Deathmatches.Contains(Controll.gamemode) && player.team == Controll.pl.team) || player.spawnprotect ||
    //            Controll.pl.go == null || player.go == null || player.goHead == null || Controll.pl.currweapon == null)
    //            continue;

    //        float playerToTargetDistance = Vector3.Distance(player.go.transform.position, Controll.pl.go.transform.position);

    //        RaycastHit raycastHit;
    //        if (Physics.Raycast(Controll.csCam.transform.position, player.goHead.transform.position - Controll.csCam.transform.position, out raycastHit, float.PositiveInfinity)) {
    //            string name = raycastHit.transform.name;
    //            //Plugin.Log.LogWarning($"raycast hit name: {name}, it owner {player.name}");
    //            if (name == "Map" || name == "MapBackPlatform" || name.Contains("p_weapon") || raycastHit.transform != player.goHead.transform)
    //                continue;
    //            if (name == "head" || name == "body" || name == "RightArmUp" || name == "RightArmDown" || name == "LeftArmUp" || name == "LeftArmDown" || name == "RightLegUp" ||
    //                name == "RightLegDown" || name == "LeftLegUp" || name == "LeftLegDown" || name == "RightLegBoot" || name == "LeftLegBoot") {
    //                if (minPlayerDist > 0f) {
    //                    if (playerToTargetDistance < minPlayerDist) {
    //                        minPlayerDist = playerToTargetDistance;
    //                        targetPlayer = player;
    //                    }
    //                }
    //                else {
    //                    minPlayerDist = playerToTargetDistance;
    //                    targetPlayer = player;
    //                }
    //            }
    //        }
    //    }

    //    if (targetPlayer != null) {
    //        Controll.csCam.transform.LookAt(targetPlayer.goHead.transform.position);
    //        var hitData = targetPlayer.goHead.GetComponent<HitData>();
    //        Il2CppSystem.Collections.Generic.List<GameClass.AttackData> attackList = new();
    //        attackList.Add(new GameClass.AttackData(targetPlayer.idx, hitData.box, targetPlayer.goHead.transform.position));
    //        Client.cs.send_attack(Controll.csCam.transform.position, (uint)Time.time, attackList);
    //        //Client.cs.send_reload();
    //        //Client.cs.send_reloadactive();
    //    }
    //}

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse3)) {
            enabled = !enabled;
            Plugin.Log.LogWarning($"Aim State is now: {enabled}");
        }
    }
}
