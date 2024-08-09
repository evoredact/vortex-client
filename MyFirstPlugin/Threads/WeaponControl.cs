using UnityEngine;

namespace VortexClient.Threads;

internal class WeaponControl : MonoBehaviour {
    void Update() {
        if (!Plugin.IsRoundStarted || UIAmmo.Instance == null || UIAmmo.Instance._reloadingProgress == null)
            return;
        if (Patches.ClientControlling.LoopUntilReloadEnd)
            Controll.reload_active = UIAmmo.Instance._reloadingProgress.fillAmount;
    }
}
