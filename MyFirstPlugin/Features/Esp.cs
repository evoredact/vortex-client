using Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VortexClient.Features;

internal class Esp : MonoBehaviour {
    internal static new bool enabled = true;

    void OnGUI() {
        if (!Plugin.IsRoundStarted || !enabled || Controll.pl == null || Controll.csCam == null)
            return;

        var localAdditionalPlayer = Plugin.AdditionalPlayers.FirstOrDefault(playerData => playerData.Key.idx == Controll.pl.idx).Value;
        for (int i = 0; i < Plugin.Players.Count; i++) {
            var player = Plugin.Players[i];
            var additionalPlayer = Plugin.AdditionalPlayers.GetValueOrDefault(player);

            if (additionalPlayer == null || localAdditionalPlayer == null || additionalPlayer.IsAlive == false || player.IsMainPlayer ||
                player.tr == null || player.rbHead == null || player.currweapon == null)
                continue;

            Vector3 position = player.tr.position;
            Vector3 position2 = player.rbHead.position;
            Vector3 position3 = new(position.x, position.y - 2.1f, position.z);
            Vector3 position4 = new(position2.x, position2.y + 0.7f, position2.z);

            Vector3 vector = Controll.csCam.WorldToScreenPoint(position);
            Vector3 vector2 = Controll.csCam.WorldToScreenPoint(position4);
            Vector3 vector3 = Controll.csCam.WorldToScreenPoint(position3);

            if (vector.z > 0f) {
                float num = vector2.y - vector3.y;
                float num3 = num / 2f;

                GUIStyle guistyle = new GUIStyle() {
                    normal = { textColor = Color.white },
                    fontSize = 10,
                    fontStyle = FontStyle.Bold
                };

                GUI.Label(new Rect(vector2.x - 20f, Screen.height - vector2.y - 15f, 200f, 50f), player.name, guistyle);
                GUI.Label(new Rect(vector3.x - 15f, Screen.height - vector3.y + 3f, 200f, 50f), $"{player.health.ToString()} / 100", guistyle);
                //GUI.Label(new Rect(vector2.x - 20f, Screen.height - vector2.y - 25f, 200f, 50f), player.currweapon.weaponname, guistyle);
                Color boxColor = (
                    (Controll.gamemode == 5 && player.team == Controll.pl.team && additionalPlayer.IsZombie == localAdditionalPlayer.IsZombie) ||
                    (Controll.gamemode != 5 && !Plugin.Deathmatches.Contains(Controll.gamemode) && player.team == Controll.pl.team)
                ) ? Color.magenta : Color.red;
                Render.DrawBox(vector3.x - num3 / 2f, Screen.height - vector3.y - num, num3, num, boxColor, 1f);
            }
        }
    }

    public class Render : MonoBehaviour {
        public static Color Color {
            get {
                return GUI.color;
            }
            set {
                GUI.color = value;
            }
        }

        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width) {
            Matrix4x4 matrix = GUI.matrix;
            if (!lineText)
                lineText = new Texture2D(1, 1);

            Color color2 = GUI.color;
            GUI.color = color;
            float num = Vector3.Angle(pointB - pointA, Vector2.right);
            if (pointA.y > pointB.y)
                num = -num;

            GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));
            GUIUtility.RotateAroundPivot(num, pointA);
            GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1f, 1f), lineText);
            GUI.matrix = matrix;
            GUI.color = color2;
        }

        public static void DrawBox(float x, float y, float w, float h, Color color, float thickness) {
            DrawLine(new Vector2(x, y), new Vector2(x + w, y), color, thickness);
            DrawLine(new Vector2(x, y), new Vector2(x, y + h), color, thickness);
            DrawLine(new Vector2(x + w, y), new Vector2(x + w, y + h), color, thickness);
            DrawLine(new Vector2(x, y + h), new Vector2(x + w, y + h), color, thickness);
        }

        public static Texture2D lineText;
    }
}
