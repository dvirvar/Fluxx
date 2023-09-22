using System.Collections;

public class EndOfTurnState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        if (gameStateMachine.Played >= gameStateMachine.CurrentPlays || gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer).Count == 0)
        {
            gameStateMachine.SetState(new ChangeCurrentPlayerState(gameStateMachine));
        } else
        {
            gameStateMachine.SetState(new StartOfTurnState());
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