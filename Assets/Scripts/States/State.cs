using System.Collections;

public abstract class State
{
    public static readonly State NULL = new NullState();
    public abstract IEnumerator OnEnter(GameStateMachine gameStateMachine);
    public virtual IEnumerator OnResume(GameStateMachine gameStateMachine) { yield break; }
    public virtual IEnumerator OnPause(GameStateMachine gameStateMachine) { yield break; }
    public abstract IEnumerator OnExit(GameStateMachine gameStateMachine);
    public abstract IEnumerator Play(GameStateMachine gameStateMachine, Card card);
}
