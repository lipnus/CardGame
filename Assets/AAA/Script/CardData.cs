using System;
using System.Collections.Generic;
using UnityEngine;

public enum CardSymbol // 과일 대신 A~D
{
    A, B, C, D
}

[Serializable]
public struct CardData
{
    public CardSymbol Symbol;  // A~D
    public int Count;           // 1~5

    // 향후 이미지 표현을 위한 키(주소/리소스 키 등) 확장 포인트
    public string ArtKey;

    public CardData(CardSymbol symbol, int count, string artKey = "")
    {
        Symbol = symbol;
        Count = count;
        ArtKey = artKey;
    }
}