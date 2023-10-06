using System.Collections;

public class DiscardAndDrawState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        var handCards = gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer);
        var cardsToDraw = handCards.Count;
        for (int i = cardsToDraw - 1; i >= 0; --i)
        {
            var card = handCards[i];
            card.SetCanBeSelected(false);
            handCards.RemoveAt(i);
            gameStateMachine.Board.AddToDiscardPile(card);
        }
        for (int i = 0; i < cardsToDraw; ++i)
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
