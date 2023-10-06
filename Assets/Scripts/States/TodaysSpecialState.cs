using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TodaysSpecialState : State
{
    readonly List<Card> cardsToShow = new();
    readonly List<NewRuleCard> rulesThatCouldBeSelected = new();
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        int toDraw = 3;
        if (gameStateMachine.Inflation)
        {
            ++toDraw;
        }
        for (int i = 0; i < toDraw; ++i)
        {
            var card = gameStateMachine.Board.DrawCard();
            if (card != null)
            {
                card.SetCanBeSelected(true);
                cardsToShow.Add(card);
            }
        }
        if (cardsToShow.Count == 0)
        {
            gameStateMachine.PopState();
            yield break;
        }
        rulesThatCouldBeSelected.AddRange(gameStateMachine.Board.GetNewRuleCards().FindAll(r => r.CanBeSelected));
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(false);
        }
        gameStateMachine.SetCardsInfrontOfCamera(cardsToShow);
        gameStateMachine.ShowCardsInfrontOfCamera(true);
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(gameStateMachine.CurrentPlayer, false, false);
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(true);
        }
        cardsToShow.Clear();
        rulesThatCouldBeSelected.Clear();
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(gameStateMachine.CurrentPlayer, true, true);
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        if (cardsToShow.Remove(card))
        {
            gameStateMachine.PushState(new HandCardActionState(card, true));
        }
        yield break;
    }

    public override IEnumerator OnResume(GameStateMachine gameStateMachine)
    {
        if (cardsToShow.Count <= 2)
        {
            foreach (var card in cardsToShow)
            {
                card.SetCanBeSelected(false);
                card.gameObject.SetActive(true);
                gameStateMachine.Board.AddToDiscardPile(card);
            }
            gameStateMachine.PopState();
        }
        else
        {
            gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(gameStateMachine.CurrentPlayer, false, false);
            gameStateMachine.SetCardsInfrontOfCamera(cardsToShow);
            gameStateMachine.ShowCardsInfrontOfCamera(true);
            foreach (var card in cardsToShow)
            {
                card.gameObject.SetActive(true);
            }
        }
        yield break;
    }

    public override IEnumerator OnPause(GameStateMachine gameStateMachine)
    {
        foreach (var card in cardsToShow)
        {
            card.gameObject.SetActive(false);
        }
        gameStateMachine.ShowCardsInfrontOfCamera(false);
        yield break;
    }
}
