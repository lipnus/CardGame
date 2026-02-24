using UnityEngine;

public sealed class UserDeck : DeckBase
{
    private int USER_INDEX = 0;
    
    protected override void HandlePlayDropped()
    {
        Debug.Log("### 주인공 카드 냈다");
        PopTop();
        
        var gameManager = FindObjectOfType<GameManager>();
        gameManager.OnCardPlayed(USER_INDEX);
    }
    
    protected override void OnTopCardChanged(Card newTop)
    {
        if (newTop == null) return;
        Debug.Log("OnTopCardChanged: " + newTop.Data.Symbol + "");
        newTop.SetGrabbable(true);
    }
}