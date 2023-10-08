using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveDoubleAgendaState : State
{
    GameStateMachine gameStateMachine;
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        this.gameStateMachine = gameStateMachine;
        var currentGoalCard = gameStateMachine.Board.GetCurrentGoalCard();
        var secondCurrentGoalCard = gameStateMachine.Board.GetSecondCurrentGoalCard();
        if (currentGoalCard == null)
        {
            if (secondCurrentGoalCard == null)
            {
                gameStateMachine.PopState();
                yield break;
            }
            gameStateMachine.Board.SetCurrentGoal(gameStateMachine.Board.RemoveSecondCurrentGoal());
            gameStateMachine.PopState();
            yield break;
        }
        else if (secondCurrentGoalCard == null)
        {
            gameStateMachine.PopState();
            yield break;
        }
        gameStateMachine.DoubleAgendaManager.RemoveDoubleAgenda(currentGoalCard.GoalCardInfo.Name, secondCurrentGoalCard.GoalCardInfo.Name);
        gameStateMachine.DoubleAgendaManager.ButtonPressed += DoubleAgendaManager_ButtonPressed;
        yield break;
    }

    private void DoubleAgendaManager_ButtonPressed(bool secondPressed)
    {
        var secondGoalcard = gameStateMachine.Board.GetSecondCurrentGoalCard();
        if (secondPressed)
        {
            gameStateMachine.Board.SetCurrentGoal(secondGoalcard);
        } else
        {
            gameStateMachine.Board.AddToDiscardPile(secondGoalcard);
        }
        gameStateMachine.PopState();
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        gameStateMachine.DoubleAgendaManager.ButtonPressed -= DoubleAgendaManager_ButtonPressed;
        this.gameStateMachine = null;
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        yield break;
    }
}
