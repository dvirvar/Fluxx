using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class TrashANewRuleState : State
{
    readonly List<NewRuleCard> rulesThatCouldNotBeSelected = new();
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        var newRuleCards = gameStateMachine.Board.GetNewRuleCards();
        if (newRuleCards.Count == 0)
        {
            gameStateMachine.PopState();
            yield break;
        }
        rulesThatCouldNotBeSelected.AddRange(newRuleCards.FindAll(r => !r.CanBeSelected));
        foreach (var rule in newRuleCards)
        {
            rule.SetCanBeSelected(true);
        }
        foreach (var card in gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer))
        {
            card.SetCanBeSelected(false);
        }
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        foreach (var rule in rulesThatCouldNotBeSelected)
        {
            rule.SetCanBeSelected(false);
        }
        foreach (var card in gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer))
        {
            card.SetCanBeSelected(true);
        }
        rulesThatCouldNotBeSelected.Clear();
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        if (card is NewRuleCard newRuleCard)
        {
            rulesThatCouldNotBeSelected.Remove(newRuleCard);
            gameStateMachine.DiscardNewRules(new() { newRuleCard });
            gameStateMachine.PopState();
        }
        yield break;
    }
}
