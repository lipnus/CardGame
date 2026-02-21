using UnityEngine;

public sealed class UserDeck : DeckBase
{
    protected override void OnTopCardChanged(Card newTop)
    {
        // 유저 덱: 최상단만 Grab 가능, 나머지는 불가
        for (int i = 0; i < cards.Count; i++)
        {
            bool isTop = (cards[i] == newTop);
            cards[i].SetGrabbable(isTop);
        }

        // 최상단 카드만 잡힐 수 있으니 물리/키네마틱도 정책적으로 분리 가능
        // 여기서는 덱에 있는 동안 다 kinematic 유지
    }
}