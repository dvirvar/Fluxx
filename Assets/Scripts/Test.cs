using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] CardPrefabsSO cardPrefabInfoSO;
    private void Start()
    {
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
        StartCoroutine(x());
    }

    IEnumerator x()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            var card = board.DrawCard();
            if (card is NewRuleCard newRuleCard)
            {
                board.AddNewRule(newRuleCard);
            }
            else if (card is KeeperCard keeperCard)
            {
                board.AddKeeperTo(EnumUtil.GetRandomOf<GameStateMachine.Player>(), keeperCard);
            } 
            else if (card is GoalCard goalCard)
            {
                if (Random.Range(0, 2) == 1)
                {
                    board.SetCurrentGoal(goalCard);
                }
                else
                {
                    board.SetSecondCurrentGoal(goalCard);
                }
            }
            else
            {
                board.AddToDiscardPile(card);
            }
        }
        
    }
}
