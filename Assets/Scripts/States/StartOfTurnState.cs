using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOfTurnState : State
{

    public StartOfTurnState(GameStateMachine gameStateMachine, GameStateMachine.Player currentPlayer) {
        gameStateMachine.CurrentPlayer = currentPlayer;
    }

    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        gameStateMachine.ResetDrawedAndPlayed();
        var showPlayer1 = gameStateMachine.CurrentPlayer == GameStateMachine.Player.Player1;
        gameStateMachine.Board.ShowPlayer1Hand(showPlayer1);
        gameStateMachine.Board.ShowPlayer2Hand(!showPlayer1);
        DrawToMatchDraws(gameStateMachine);
        gameStateMachine.SetState(new IdleState());
        yield break;
    }

    void DrawToMatchDraws(GameStateMachine gameStateMachine)
    {
        for (; gameStateMachine.Drawed < gameStateMachine.currentDraws; gameStateMachine.DrawedCard())
        {
            var card = gameStateMachine.Board.DrawCard();
            if (card == null)
            {
                continue;
            }
            card.SetCanBeSelected(true);
            if (gameStateMachine.CurrentPlayer == GameStateMachine.Player.Player1)
            {
                gameStateMachine.Board.AddCardToPlayer1Hand(card);
            }
            else
            {
                gameStateMachine.Board.AddCardToPlayer2Hand(card);
            }
        }
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
