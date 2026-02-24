using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class CardView : MonoBehaviour
{
    [Header("현재는 텍스트로 표시")]
    [SerializeField] private TMP_Text text;

    [Header("향후 이미지 표시용(지금은 비워둬도 됨)")]
    [SerializeField] private Image image;

    // 카드 데이터에 맞게 시각표현 갱신
    public void Render(CardData data)
    {
        // 지금은 텍스트: A A A 형태로 표시
        if (text != null)
        {
            text.text = BuildSymbolString(data.Symbol, data.Count);
        }

        // 나중에 이미지로 갈 때:
        // if (image != null) image.sprite = CardArtDB.GetSprite(data.ArtKey) 같은 식으로 교체
    }

    private string BuildSymbolString(CardSymbol symbol, int count)
    {
        // 예: A 3 -> "A A A"
        // 직관 우선(효율 무시)
        string s = "";
        for (int i = 0; i < count; i++)
        {
            s += symbol.ToString();
            if (i != count - 1) s += " ";
        }
        return s;
    }
}