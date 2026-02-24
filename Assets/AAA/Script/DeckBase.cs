using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class DeckBase : MonoBehaviour
{
    [Header("덱에 쌓이는 카드 위치 기준")]
    [SerializeField] protected Transform stackRoot;

    [Header("카드 크기/높이 고려")]
    [SerializeField] protected float cardHeight = 0.0025f;

    [Header("살짝 랜덤 쌓기(보기 좋게)")]
    [SerializeField] protected float randomXY = 0.002f;
    [SerializeField] protected float randomYaw = 3f;

    [FormerlySerializedAs("userPlaySocket")]
    [FormerlySerializedAs("cardSocket")]
    [Header("각자의 소켓(카드내는곳)과 연결")] 
    [SerializeField] private PlaySocketBase playSocket;
    
    // 관찰용
    public List<Card> cards = new List<Card>(); // 아래->위 순으로 유지(맨 끝이 top)
    
    public int Count => cards.Count;

    
    
    void Start()
    {
        playSocket.OnCardDropped += HandlePlayDropped;
    }

    void OnDestroy()
    {
        playSocket.OnCardDropped -= HandlePlayDropped;
    }
    
    
    protected abstract void HandlePlayDropped();
    

    
    public void AddCardToBottom(Card card)
    {
        // 덱 소유로 편입
        card.transform.SetParent(stackRoot, worldPositionStays: false);
        
        // 리스트에 추가
        cards.Insert(0, card);
    }

    public Card PeekTop()
    {
        if (cards.Count == 0) return null;
        return cards[cards.Count - 1];
    }

    public Card PopTop()
    {
        if (cards.Count == 0) return null;
        var top = cards[cards.Count - 1];
        cards.RemoveAt(cards.Count - 1);

        // top이 빠졌으니 다음 top 갱신
        OnTopCardChanged(PeekTop());
        return top;
    }

    // 덱 비주얼/물리 배치 (Template Method)
    public void BuildStackVisual()
    {
        for (var i = 0; i < cards.Count; i++)
        {
            var card = cards[i];

            // y는 순서대로 쌓기
            var y = i * cardHeight;

            // 약간의 랜덤으로 자연스럽게
            var rx = Random.Range(-randomXY, randomXY);
            var rz = Random.Range(-randomXY, randomXY);
            var ryaw = Random.Range(-randomYaw, randomYaw);

            card.transform.localPosition = new Vector3(rx, y, rz);
            card.transform.localRotation = Quaternion.Euler(0, ryaw, 180);
        }
        
        Debug.Log("### 빌드 탑");
        
        // 최상단 카드 처리
        OnTopCardChanged(PeekTop());
    }
    
    
    // 공통 흐름은 고정, 각 덱 타입별로 최상단 처리만 다르게
    protected abstract void OnTopCardChanged(Card newTop);
}