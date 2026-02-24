using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public sealed class UserPlaySocket : PlaySocketBase
{
    [SerializeField] private XRSocketInteractor socket;

    private void Reset()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    private void OnEnable()
    {
        if (!socket) socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnDisable()
    {
        if (socket) socket.selectEntered.RemoveListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        var interactable = args.interactableObject;
        if (interactable == null) return;

        var card = interactable.transform.GetComponent<Card>();
        if (!card) return;

        // 카드 더미에 쌓기 + 정책 적용
        AcceptCard(card);

        // 소켓이 계속 잡고 있으면 다음 카드 못 받는 케이스 방지
        StartCoroutine(ForceReleaseNextFrame(interactable));

        // 유저는 XR 이벤트 기준으로 "냈다"
        RaiseDropped();
    }

    protected override void ApplyInteractionPolicy(Card card)
    {
        // 기존 유지: 더미에 들어간 카드는 다시 집히면 안 됨
        var grab = card.GetComponent<XRGrabInteractable>();
        if (grab) grab.enabled = false;

        if (card.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        foreach (var col in card.GetComponentsInChildren<Collider>())
            col.enabled = false;

        card.SetGrabbable(false);
        card.SetKinematic(true);
    }

    private IEnumerator ForceReleaseNextFrame(IXRSelectInteractable interactable)
    {
        yield return null;

        if (socket && socket.interactionManager && socket.hasSelection)
            socket.interactionManager.SelectExit(socket, interactable);
    }
}