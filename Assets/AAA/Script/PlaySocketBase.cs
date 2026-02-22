using UnityEngine;

public abstract class PlaySocketBase : MonoBehaviour
{
    [SerializeField] protected Transform dropPoint;
    [SerializeField] protected float settleHeight = 0.001f;

    public Card CurrentCard { get; private set; }

    // 카드 수락(덱/손에서 소켓으로 이동)
    public virtual void AcceptCard(Card card)
    {
        CurrentCard = card;

        // 소켓 소유로 편입
        card.transform.SetParent(dropPoint, worldPositionStays: false);

        // 위치/회전 스냅
        card.transform.localPosition = new Vector3(0, settleHeight, 0);
        card.transform.localRotation = Quaternion.identity;

        // 소켓에 올라간 카드는 물리 반응 켤지/끌지 정책 결정
        card.SetGrabbable(false);
        card.SetKinematic(true);

        OnCardAccepted(card);
    }

    protected virtual void OnCardAccepted(Card card) { }
}