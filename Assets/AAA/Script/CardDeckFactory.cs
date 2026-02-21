using System.Collections.Generic;
using UnityEngine;

public sealed class CardDeckFactory : MonoBehaviour
{
    [Header("카드 프리팹(실제 GameObject)")]
    [SerializeField] private Card cardPrefab;

    // 56장 실제 오브젝트 생성 + 데이터 설정 후 반환
    public List<Card> CreateFullDeck(Transform parentForCards)
    {
        var datas = CreateFullDeckData(); // 56개 데이터
        Shuffle(datas);

        var cards = new List<Card>(datas.Count);
        foreach (var d in datas)
        {
            var c = Instantiate(cardPrefab, parentForCards);
            c.Initialize(d);
            cards.Add(c);
        }
        return cards;
    }

    // 56장 데이터 생성 (A~D 각각 14장: 1~4는 3장씩, 5는 2장)
    public List<CardData> CreateFullDeckData()
    {
        var result = new List<CardData>(56);

        foreach (FruitSymbol s in System.Enum.GetValues(typeof(FruitSymbol)))
        {
            // 1~4 : 각 3장
            for (int count = 1; count <= 4; count++)
            {
                result.Add(new CardData(s, count));
                result.Add(new CardData(s, count));
                result.Add(new CardData(s, count));
            }

            // 5 : 2장
            result.Add(new CardData(s, 5));
            result.Add(new CardData(s, 5));
        }

        return result;
    }

    // 플레이어 수(유저 1 + 적 N)에 맞춰 분배 (간단하게 라운드 로빈)
    public void Deal(List<Card> shuffledCards, List<DeckBase> decks)
    {
        int deckCount = decks.Count;
        for (int i = 0; i < shuffledCards.Count; i++)
        {
            decks[i % deckCount].AddCardToBottom(shuffledCards[i]);
        }

        // 각 덱에서 쌓기 정렬 수행
        foreach (var d in decks)
            d.BuildStackVisual();
    }

    private void Shuffle<T>(List<T> list)
    {
        // Fisher-Yates (직관 구현)
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}