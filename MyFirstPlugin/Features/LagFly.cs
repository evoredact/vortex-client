using UnityEngine;

namespace VortexClient.Features;

internal class LagFly : MonoBehaviour {
    public static new bool enabled = false;

    private static Vector3 pos = Vector3.zero;

    private void Update() {
        // maybe add if (Input.GetKey(KeyCode.Mouse4))?
        if (Input.GetKeyDown(KeyCode.C)) {
            enabled = !enabled;
            if (enabled) {
                Physics.gravity = Vector3.zero;
            } else {
                Physics.gravity = Plugin.CurrentGravity;
                pos = Vector3.zero;
            }
            Plugin.Log.LogWarning($"LagFly State is now: {enabled}");
        }

        if (!Plugin.IsRoundStarted || !enabled || Controll.pl == null || Controll.pl.go == null || Controll.csCam == null)
            return;

        if (pos == Vector3.zero)
            pos = Controll.csCam.transform.position - new Vector3(0f, 2.2f, 0f);

        Transform cameraTransform = Controll.csCam.transform;

        if (Input.GetAxis("Vertical") != 0f)
            pos += cameraTransform.forward * Time.deltaTime * Input.GetAxis("Vertical") * 22;

        if (Input.GetAxis("Horizontal") != 0f)
            pos += cameraTransform.right * Time.deltaTime * Input.GetAxis("Horizontal") * 22;

        if (Input.GetKey(KeyCode.E))
            pos += Vector3.up * Time.deltaTime * 17;

        if (Input.GetKey(KeyCode.Q))
            pos += Vector3.down * Time.deltaTime * 17;

        //Controll.csCam.transform.position = pos;
        Controll.Pos = pos;
        Controll.currPos = pos;
        Controll.prevPos = pos;
    }
}
