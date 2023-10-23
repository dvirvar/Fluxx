using System.Collections;
using System.Collections.Generic;

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
            var state = gameStateMachine.CheckHasPlayerWon();
            if (state != null)
            {
                gameStateMachine.ResetAndSetState(state);
            } else
            {
                gameStateMachine.PopState();
            }
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
        gameStateMachine.GameUI.DoubleAgendaManager.ChooseDoubleAgenda(firstName, secondName);
        gameStateMachine.GameUI.DoubleAgendaManager.ButtonPressed += DoubleAgendaManager_ButtonPressed;
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
        var state = gameStateMachine.CheckHasPlayerWon();
        if (state != null)
        {
            gameStateMachine.ResetAndSetState(state);
        } else
        {
            gameStateMachine.PopState();
        }
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        gameStateMachine.GameUI.DoubleAgendaManager.ButtonPressed -= DoubleAgendaManager_ButtonPressed;
        card = null;
        this.gameStateMachine = null;
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        yield break;
    }
}
