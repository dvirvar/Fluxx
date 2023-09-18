using System.Collections;

public class NullState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
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
