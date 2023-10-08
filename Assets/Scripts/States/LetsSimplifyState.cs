using System.Collections;
using System.Collections.Generic;

public class LetsSimplifyState : State
{
    readonly List<NewRuleCard> rulesThatCouldBeSelected = new();
    readonly List<NewRuleCard> rulesToDiscard = new();
    int maxNewRulesToDiscard = 0;
    Card card;

    public LetsSimplifyState(ActionCard card)
    {
        this.card = card;
    }

    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        var newRuleCards = gameStateMachine.Board.GetNewRuleCards();
        if (newRuleCards.Count == 0)
        {
            gameStateMachine.PopState();
            yield break;
        }
        card.SetCanBeSelected(true);
        //+ 1 will round up when you have odd number of cards
        maxNewRulesToDiscard = (newRuleCards.Count + 1)/ 2;
        rulesThatCouldBeSelected.AddRange(newRuleCards.FindAll(r => r.CanBeSelected));
        foreach (var rule in newRuleCards)
        {
            rule.SetCanBeSelected(true);
        }
        foreach (var card in gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer))
        {
            card.SetCanBeSelected(false);
        }
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        foreach(var rule in gameStateMachine.Board.GetNewRuleCards())
        {
            rule.SetCanBeSelected(false);
        }
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(true);
        }
        foreach (var card in gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer))
        {
            card.SetCanBeSelected(true);
        }
        card.SetCanBeSelected(false);
        rulesThatCouldBeSelected.Clear();
        rulesToDiscard.Clear();
        card = null;
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        if (card is NewRuleCard newRuleCard)
        {
            newRuleCard.SetCanBeSelected(false);
            rulesThatCouldBeSelected.Remove(newRuleCard);
            rulesToDiscard.Add(newRuleCard);
            if (rulesToDiscard.Count >= maxNewRulesToDiscard)
            {
                DiscardRulesAndPopState(gameStateMachine);
            }
        } else if (card is ActionCard actionCard && actionCard.ActionCardInfo.ActionType == ActionCardType.LetsSimplify)
        {
            actionCard.SetCanBeSelected(false);
            DiscardRulesAndPopState(gameStateMachine);
        }
        yield break;
    }

    void DiscardRulesAndPopState(GameStateMachine gameStateMachine)
    {
        var state = gameStateMachine.DiscardNewRules(rulesToDiscard);
        if (state != null)
        {
            gameStateMachine.SetState(state);
        } else
        {
            gameStateMachine.PopState();
        }
    }
}
