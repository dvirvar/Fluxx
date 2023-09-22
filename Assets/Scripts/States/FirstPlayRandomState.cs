using System.Collections;

public class FirstPlayRandomState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        if (gameStateMachine.CurrentPlays <= 1)
        {
            gameStateMachine.SetState(new StartOfTurnState());
            yield break;
        }
        foreach (var player in EnumUtil.GetArrayOf<GameStateMachine.Player>())
        {
            gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(player, false, player == gameStateMachine.CurrentPlayer);
        }
        foreach (var rule in gameStateMachine.Board.GetNewRuleCards())
        {
            rule.SetCanBeSelected(false);
        }
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(gameStateMachine.CurrentPlayer, true, true);
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
            gameStateMachine.SetState(new PlayHandCardState(card));
        }
        yield break;
    }
}
