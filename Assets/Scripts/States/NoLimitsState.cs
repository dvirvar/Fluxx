using System.Collections;

public class NoLimitsState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        var newRuleCards = gameStateMachine.Board.GetNewRuleCards();
        for (int i = 0; i < newRuleCards.Count; ++i)
        {
            if (IsRuleLimiting(newRuleCards[i]))
            {
                newRuleCards[i].SetCanBeSelected(false);
                gameStateMachine.Board.AddToDiscardPile(newRuleCards[i]);
                newRuleCards.RemoveAt(i);
            }
        }
        gameStateMachine.Board.RearrangeNewRules();
        gameStateMachine.PopState();
        yield break;
    }

    bool IsRuleLimiting(NewRuleCard card)
    {
        var ruleType = card.NewRuleCardInfo.NewRuleType.GetRuleType();
        return ruleType == RuleType.HandLimit || ruleType == RuleType.KeeperLimit;
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
