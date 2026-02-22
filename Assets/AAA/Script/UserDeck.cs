using UnityEngine;

public sealed class UserDeck : DeckBase
{
    protected override void OnTopCardChanged(Card newTop)
    {
        if (newTop == null) return;
        Debug.Log("OnTopCardChanged: " + newTop.Data.Symbol + "");
        newTop.SetGrabbable(true);

        // 최상단 카드만 잡힐 수 있으니 물리/키네마틱도 정책적으로 분리 가능
        // 여기서는 덱에 있는 동안 다 kinematic 유지
    }
}