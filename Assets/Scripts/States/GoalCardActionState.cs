using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCardActionState : State
{
    GoalCard card;
    GameStateMachine gameStateMachine;

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
            gameStateMachine.PopState();
            yield break;
        }
        var currentGoalCard = gameStateMachine.Board.GetCurrentGoalCard();
        var secondCurrentGoalCard = gameStateMachine.Board.GetSecondCurrentGoalCard();
        string firstName, secondName;
        if (currentGoalCard == null)
        {
            firstName = "1";
        } else
        {
            firstName = currentGoalCard.GoalCardInfo.Name;
        }
        if (secondCurrentGoalCard == null)
        {
            secondName = "2";
        } else
        {
            secondName = secondCurrentGoalCard.GoalCardInfo.Name;
        }
        this.gameStateMachine = gameStateMachine;
        gameStateMachine.SetCardsInfrontOfCamera(new List<Card> { card });
        gameStateMachine.ShowCardsInfrontOfCamera(true);
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(gameStateMachine.CurrentPlayer, true, false);
        gameStateMachine.DoubleAgendaManager.ChooseDoubleAgenda(firstName, secondName);
        gameStateMachine.DoubleAgendaManager.ButtonPressed += DoubleAgendaManager_ButtonPressed;
        yield break;
    }

    private void DoubleAgendaManager_ButtonPressed(bool secondPressed)
    {
        if (secondPressed)
        {
            gameStateMachine.Board.SetSecondCurrentGoal(card);
        } else
        {
            gameStateMachine.Board.SetCurrentGoal(card);
        }
        if (gameStateMachine.CheckHasPlayerWon() != null)
        {
            Debug.Log($"{gameStateMachine.CheckHasPlayerWon()} has won");
        }
        gameStateMachine.PopState();
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        gameStateMachine.DoubleAgendaManager.ButtonPressed -= DoubleAgendaManager_ButtonPressed;
        card = null;
        this.gameStateMachine = null;
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        yield break;
    }
}
