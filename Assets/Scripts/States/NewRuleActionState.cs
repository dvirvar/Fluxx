using System;
using System.Collections;
using UnityEngine.Assertions;

public class NewRuleActionState : State
{
    NewRuleCard card;

    public NewRuleActionState(NewRuleCard card)
    {
        this.card = card;
        Assert.IsTrue(card.NewRuleCardInfo.NewRuleType.Actionable());
    }

    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        card.SetCanBeSelected(false);
        switch (card.NewRuleCardInfo.NewRuleType)
        {
            case NewRuleCardType.GetOnWithIt:
                var handCards = gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer);
                for (int i = handCards.Count - 1; i >= 0; --i)
                {
                    var handCard = handCards[i];
                    handCard.SetCanBeSelected(false);
                    handCards.RemoveAt(i);
                    gameStateMachine.Board.AddToDiscardPile(handCard);
                }
                for (var i = 0; i < (gameStateMachine.Inflation ? 4 : 3); ++i)
                {
                    var drawedCard = gameStateMachine.Board.DrawCard();
                    if (drawedCard != null)
                    {
                        gameStateMachine.Board.AddHandCardTo(gameStateMachine.CurrentPlayer, drawedCard);
                    }
                }
                gameStateMachine.ResetAndSetState(new ChangeCurrentPlayerState());
                break;
            case NewRuleCardType.Recycling:
                gameStateMachine.SetState(new RecyclingState());
                break;
            case NewRuleCardType.SwapPlaysForDraws:
                var board = gameStateMachine.Board;
                var cardsToDraw = Math.Min(gameStateMachine.CurrentPlays - gameStateMachine.Played, board.GetPlayerHandCards(gameStateMachine.CurrentPlayer).Count);
                for (var i = 0; i < cardsToDraw; ++i)
                {
                    var drawedCard = board.DrawCard();
                    if (drawedCard != null)
                    {
                        board.AddHandCardTo(gameStateMachine.CurrentPlayer, drawedCard);
                    }
                }
                if ((board.GetCurrentGoalCard() != null && board.GetCurrentGoalCard().GoalCardInfo.GoalType == GoalCardType.TenCardsInHand) || (board.GetSecondCurrentGoalCard() != null && board.GetSecondCurrentGoalCard().GoalCardInfo.GoalType == GoalCardType.TenCardsInHand))
                {
                    var state = gameStateMachine.CheckHasPlayerWon();
                    if (state != null)
                    {
                        gameStateMachine.ResetAndSetState(state);
                        break;
                    }
                }
                gameStateMachine.ResetAndSetState(new ChangeCurrentPlayerState());
                break;
            case NewRuleCardType.GoalMill:
                gameStateMachine.SetState(new GoalMillState());
                break;
            case NewRuleCardType.MysteryPlay:
                var cardToPlay = gameStateMachine.Board.DrawCard();
                if (cardToPlay != null)
                {
                    gameStateMachine.SetState(new HandCardActionState(cardToPlay, true));
                } else
                {
                    gameStateMachine.PopState();
                }
                break;
        }
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
