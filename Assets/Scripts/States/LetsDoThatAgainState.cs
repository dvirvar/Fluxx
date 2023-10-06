using System.Collections;
using System.Collections.Generic;

public class LetsDoThatAgainState : State
{
    readonly List<Card> usableCards = new();
    
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        usableCards.AddRange(gameStateMachine.Board.GetDiscardPileCards().FindAll(c => c is ActionCard || c is NewRuleCard));
        usableCards.RemoveAt(usableCards.Count - 1);//Lets do that again card
        if (usableCards.Count == 0 ) { 
            gameStateMachine.PopState();
            yield break;
        }        
        foreach (Card card in usableCards)
        {
            card.SetCanBeSelected(true);
        }
        gameStateMachine.LookAtDiscardPile(true);
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        foreach (Card card in usableCards)
        {
            card.SetCanBeSelected(false);
        }
        usableCards.Clear();
        gameStateMachine.LookAtDiscardPile(false);
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        if (usableCards.Remove(card))
        {
            gameStateMachine.Board.GetDiscardPileCards().Remove(card);
            gameStateMachine.Board.RearrangeDiscardPile();
            gameStateMachine.SetState(new HandCardActionState(card, true));
        }
        yield break;
    }
}
