using System.Collections;
using UnityEngine;

public class PlayHandCardState : State
{
    Card card;
    readonly bool freePlay = false;

    public PlayHandCardState(Card card)
    {
        this.card = card;
    }

    public PlayHandCardState(Card card, bool freePlay)
    {
        this.card = card;
        this.freePlay = freePlay;
    }

    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        switch (card)
        {
            case KeeperCard keeperCard:
                gameStateMachine.Board.AddKeeperTo(gameStateMachine.CurrentPlayer, keeperCard);
                keeperCard.SetCanBeSelected(false);
                if (gameStateMachine.CheckHasPlayerWon() != null)
                {
                    Debug.Log($"{gameStateMachine.CheckHasPlayerWon()} has won");
                }
                break;
            case GoalCard goalCard:
                gameStateMachine.Board.SetCurrentGoal(goalCard);
                goalCard.SetCanBeSelected(false);
                if (gameStateMachine.CheckHasPlayerWon() != null)
                {
                    Debug.Log($"{gameStateMachine.CheckHasPlayerWon()} has won");
                }
                break;
            case ActionCard actionCard:
                actionCard.SetCanBeSelected(false);
                gameStateMachine.Board.AddToDiscardPile(actionCard);//TODO
                break;
            case NewRuleCard newRuleCard:
                gameStateMachine.Board.AddNewRule(newRuleCard);
                var ruleType = newRuleCard.NewRuleCardInfo.NewRuleType.GetRuleType();
                if (ruleType == RuleType.Draw)
                {
                    gameStateMachine.SetCurrentDrawRule(newRuleCard.NewRuleCardInfo.NewRuleType);
                    gameStateMachine.DrawToMatchDraws();
                }
                else if (ruleType == RuleType.Play)
                {
                    gameStateMachine.SetCurrentPlayRule(newRuleCard.NewRuleCardInfo.NewRuleType);
                }
                else if (newRuleCard.NewRuleCardInfo.NewRuleType == NewRuleCardType.Inflation)
                {
                    gameStateMachine.Inflation = true;
                    gameStateMachine.DrawToMatchDraws();
                }
                else if (newRuleCard.NewRuleCardInfo.NewRuleType == NewRuleCardType.FirstPlayRandom)
                {
                    gameStateMachine.IsFirstPlayRandom = true;
                }
                newRuleCard.SetCanBeSelected(newRuleCard.NewRuleCardInfo.NewRuleType.Actionable());
                break;
        }
        gameStateMachine.Board.RearrangePlayerHand(gameStateMachine.CurrentPlayer);
        if (!freePlay)
        {
            gameStateMachine.PlayedCard();
        }
        gameStateMachine.SetState(new EndOfTurnState());
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        card = null;
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        yield break;
    }
}
