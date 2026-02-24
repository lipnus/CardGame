using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Header("연결")]
    [SerializeField] private CardDeckFactory factory;
    [SerializeField] private Transform cardPoolParent;   // 카드 생성 부모 (정리용)
    [SerializeField] private UserDeck userDeck;
    
    [SerializeField] private GameManager gameManager;
    
    public void ButtonAction1()
    {
        Debug.Log("Button 1 실행됨");
        CreateAndStackAllCards();
    }

    public void ButtonAction2()
    {

    }

    private void Start()
    {
        gameManager.StartGame(1);
    }

    private void CreateAndStackAllCards()
    {
        if (!factory || !userDeck)
        {
            Debug.LogError("Factory 또는 UserDeck 연결 안됨");
            return;
        }

        // 1. 56장 생성 (셔플 포함)
        var cards = factory.CreateFullDeck(cardPoolParent);

        // 2. 전부 유저 덱에 넣기
        foreach (var card in cards)
        {
            userDeck.AddCardToBottom(card);
        }
        
        // 3. 덱 시각적으로 쌓기
        userDeck.BuildStackVisual();
        Debug.Log($"총 {cards.Count}장 유저 덱에 추가 완료");
    }
}