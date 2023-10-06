using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTaxState : State
{
    readonly List<NewRuleCard> rulesThatCouldBeSelected = new();
    GameStateMachine.Player otherPlayer;
    int maxCardsToTax = 0;
    List<Card> cardsToTax = new();
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        otherPlayer = gameStateMachine.CurrentPlayer == GameStateMachine.Player.Player1 ? GameStateMachine.Player.Player2 : GameStateMachine.Player.Player1;
        var otherPlayerHandCards = gameStateMachine.Board.GetPlayerHandCards(otherPlayer);
        if (otherPlayerHandCards.Count == 0)
        {
            gameStateMachine.PopState();
            yield break;
        }
        rulesThatCouldBeSelected.AddRange(gameStateMachine.Board.GetNewRuleCards().FindAll(r => r.CanBeSelected));
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(false);
        }
        maxCardsToTax = gameStateMachine.Inflation ? 2 : 1;
        foreach (var card in otherPlayerHandCards)
        {
            card.SetCanBeSelected(true);
        }
        foreach (var card in gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer))
        {
            card.SetCanBeSelected(false);
        }
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(true);
        }
        foreach (var card in gameStateMachine.Board.GetPlayerHandCards(otherPlayer))
        {
            card.SetCanBeSelected(false);
        }
        rulesThatCouldBeSelected.Clear();
        cardsToTax.Clear();
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        var otherPlayerHandCards = gameStateMachine.Board.GetPlayerHandCards(otherPlayer);
        if (otherPlayerHandCards.Remove(card))
        {
            card.SetCanBeSelected(false);
            cardsToTax.Add(card);
            if (cardsToTax.Count >= maxCardsToTax || otherPlayerHandCards.Count == 0)
            {
                foreach(var tax in cardsToTax)
                {
                    gameStateMachine.Board.AddHandCardTo(gameStateMachine.CurrentPlayer, tax);
                }
                gameStateMachine.Board.RearrangePlayerHand(otherPlayer);
                gameStateMachine.PopState();
                yield break;
            }
        }
        yield break;
    }
}
