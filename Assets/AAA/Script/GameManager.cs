using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    private enum GameState
    {
        NotStarted,
        WaitingTurnAction,   // 현재 플레이어가 카드 내기를 기다림
        EvaluatingCombo,     // 카드 낸 직후 콤보 판정
        BellWindow,          // 벨을 칠 수 있는 구간
        Resolving            // 승패/카드 회수 등 처리(추후 확장)
    }

    [Header("플레이어 구성")]
    [SerializeField] private UserDeck userDeck;
    [SerializeField] private UserPlaySocket userSocket;

    [SerializeField] private List<EnemyAgent> enemies = new List<EnemyAgent>();
    [SerializeField] private List<EnemyPlaySocket> enemySockets = new List<EnemyPlaySocket>();

    [Header("카드 생성/분배")]
    [SerializeField] private CardDeckFactory factory;
    [SerializeField] private Transform cardPoolParent;

    private readonly List<PlaySocketBase> allSockets = new List<PlaySocketBase>();
    private readonly TableRuleEvaluator evaluator = new TableRuleEvaluator();

    private GameState state = GameState.NotStarted;
    private int currentTurnIndex = 0; // 0=유저, 1~ = 적

    private bool comboAvailable = false;
    private CardSymbol comboSymbol;
    
    public void StartGame(int enemyCount)
    {
        Debug.Log($"### StartGame({enemyCount})");
        
        // 소켓 리스트 구성
        allSockets.Clear();
        allSockets.Add(userSocket);
        for (int i = 0; i < enemyCount; i++)
            allSockets.Add(enemySockets[i]);

        // 덱 리스트 구성(유저 + 적)
        var decks = new List<DeckBase>();
        decks.Add(userDeck);
        for (int i = 0; i < enemyCount; i++)
            decks.Add(enemies[i].GetComponentInChildren<EnemyDeck>());

        // 56장 생성/셔플/분배
        var full = factory.CreateFullDeck(cardPoolParent);
        factory.Deal(full, decks);

        // 시작 턴
        currentTurnIndex = 0;
        state = GameState.WaitingTurnAction;

        // 적 수에 맞게 EnemyAgent 활성/연결 상태는 씬에서 이미 맞춰둔다고 가정
    }

    // 유저가 카드를 소켓에 냈을 때, 또는 적이 냈을 때 호출
    public void OnCardPlayed(int playerIndex)
    {
        
        Debug.Log($"#### playerIndex:{playerIndex}, crrentTurnIndex:{currentTurnIndex}, state:{state}");
        
        // 현재 턴 플레이어가 아닌데 들어오면 무시(안전장치)
        if (playerIndex != currentTurnIndex) return;
        if (state != GameState.WaitingTurnAction) return;
        
        state = GameState.EvaluatingCombo;

        // 콤보 판정
        comboAvailable = evaluator.HasFiveCombo(allSockets, out comboSymbol);

        if (comboAvailable)
        {
            Debug.Log("### [판정] 누를 수 있는 타이밍");
            // 벨 윈도우 오픈
            state = GameState.BellWindow;

            // 적들에게도 "벨 칠지 말지" 기회 부여
            for (int i = 0; i < enemies.Count; i++)
                enemies[i].TryRingBellIfCombo();
        }
        else
        {
            Debug.Log("### [판정] 불가");
            // 콤보 없으면 다음 턴
            AdvanceTurn();
        }
    }

    // 벨이 눌렸을 때 (유저든 적이든)
    public void OnBellPressed(int playerIndex)
    {
        if (state != GameState.BellWindow)
            return;

        // 지금 콤보가 없는데 벨을 치면 페널티 등(추후)
        // 여기서는 콤보가 있을 때만 처리한다고 가정
        if (!comboAvailable)
            return;

        state = GameState.Resolving;

        // TODO: 승자 처리(카드 회수/점수/덱에 넣기 등)
        // 지금은 “누가 먼저 눌렀는지”만 확정
        Debug.Log($"벨 승자: playerIndex={playerIndex}, comboSymbol={comboSymbol}");

        // 라운드 정리 후 다음 턴으로
        comboAvailable = false;
        state = GameState.WaitingTurnAction;
        // 보통은 벨 승자가 다음 턴 시작(혹은 카드 회수 후 승자 턴) 정책 결정 필요
        currentTurnIndex = playerIndex;
        
        // 턴 시작!
        if (currentTurnIndex != 0)
        {
            var enemyIdx = currentTurnIndex - 1;
            enemies[enemyIdx].TakeTurn();
        }
    }

    // 다음 턴으로 이동
    private void AdvanceTurn()
    {
        int totalPlayers = 1 + enemies.Count;
        currentTurnIndex = (currentTurnIndex + 1) % totalPlayers;

        state = GameState.WaitingTurnAction;

        // 적 턴이면 적이 스스로 딜레이 후 카드 냄
        if (currentTurnIndex != 0)
        {
            int enemyIdx = currentTurnIndex - 1;
            enemies[enemyIdx].TakeTurn();
        }
        else
        {
            // 유저 턴: 유저가 직접 top card grab 해서 소켓에 놓도록 대기
        }
    }
}