using System.Collections;
using UnityEngine;

public class HandCardActionState : State
{
    Card card;
    readonly bool freePlay = false;

    public HandCardActionState(Card card)
    {
        this.card = card;
    }

    public HandCardActionState(Card card, bool freePlay)
    {
        this.card = card;
        this.freePlay = freePlay;
    }

    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        gameStateMachine.Board.RearrangePlayerHand(gameStateMachine.CurrentPlayer);
        if (!freePlay)
        {
            gameStateMachine.PlayedCard();
        }
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
                goalCard.SetCanBeSelected(false);
                gameStateMachine.SetState(new GoalCardActionState(goalCard));
                yield break;
            case ActionCard actionCard:
                actionCard.SetCanBeSelected(false);
                gameStateMachine.Board.AddToDiscardPile(actionCard);
                switch (actionCard.ActionCardInfo.ActionType)
                {
                    case ActionCardType.Draw3AndPlay2:
                        gameStateMachine.SetState(new Draw3AndPlay2State());
                        break;
                    case ActionCardType.EverybodyGets1:
                        gameStateMachine.SetState(new EverybodyGets1State());
                        break;
                    case ActionCardType.RotateHands:
                        gameStateMachine.SetState(new RotateHandsState());
                        break;
                    case ActionCardType.StealAKeeper:
                        gameStateMachine.SetState(new StealAKeeperState());
                        break;
                    case ActionCardType.LetsSimplify:
                        gameStateMachine.SetState(new LetsSimplifyState(actionCard));
                        break;
                    case ActionCardType.Jackpot:
                        gameStateMachine.SetState(new JackpotState());
                        break;
                    case ActionCardType.TrashAKeeper:
                        gameStateMachine.SetState(new TrashAKeeperState());
                        break;
                    case ActionCardType.TodaysSpecial:
                        gameStateMachine.SetState(new TodaysSpecialState());
                        break;
                    case ActionCardType.Draw2AndUseThem:
                        gameStateMachine.SetState(new Draw2AndUseThemState());
                        break;
                    case ActionCardType.TakeAnotherTurn:
                        gameStateMachine.TakeAnotherTurn = true;
                        gameStateMachine.PopState();
                        break;
                    case ActionCardType.RulesReset:
                        gameStateMachine.SetState(new RulesResetState());
                        break;
                    case ActionCardType.NoLimits:
                        gameStateMachine.SetState(new NoLimitsState());
                        break;
                    case ActionCardType.TrashANewRule:
                        gameStateMachine.SetState(new TrashANewRuleState());
                        break;
                    case ActionCardType.ZapACard:
                        gameStateMachine.SetState(new ZapACardState());
                        break;
                    case ActionCardType.DiscardAndDraw:
                        gameStateMachine.SetState(new DiscardAndDrawState());
                        break;
                    case ActionCardType.LetsDoThatAgain:
                        gameStateMachine.SetState(new LetsDoThatAgainState());
                        break;
                    case ActionCardType.UseWhatYouTake:
                        gameStateMachine.SetState(new UseWhatYouTakeState());
                        break;
                    case ActionCardType.EmptyTheTrash:
                        gameStateMachine.SetState(new EmptyTheTrashState());
                        break;
                    case ActionCardType.RandomTax:
                        gameStateMachine.SetState(new RandomTaxState());
                        break;
                    case ActionCardType.TradeHands:
                        gameStateMachine.SetState(new TradeHandsState());
                        break;
                    case ActionCardType.RockPaperScissorsShowdown:
                        gameStateMachine.SetState(new RockPaperScissorsShowdownState());
                        break;
                    case ActionCardType.ExchangeKeepers:
                        gameStateMachine.SetState(new ExchangeKeepersState());
                        break;
                    case ActionCardType.ShareTheWealth:
                        gameStateMachine.SetState(new ShareTheWealthState());
                        break;
                }
                yield break;
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
                } else if (newRuleCard.NewRuleCardInfo.NewRuleType == NewRuleCardType.DoubleAgenda)
                {
                    gameStateMachine.HasDoubleAgenda = true;
                }
                newRuleCard.SetCanBeSelected(newRuleCard.NewRuleCardInfo.NewRuleType.Actionable());
                break;
        }
        gameStateMachine.PopState();
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
