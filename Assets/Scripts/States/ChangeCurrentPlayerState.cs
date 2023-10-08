using System.Collections;
using UnityEngine;

public class ChangeCurrentPlayerState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        gameStateMachine.AdvanceTurn();
        foreach(var rule in gameStateMachine.Board.GetNewRuleCards())
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
        var won = gameStateMachine.CheckHasPlayerWon();
        if (won != null)
        {
            Debug.Log($"{won.Value} has won");
        }
        if (gameStateMachine.CurrentPlays == 0)
        {
            Debug.Log("0 current plays");
        }
        if (gameStateMachine.IsFirstPlayRandom)
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
        gameStateMachine.SetState(new StartOfPlayState());
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
