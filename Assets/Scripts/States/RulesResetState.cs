using System.Collections;

public class RulesResetState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        var state = gameStateMachine.DiscardAllNewRules();
        if (state != null)
        {
            gameStateMachine.SetState(state);
        } else
        {
            gameStateMachine.PopState();
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
