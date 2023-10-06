using System.Collections;
using System.Linq;

public class EmptyTheTrashState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        var originalDiscardPileCards = gameStateMachine.Board.GetDiscardPileCards();
        var discardPileCards = originalDiscardPileCards.ToList();
        originalDiscardPileCards.Clear();
        var emptyTheTrashCardIndex = discardPileCards.FindLastIndex(c=>c is ActionCard action && action.ActionCardInfo.ActionType == ActionCardType.EmptyTheTrash);
        var emptyTheTrashCard = discardPileCards[emptyTheTrashCardIndex];
        discardPileCards.RemoveAt(emptyTheTrashCardIndex);
        var cards = gameStateMachine.Board.GetDeckCards().ToList();
        cards.AddRange(discardPileCards);
        gameStateMachine.Board.AddToDiscardPile(emptyTheTrashCard);
        cards.Shuffle();
        gameStateMachine.Board.SetDeck(cards);
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
