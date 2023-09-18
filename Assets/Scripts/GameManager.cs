using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameStateMachine))]
public class GameManager : MonoBehaviour
{
    GameStateMachine gameStateMachine;
    [SerializeField] InputManager inputManager;
    [SerializeField] Board board;
    [SerializeField] CardPrefabsSO cardPrefabInfoSO;

    void Awake()
    {
        gameStateMachine = GetComponent<GameStateMachine>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        inputManager.OnCardClicked += PlayCard;
        var deck = new List<Card>();
        var actionCardPrefab = cardPrefabInfoSO.ActionCardPrefab;
        foreach (var item in DeckData.ActionCards)
        {
            var card = Instantiate(actionCardPrefab);
            card.Init(item);
            deck.Add(card);
        }
        var goalCardPrefab = cardPrefabInfoSO.GoalCardPrefab;
        foreach (var item in DeckData.GoalCards)
        {
            var card = Instantiate(goalCardPrefab);
            card.Init(item);
            deck.Add(card);
        }
        var keeperCardPrefab = cardPrefabInfoSO.KeeperCardPrefab;
        foreach (var item in DeckData.KeeperCards)
        {
            var card = Instantiate(keeperCardPrefab);
            card.Init(item);
            deck.Add(card);
        }
        var newRuleCardPrefab = cardPrefabInfoSO.NewRuleCardPrefab;
        foreach (var item in DeckData.NewRuleCards)
        {
            var card = Instantiate(newRuleCardPrefab);
            card.Init(item);
            deck.Add(card);
        }
        deck.Shuffle();
        board.SetDeck(deck);
        for (var i = 0; i < 6; ++i)
        {
            var card = board.DrawCard();
            if (i < 3)
            {
                board.AddCardToPlayer1Hand(card);
            } else
            {
                board.AddCardToPlayer2Hand(card);
            }
        }
        gameStateMachine.Init(new StartOfTurnState(gameStateMachine, GameStateMachine.Player.Player1), board);
    }

    void OnDestroy()
    {
        inputManager.OnCardClicked -= PlayCard;
    }

    void PlayCard(Card card)
    {
        gameStateMachine.PlayCard(card);
    }
}
