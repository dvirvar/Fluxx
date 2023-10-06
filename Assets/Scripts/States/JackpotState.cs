using System.Collections;

public class JackpotState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        var amountToDraw = gameStateMachine.Inflation ? 4 : 3;
        for (int i = 0; i < amountToDraw; ++i)
        {
            var drawedCard = gameStateMachine.Board.DrawCard();
            if (drawedCard != null)
            {
                gameStateMachine.Board.AddHandCardTo(gameStateMachine.CurrentPlayer, drawedCard);
            }
        }
        gameStateMachine.PopState();
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
