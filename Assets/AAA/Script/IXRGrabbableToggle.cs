using UnityEngine;

public interface IXRGrabbableToggle
{
    // 잡기 가능/불가능 전환 (XRGrabInteractable.enabled 연결하면 끝)
    void SetGrabEnabled(bool enabled);
}