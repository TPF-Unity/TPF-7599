using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
        Sprite3D
    }

    [SerializeField] private Mode mode;

    private void LateUpdate() {
        switch (mode) {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
            case Mode.Sprite3D:
                Vector3 cameraForward = Camera.main.transform.forward;
                // cameraForward.x = 0f;
                cameraForward.y = 0f;
                // cameraForward.z = 0f;
                transform.LookAt(cameraForward);
                break;
        }
    }
}
