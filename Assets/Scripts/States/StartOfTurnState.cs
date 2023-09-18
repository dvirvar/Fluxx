using System.Collections;

public class StartOfTurnState : State
{

    public StartOfTurnState(GameStateMachine gameStateMachine, GameStateMachine.Player currentPlayer) {
        gameStateMachine.CurrentPlayer = currentPlayer;
    }

    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        gameStateMachine.ResetDrawedAndPlayed();
        foreach (var player in EnumUtil.GetArrayOf<GameStateMachine.Player>())
        {
            gameStateMachine.Board.ShowPlayerHand(player, player == gameStateMachine.CurrentPlayer);
        }
        gameStateMachine.DrawToMatchDraws();
        if (gameStateMachine.IsFirstPlayRandom)
        {
            gameStateMachine.SetState(new FirstPlayRandomState());
        } else
        {
            gameStateMachine.SetState(new IdleState());
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
