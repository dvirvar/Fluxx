using System.Collections;

public class EndOfPlayState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        gameStateMachine.DrawToMatchDraws();
        if (gameStateMachine.Played >= gameStateMachine.CurrentPlays || gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer).Count == 0)
        {
            gameStateMachine.SetState(new ChangeCurrentPlayerState());
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
