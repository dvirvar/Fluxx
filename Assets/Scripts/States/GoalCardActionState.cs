using System.Collections;
using UnityEngine;

public class GoalCardActionState : State
{
    GoalCard card;

    public GoalCardActionState(GoalCard card)
    {
        this.card = card;
    }

    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        if (!gameStateMachine.HasDoubleAgenda)
        {
            gameStateMachine.Board.SetCurrentGoal(card);
            if (gameStateMachine.CheckHasPlayerWon() != null)
            {
                Debug.Log($"{gameStateMachine.CheckHasPlayerWon()} has won");
            }
        }
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        card = null;
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        yield break;
    }
}
