using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashAKeeperState : State
{
    readonly List<NewRuleCard> rulesThatCouldBeSelected = new();
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        var keepersCount = 0;
        var allPlayers = EnumUtil.GetArrayOf<GameStateMachine.Player>();
        foreach (var player in allPlayers)
        {
            keepersCount += gameStateMachine.Board.GetPlayerKeeperCards(player).Count;
        }
        if (keepersCount == 0) { 

            gameStateMachine.PopState();
            yield break;
        }
        rulesThatCouldBeSelected.AddRange(gameStateMachine.Board.GetNewRuleCards().FindAll(r => r.CanBeSelected));
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(false);
        }
        foreach (var card in  gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer))
        {
            card.SetCanBeSelected(false);
        }
        foreach (var player in allPlayers)
        {
            foreach (var keeper in gameStateMachine.Board.GetPlayerKeeperCards(player))
            {
                keeper.SetCanBeSelected(true);
            }
        }
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(true);
        }
        foreach (var card in gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer))
        {
            card.SetCanBeSelected(true);
        }
        foreach (var player in EnumUtil.GetArrayOf<GameStateMachine.Player>())
        {
            foreach (var keeper in gameStateMachine.Board.GetPlayerKeeperCards(player))
            {
                keeper.SetCanBeSelected(false);
            }
        }
        rulesThatCouldBeSelected.Clear();
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        if (card is KeeperCard keeperCard)
        {
            foreach (var player in EnumUtil.GetArrayOf<GameStateMachine.Player>())
            {
                if (gameStateMachine.Board.GetPlayerKeeperCards(player).Remove(keeperCard))
                {
                    keeperCard.SetCanBeSelected(false);
                    gameStateMachine.Board.AddToDiscardPile(keeperCard);
                    gameStateMachine.Board.RearrangePlayerKeepers(player);
                    break;
                }
            }
            var state = gameStateMachine.CheckHasPlayerWon();
            if (state != null)
            {
                gameStateMachine.ResetAndSetState(state);
            } else
            {
                gameStateMachine.PopState();
            }
        }
        yield break;
    }
}
