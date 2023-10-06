using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealAKeeperState : State
{
    readonly List<NewRuleCard> rulesThatCouldBeSelected = new();
    GameStateMachine.Player otherPlayer;
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        otherPlayer = gameStateMachine.CurrentPlayer == GameStateMachine.Player.Player1 ? GameStateMachine.Player.Player2 : GameStateMachine.Player.Player1;
        var keeperCards = gameStateMachine.Board.GetPlayerKeeperCards(otherPlayer);
        if (keeperCards.Count == 0)
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
        foreach (var keeper in keeperCards)
        {
            keeper.SetCanBeSelected(true);
        }
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(true);
        }
        foreach (var keeper in gameStateMachine.Board.GetPlayerKeeperCards(otherPlayer))
        {
            keeper.SetCanBeSelected(false);
        }
        rulesThatCouldBeSelected.Clear();
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        if (card is KeeperCard keeperCard && gameStateMachine.Board.GetPlayerKeeperCards(otherPlayer).Remove(keeperCard))
        {
            card.SetCanBeSelected(false);
            gameStateMachine.Board.RearrangePlayerKeepers(otherPlayer);
            gameStateMachine.Board.AddKeeperTo(gameStateMachine.CurrentPlayer, keeperCard);
            gameStateMachine.PopState();
        }
        yield break;
    }
}
