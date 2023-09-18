using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class State
{
    public static readonly State NULL = new NullState();
    public abstract IEnumerator OnEnter(GameStateMachine gameStateMachine);
    public abstract IEnumerator OnExit(GameStateMachine gameStateMachine);
    public abstract IEnumerator Play(GameStateMachine gameStateMachine, Card card);
}
