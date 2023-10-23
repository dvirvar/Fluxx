using System.Collections;

public class FirstPlayRandomState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        foreach (var player in EnumUtil.GetArrayOf<GameStateMachine.Player>())
        {
            gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(player, false, player == gameStateMachine.CurrentPlayer);
        }
        foreach (var rule in gameStateMachine.Board.GetNewRuleCards())
        {
            rule.SetCanBeSelected(false);
        }
        yield break;
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
            gameStateMachine.SetState(new HandCardActionState(card));
        }
        yield break;
    }
}
