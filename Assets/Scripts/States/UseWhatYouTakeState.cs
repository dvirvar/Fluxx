using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseWhatYouTakeState : State
{
    readonly List<NewRuleCard> rulesThatCouldBeSelected = new();
    GameStateMachine.Player otherPlayer;
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
        foreach (var card in gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer))
        {
            card.SetCanBeSelected(false);
        }
        foreach (var card in otherPlayerHandCards)
        {
            card.SetCanBeSelected(true);
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
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        if (gameStateMachine.Board.GetPlayerHandCards(otherPlayer).Remove(card))
        {
            gameStateMachine.Board.RearrangePlayerHand(otherPlayer);
            gameStateMachine.SetState(new HandCardActionState(card, true));
        }
        yield break;
    }
}
