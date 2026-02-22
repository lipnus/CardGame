using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using Random = UnityEngine.Random;

public class CardSocket : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private XRSocketInteractor xrSocketInteractor;
    [SerializeField] private Transform pileRoot;

    [Header("Stack Pose")]
    [SerializeField] private Vector3 localOffsetPerCard = new Vector3(0f, 0.002f, 0f); // 카드 두께만큼
    [SerializeField] private float randomYawDeg = 2.0f;

    public event Action OnCardDropped;
    private int count;

    private void Reset()
    {
        xrSocketInteractor = GetComponent<XRSocketInteractor>();
    }

    private void OnEnable()
    {
        if (!xrSocketInteractor) xrSocketInteractor = GetComponent<XRSocketInteractor>();
        xrSocketInteractor.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnDisable()
    {
        if (xrSocketInteractor) xrSocketInteractor.selectEntered.RemoveListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        var interactable = args.interactableObject;
        if (interactable == null) return;
        
        Debug.Log("OnSelectEntered");

        // 카드 컴포넌트
        var cardGo = interactable.transform.gameObject;
        var grab = cardGo.GetComponent<XRGrabInteractable>();

        // 1) 더미로 스냅 (부모 변경 + 포즈)
        SnapToPile(cardGo.transform);

        // 2) 다시 집지 못하게 만들기 (선택: 프로젝트 정책에 맞게)
        DisableCardInteraction(cardGo, grab);

        // 3) 소켓에서 즉시 해제 -> 다음 카드 받기 가능
        StartCoroutine(ForceReleaseNextFrame(interactable));
        
        // 4) 카드 놓아졌다고 콜백 호출
        OnCardDropped?.Invoke();
    }

    private void SnapToPile(Transform card)
    {
        if (!pileRoot) return;

        card.SetParent(pileRoot, worldPositionStays: false);

        // 카드 N장째 포즈
        var basePos = localOffsetPerCard * count;
        card.localPosition = basePos;

        // 약간 랜덤 yaw
        var yaw = Random.Range(-randomYawDeg, randomYawDeg);
        card.localRotation = Quaternion.Euler(0f, yaw, 0f);

        count++;
    }

    private void DisableCardInteraction(GameObject cardGo, XRGrabInteractable grab)
    {
        // 가장 안전한 조합: Grab 끄고, 물리도 멈춤
        if (grab) grab.enabled = false;

        if (cardGo.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 더미 안에서 다시 소켓/핸드에 걸리지 않게 콜라이더 끄기 (필요시)
        foreach (var col in cardGo.GetComponentsInChildren<Collider>())
            col.enabled = false;
    }

    private IEnumerator ForceReleaseNextFrame(IXRSelectInteractable interactable)
    {
        // 같은 프레임에 부모/컴포넌트 끄면 XRI 내부 상태랑 충돌하는 케이스가 있어서 1프레임 늦춤
        yield return null;

        if (xrSocketInteractor != null && xrSocketInteractor.interactionManager != null && xrSocketInteractor.hasSelection)
        {
            // 인터랙션 매니저를 통해 강제 SelectExit
            xrSocketInteractor.interactionManager.SelectExit(xrSocketInteractor, interactable);
        }
    }
}