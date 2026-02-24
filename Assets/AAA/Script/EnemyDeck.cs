using UnityEngine;

public sealed class EnemyDeck : DeckBase
{
    protected override void HandlePlayDropped()
    {
        Debug.Log("적이 카드 냈다");
    }

    protected override void OnTopCardChanged(Card newTop)
    {
        // 적 덱은 사람이 잡지 않으니 전부 grab off
        for (int i = 0; i < cards.Count; i++)
            cards[i].SetGrabbable(false);
    }

    // 외부 호출 시: 카드 한 장을 소켓으로 낸다
    public bool TryPlayTopToSocket(PlaySocketBase socket, out Card playedCard)
    {
        playedCard = null;

        Debug.Log($"TryPlayTopToSocket: this={name}, socket={(socket ? socket.name : "NULL")} cardsCount={(cards==null ? -1 : cards.Count)}");
        var top = PopTop();
        Debug.Log($"PopTop result: {(top ? top.name : "NULL")}");

        if (top == null) return false;

        playedCard = top;
        socket.AcceptCard(top);
        return true;
    }
}