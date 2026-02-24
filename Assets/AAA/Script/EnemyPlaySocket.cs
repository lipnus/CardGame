using UnityEngine;

public sealed class EnemyPlaySocket : PlaySocketBase
{
    protected override void OnCardAccepted(Card card)
    {
        // AI는 XR 이벤트가 없으므로, 수락 완료 시점 = "냈다"
        RaiseDropped();
    }
}