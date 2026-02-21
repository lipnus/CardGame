using System.Collections.Generic;

public sealed class TableRuleEvaluator
{
    // 테이블(각 플레이어 소켓)에 올라온 카드들을 받아서
    // "같은 문양 합이 5"가 있는지 반환
    public bool HasFiveCombo(List<PlaySocketBase> sockets, out FruitSymbol symbolHit)
    {
        symbolHit = FruitSymbol.A;

        var sum = new Dictionary<FruitSymbol, int>()
        {
            {FruitSymbol.A, 0},
            {FruitSymbol.B, 0},
            {FruitSymbol.C, 0},
            {FruitSymbol.D, 0},
        };

        for (int i = 0; i < sockets.Count; i++)
        {
            var c = sockets[i].CurrentCard;
            if (c == null) continue;

            sum[c.Data.Symbol] += c.Data.Count;
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
