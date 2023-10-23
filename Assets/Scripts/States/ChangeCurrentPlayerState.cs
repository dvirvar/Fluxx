using System.Collections;
using UnityEngine;

public class ChangeCurrentPlayerState : State
{
    bool comingFromHandLimitRule = false;
    bool comingFromKeeperLimitRule = false;
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        if (gameStateMachine.CurrentHandLimitRule.HasValue)
        {
            comingFromHandLimitRule = true;
            gameStateMachine.PushState(new HandLimitState(gameStateMachine.CurrentPlayer));
            yield break;
        }
        if (gameStateMachine.CurrentKeeperLimitRule.HasValue)
        {
            comingFromKeeperLimitRule = true;
            gameStateMachine.PushState(new KeeperLimitState(gameStateMachine.CurrentPlayer));
            yield break;
        }
        yield return StartOfTurn(gameStateMachine);
    }

    IEnumerator StartOfTurn(GameStateMachine gameStateMachine)
    {
        gameStateMachine.AdvanceTurn();
        foreach (var rule in gameStateMachine.Board.GetNewRuleCards())
        {
            rule.SetCanBeSelected(rule.NewRuleCardInfo.NewRuleType.Actionable());
        }
        if (gameStateMachine.NoHandBonus && gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer).Count == 0)
        {
            var draws = gameStateMachine.Inflation ? 4 : 3;
            for (int i = 0; i < draws; ++i)
            {
                var card = gameStateMachine.Board.DrawCard();
                if (card != null)
                {
                    gameStateMachine.Board.AddHandCardTo(gameStateMachine.CurrentPlayer, card);
                }
            }
        }
        gameStateMachine.DrawToMatchDraws();
        var state = gameStateMachine.CheckHasPlayerWon();
        if (state != null)
        {
            gameStateMachine.ResetAndSetState(state);
        } else if (gameStateMachine.IsFirstPlayRandom && gameStateMachine.CurrentPlays > 1 && gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer).Count > 1)
        {
            gameStateMachine.PushState(new FirstPlayRandomState());
        }
        else
        {
            gameStateMachine.SetState(new StartOfPlayState());
        }
        yield break;
    }

    public override IEnumerator OnResume(GameStateMachine gameStateMachine)
    {
        if (comingFromHandLimitRule && gameStateMachine.CurrentKeeperLimitRule.HasValue)
        {
            comingFromHandLimitRule = false;
            comingFromKeeperLimitRule = true;
            gameStateMachine.PushState(new KeeperLimitState(gameStateMachine.CurrentPlayer));
            yield break;
        }
        if (comingFromHandLimitRule || comingFromKeeperLimitRule)
        {
            comingFromHandLimitRule = false;
            comingFromKeeperLimitRule = false;
            yield return StartOfTurn(gameStateMachine);
        } else
        {
            gameStateMachine.SetState(new StartOfPlayState());
        }
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        yield break;
    }
}
