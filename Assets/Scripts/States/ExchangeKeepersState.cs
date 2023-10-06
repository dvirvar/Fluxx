using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ExchangeKeepersState : State
{
    readonly List<NewRuleCard> rulesThatCouldBeSelected = new();
    GameStateMachine.Player otherPlayer;
    KeeperCard myKeeperToExchange;
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        var keepers = gameStateMachine.Board.GetPlayerKeeperCards(gameStateMachine.CurrentPlayer);
        otherPlayer = gameStateMachine.CurrentPlayer == GameStateMachine.Player.Player1 ? GameStateMachine.Player.Player2 : GameStateMachine.Player.Player1;
        var otherPlayerKeepers = gameStateMachine.Board.GetPlayerKeeperCards(otherPlayer);
        if (keepers.Count == 0 || otherPlayerKeepers.Count == 0)
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
        foreach (var card in keepers)
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
        rulesThatCouldBeSelected.Clear();
        myKeeperToExchange = null;
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        if (myKeeperToExchange == null)
        {
            var keepers = gameStateMachine.Board.GetPlayerKeeperCards(gameStateMachine.CurrentPlayer);
            if (card is KeeperCard keeperCard && keepers.Remove(keeperCard))
            {
                keeperCard.SetCanBeSelected(false);
                foreach(var keeper in keepers)
                {
                    keeper.SetCanBeSelected(false);
                }
                foreach(var keeper in gameStateMachine.Board.GetPlayerKeeperCards(otherPlayer))
                {
                    keeper.SetCanBeSelected(true);
                }
                myKeeperToExchange = keeperCard;
            }
        } else
        {
            var otherPlayerKeepers = gameStateMachine.Board.GetPlayerKeeperCards(otherPlayer);
            if (card is KeeperCard keeperCard && otherPlayerKeepers.Remove(keeperCard))
            {
                keeperCard.SetCanBeSelected(false);
                foreach(var keeper in otherPlayerKeepers)
                {
                    keeper.SetCanBeSelected(false);
                }
                gameStateMachine.Board.AddKeeperTo(gameStateMachine.CurrentPlayer, keeperCard);
                gameStateMachine.Board.AddKeeperTo(otherPlayer, myKeeperToExchange);
                gameStateMachine.Board.RearrangePlayerKeepers(gameStateMachine.CurrentPlayer);
                gameStateMachine.Board.RearrangePlayerKeepers(otherPlayer);
                gameStateMachine.PopState();
            }
        }
        yield break;
    }
}
