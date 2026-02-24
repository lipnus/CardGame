using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class CardView : MonoBehaviour
{
    [Header("현재는 텍스트로 표시")]
    [SerializeField] private TMP_Text text;

    [Header("향후 이미지 표시용(지금은 비워둬도 됨)")]
    [SerializeField] private Image image;

    public void Render(CardData data)
    {
        if (text == null) return;

        string symbol = "";
        Color color = Color.white;

        switch (data.Symbol)
        {
            case CardSymbol.A:
                symbol = "♥";
                color = new Color32(220, 53, 69, 255);   // 빨
                break;

            case CardSymbol.B:
                symbol = "■";
                color = new Color32(1, 1, 1, 255);  
                break;

            case CardSymbol.C:
                symbol = "●";
                color = new Color32(0, 123, 255, 255);   // 파
                break;

            case CardSymbol.D:
                symbol = "♠";
                color = new Color32(255, ㅋ, 7, 255);   // 노
                break;
        }

        string s = "";
        for (int i = 0; i < data.Count; i++)
        {
            s += symbol;
            if (i != data.Count - 1) s += " ";
        }

        text.text = s;
        text.color = color;
        
        // 나중에 이미지로 갈 때:
        // if (image != null) image.sprite = CardArtDB.GetSprite(data.ArtKey) 같은 식으로 교체
    } 

}