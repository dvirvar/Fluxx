using System.Collections;

public class StartOfTurnState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        foreach (var player in EnumUtil.GetArrayOf<GameStateMachine.Player>())
        {
            gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(player, player == gameStateMachine.CurrentPlayer, player == gameStateMachine.CurrentPlayer);
        }
        var getOnWithIt = gameStateMachine.Board.GetNewRuleCards().Find(r=>r.NewRuleCardInfo.NewRuleType == NewRuleCardType.GetOnWithIt);
        if (getOnWithIt != null)
        {
            var isFinalPlay = gameStateMachine.CurrentPlays - gameStateMachine.Played == 1;
            getOnWithIt.SetCanBeSelected(isFinalPlay && gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer).Count > 0);
        }
        gameStateMachine.SetState(new IdleState());
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
