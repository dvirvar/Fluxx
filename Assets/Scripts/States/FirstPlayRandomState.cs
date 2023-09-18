using System.Collections;

public class FirstPlayRandomState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        if (gameStateMachine.CurrentPlays <= 1)
        {
            gameStateMachine.SetState(new IdleState());
            yield break;
        }
        gameStateMachine.Board.ShowPlayerHand(gameStateMachine.CurrentPlayer, false);
        foreach (var rule in gameStateMachine.Board.GetNewRuleCards())
        {
            rule.SetCanBeSelected(false);
        }
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        gameStateMachine.Board.ShowPlayerHand(gameStateMachine.CurrentPlayer, true);
        foreach (var rule in gameStateMachine.Board.GetNewRuleCards())
        {
            rule.SetCanBeSelected(rule.NewRuleCardInfo.NewRuleType.Actionable());
        }
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        if (gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer).Remove(card))
        {

        }
        yield break;
    }
}
