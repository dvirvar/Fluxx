using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        List<Card> handCards = gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer);
        if (handCards.Remove(card))
        {
            //Card is from hand
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
                    } else if (newRuleCard.NewRuleCardInfo.NewRuleType == NewRuleCardType.FirstPlayRandom)
                    {
                        gameStateMachine.IsFirstPlayRandom = true;
                    }
                    newRuleCard.SetCanBeSelected(newRuleCard.NewRuleCardInfo.NewRuleType.Actionable());
                    break;
            }
            gameStateMachine.Board.RearrangePlayerHand(gameStateMachine.CurrentPlayer);
            gameStateMachine.PlayedCard();
        } else if (card is NewRuleCard newRule && gameStateMachine.Board.GetNewRuleCards().Contains(newRule))
        {
            //Card is in new rules on board
        }

        if (gameStateMachine.Played >= gameStateMachine.CurrentPlays)
        {
            gameStateMachine.SetState(new StartOfTurnState(gameStateMachine, gameStateMachine.CurrentPlayer == GameStateMachine.Player.Player1 ? GameStateMachine.Player.Player2 : GameStateMachine.Player.Player1));
        }
        yield break;
    }
}
