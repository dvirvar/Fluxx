using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GoalMillState : State
{
    readonly List<GoalCard> goalCardsToDiscard = new();
    readonly List<NewRuleCard> rulesThatCouldBeSelected = new();
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        var handCards = gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer);
        if (handCards.Count(c=>c is GoalCard) == 0)
        {
            gameStateMachine.PopState();
            yield break;
        }
        rulesThatCouldBeSelected.AddRange(gameStateMachine.Board.GetNewRuleCards().FindAll(r => r.CanBeSelected));
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(false);
        }
        gameStateMachine.Board.GetNewRuleCards().First(r => r.NewRuleCardInfo.NewRuleType == NewRuleCardType.GoalMill).SetCanBeSelected(true);
        foreach (var card in handCards)
        {
            card.SetCanBeSelected(card is GoalCard);
        }
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(true);
        }
        goalCardsToDiscard.Clear();
        rulesThatCouldBeSelected.Clear();
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        if (gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer).Contains(card) && card is GoalCard goalCard) {
            goalCard.SetCanBeSelected(false);
            goalCardsToDiscard.Add(goalCard);
        } else if (card is NewRuleCard ruleCard && ruleCard.NewRuleCardInfo.NewRuleType == NewRuleCardType.GoalMill)
        {
            card.SetCanBeSelected(false);
            var handCards = gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer);
            foreach (var discardCard in goalCardsToDiscard)
            {
                handCards.Remove(discardCard);
                gameStateMachine.Board.AddToDiscardPile(discardCard);
                var drawedCard = gameStateMachine.Board.DrawCard();
                if (drawedCard != null)
                {
                    gameStateMachine.Board.AddHandCardTo(gameStateMachine.CurrentPlayer, drawedCard);
                }
            }
            gameStateMachine.Board.RearrangePlayerHand(gameStateMachine.CurrentPlayer);
            gameStateMachine.PopState();
        }
        yield break;
    }
}
