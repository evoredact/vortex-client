//using System;
//using System.Collections;
//using System.Collections.Generic;
//using BepInEx.Unity.IL2CPP.Utils.Collections;
//using UnityEngine;

//namespace VortexClient.Features;

//internal class Chams : MonoBehaviour {
//    private Material enemyMaterial;
//    private Material teammateMaterial;
//    private Material occlusionMaterial;

//    private List<Player.PlayerData> renderingPlayers = new();

//    void Awake() {
//        Shader shader = Shader.Find("Hidden/Internal-Colored");
//        // Shader.Find("Sprites/Default")

//        enemyMaterial = new Material(shader);
//        enemyMaterial.SetColor("_Color", Color.red);
//        enemyMaterial.renderQueue = 3000;
//        enemyMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
//        enemyMaterial.SetInt("_ZWrite", 0);
//        enemyMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);

//        teammateMaterial = new Material(shader);
//        teammateMaterial.SetColor("_Color", Color.magenta);
//        teammateMaterial.renderQueue = 3000;
//        teammateMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
//        teammateMaterial.SetInt("_ZWrite", 0);
//        teammateMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);

//        occlusionMaterial = new Material(shader);
//        occlusionMaterial.SetColor("_Color", Color.green);
//        occlusionMaterial.renderQueue = 4000;
//        occlusionMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
//        occlusionMaterial.SetInt("_ZWrite", 0);
//        occlusionMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.LessEqual);

//        //StartCoroutine(UpdateEsp().WrapToIl2Cpp());
//    }

//    private IEnumerable UpdateEsp() {
//        while (true) {
//            if (Plugin.IsRoundStarted)
//                for (int i = 0; i < Plugin.Players.Count; i++) {
//                    var player = Plugin.Players[i];
//                    //if (player != null && !player.IsMainPlayer && !renderingPlayers.Contains(player))
//                        //StartCoroutine(RenderPlayer(player).WrapToIl2Cpp());
//                }
//            yield return new WaitForSeconds(1f);
//        }
//    }

//    private class Highlighter : MonoBehaviour {
//        private Highlighter(IntPtr ptr) : base(ptr) { }
//    }

//    private IEnumerable RenderPlayer(Player.PlayerData player) {
//        if (player.go == null) {
//            yield return new WaitForSeconds(1f);
//        }
//        var renderers = player.go.GetComponentsInChildren<Renderer>();
//        List<Material[]> latestMaterials = new();

//        for (int i = 0; i < renderers.Length; i++)
//            latestMaterials.Add(renderers[i].materials);

//        while (player != null && Plugin.Players.Contains(player)) {
//            if (player.go == null || Plugin.MainPlayerData == null) {
//                yield return new WaitForSeconds(1f);
//                continue;
//            }

//            for (int i = 0; i < renderers.Length; i++) {
//                var renderer = renderers[i];
//                Material[] newMats = new Material[latestMaterials.Count + 2];
//                latestMaterials[i].CopyTo(newMats, 0);
//                newMats[latestMaterials.Count] = player.team == Plugin.MainPlayerData.team ? teammateMaterial : enemyMaterial;
//                newMats[latestMaterials.Count + 1] = occlusionMaterial;
//                renderer.materials = newMats;
//            }

//            //foreach (Renderer renderer in player.go.GetComponentsInChildren<Renderer>()) {
//            //    Material[] newMats = new Material[mats.Length + 2];
//            //    mats.CopyTo(newMats, 0);
//            //    newMats[mats.Length] = playerMaterial;
//            //    newMats[mats.Length + 1] = occlusionMaterial;
//            //    renderer.materials = newMats;
//            //}
//            yield return new WaitForSeconds(1f);
//        }
//        //Highlighter highlighter;
//        //player.transform.TryGetComponent(out highlighter);
//        //if (highlighter == null) {
//        //    foreach (Renderer renderer in player.GetComponentsInChildren<Renderer>()) {
//        //        Material[] mats = renderer.materials;
//        //        Material[] newMats = new Material[mats.Length + 2];
//        //        mats.CopyTo(newMats, 0);
//        //        newMats[mats.Length] = espMaterial["player"];
//        //        newMats[mats.Length + 1] = espMaterial["player2"];
//        //        renderer.materials = newMats;
//        //    }

//        //    highlighter = player.transform.gameObject.AddComponent<Highlighter>();
//        //    highlighter.ConstantOn(Color.red);
//        //    highlighter.occluder = true;
//        //    highlighter.overlay = true;
//        //}
//    }
//}