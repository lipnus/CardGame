using System.Collections.Generic;

public sealed class TableRuleEvaluator
{
    // 테이블(각 플레이어 소켓)에 올라온 카드들을 받아서
    // "같은 문양 합이 5"가 있는지 반환
    public bool HasFiveCombo(List<PlaySocketBase> sockets, out CardSymbol symbolHit)
    {
        symbolHit = CardSymbol.A;

        var sum = new Dictionary<CardSymbol, int>()
        {
            {CardSymbol.A, 0},
            {CardSymbol.B, 0},
            {CardSymbol.C, 0},
            {CardSymbol.D, 0},
        };

        for (int i = 0; i < sockets.Count; i++)
        {
            var card = sockets[i].CurrentCard;
            if (card == null) continue;

            sum[card.Data.Symbol] += card.Data.Count;
        }

        foreach (var kv in sum)
        {
            if (kv.Value == 5)
            {
                symbolHit = kv.Key;
                return true;
            }
        }

        return false;
    }
}
