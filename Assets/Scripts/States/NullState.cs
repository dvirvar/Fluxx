using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override IEnumerator Play(Card card)
    {
        yield break;
    }
}
