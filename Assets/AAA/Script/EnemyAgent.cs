using System.Collections;
using UnityEngine;

public sealed class EnemyAgent : MonoBehaviour
{
    [SerializeField] private int playerIndex; // 1~3
    [SerializeField] private EnemyDeck deck;
    [SerializeField] private EnemyPlaySocket socket;
    [SerializeField] private GameManager gameManager;

    [Header("카드 내는 딜레이(랜덤)")]
    [SerializeField] private Vector2 playDelayRange = new Vector2(0.3f, 1.2f);

    [Header("벨 치는 딜레이(랜덤)")]
    [SerializeField] private Vector2 bellDelayRange = new Vector2(0.1f, 0.8f);

    [Header("패턴(난이도/성향)")]
    [Range(0f, 1f)]
    [SerializeField] private float missChance = 0.05f; // 일부러 놓칠 확률

    public void TakeTurn()
    {
        StartCoroutine(TakeTurnRoutine());
    }

    private IEnumerator TakeTurnRoutine()
    {
        float d = Random.Range(playDelayRange.x, playDelayRange.y);
        yield return new WaitForSeconds(d);

        if (deck.TryPlayTopToSocket(socket, out var played))
        {
            gameManager.OnCardPlayed(playerIndex);
        }
    }

    // GameManager가 "지금 벨 찬스"라고 알려주면 호출
    public void TryRingBellIfCombo()
    {
        StartCoroutine(BellRoutine());
    }

    private IEnumerator BellRoutine()
    {
        // 일부러 놓치는 패턴
        if (Random.value < missChance)
            yield break;

        float d = Random.Range(bellDelayRange.x, bellDelayRange.y);
        yield return new WaitForSeconds(d);

        gameManager.OnBellPressed(playerIndex);
    }
}