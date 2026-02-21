using UnityEngine;

public sealed class Bell : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    // XR 터치/포크/그랩 등 어떤 방식이든
    // 최종적으로 "누가 눌렀는지"를 전달해야 함
    public void NotifyPressedBy(int playerIndex)
    {
        // playerIndex: 0 = 유저, 1~ = 적
        gameManager.OnBellPressed(playerIndex);
    }
}