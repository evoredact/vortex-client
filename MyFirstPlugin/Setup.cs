using System.Threading.Tasks;
using UnityEngine;

namespace VortexClient {
    internal static class Setup {
        public static void Run() {
            Plugin.OnGameLoad += OnGameLoad;
        }

        private static async void OnGameLoad() {
            while (!Plugin.IsAuthed) {
                if (Main.gid != 0) {
                    Plugin.TriggerAuthenticate();
                }
                await Task.Delay(1000);
            }
            Plugin.Log.LogWarning("USER authed!");

            //foreach (Controll control in Resources.FindObjectsOfTypeAll<Controll>()) {
            //    Plugin.Control = control;
            //    break;
            //}
        }
    }
}
