using UnityEngine;

namespace VortexClient.Features;

internal class FakeLag : MonoBehaviour {
    public static new bool enabled = false;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            enabled = !enabled;
            Plugin.Log.LogWarning($"FakeLag State is now: {enabled}");
        }
    }
}
