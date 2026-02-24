using System;
using UnityEngine;

public abstract class PlaySocketBase : MonoBehaviour
{
    [Header("Pile (필수)")]
    [SerializeField] protected Transform pileParent;          // 카드가 쌓일 부모
    [SerializeField] protected Vector3 pileStep = new(0f, 0.002f, 0f); // 한 장당 오프셋(두께)
    [SerializeField] protected float yawJitterDeg = 2f;        // 약간 비틀기

    public event Action OnCardDropped;
    public Card CurrentCard { get; private set; }

    protected int pileCount;

    protected void RaiseDropped() => OnCardDropped?.Invoke();

    /// <summary>
    /// 카드 1장을 "낸 카드 더미"에 쌓는다.
    /// (배치 + 정책 + 후처리까지 단일 파이프라인)
    /// </summary>
    public void AcceptCard(Card card)
    {
        if (!card)
        {
            Debug.LogError($"{name}: AcceptCard(card=null)", this);
            return;
        }
        if (!pileParent)
        {
            Debug.LogError($"{name}: pileParent not assigned", this);
            return;
        }

        CurrentCard = card;

        // 1) 더미에 쌓기 (non-virtual: 재귀/스택오버플로우 구조적으로 불가)
        PlaceOnPile(card);

        // 2) 인터랙션/물리 정책
        ApplyInteractionPolicy(card);

        // 3) 파생 클래스 후처리(턴 진행 등)
        OnCardAccepted(card);
    }

    // "쌓기"는 여기서만 한다. 오버라이드 금지(=재귀 구멍 제거).
    private void PlaceOnPile(Card card)
    {
        card.transform.SetParent(pileParent, false);
        card.transform.localPosition = pileStep * pileCount;

        float yaw = UnityEngine.Random.Range(-yawJitterDeg, yawJitterDeg);
        card.transform.localRotation = Quaternion.Euler(0f, yaw, 0f);

        pileCount++;
    }

    /// <summary>
    /// 더미에 들어간 카드가 다시 집히지/움직이지 않게 하는 기본 정책
    /// </summary>
    protected virtual void ApplyInteractionPolicy(Card card)
    {
        card.SetGrabbable(false);
        card.SetKinematic(true);
    }

    /// <summary>
    /// 카드가 "수락 완료"된 직후 호출됨
    /// </summary>
    protected virtual void OnCardAccepted(Card card) { }
}