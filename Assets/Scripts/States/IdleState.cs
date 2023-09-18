using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    GameStateMachine gameStateMachine;
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        this.gameStateMachine = gameStateMachine;
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        yield break;
    }

    public override IEnumerator Play(Card card)
    {
        if (gameStateMachine.CurrentPlayer == GameStateMachine.Player.Player1)
        {
            if (gameStateMachine.Board.GetPlayer1HandCards().Remove(card))
            {
                switch (card)
                {
                    case KeeperCard keeperCard:
                        gameStateMachine.Board.AddKeeperToPlayer1(keeperCard);
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
                        gameStateMachine.Board.GetPlayer1HandCards().Add(actionCard);//TODO
                        break;
                    case NewRuleCard newRuleCard:
                        gameStateMachine.Board.AddNewRule(newRuleCard);
                        var ruleType = newRuleCard.NewRuleCardInfo.NewRuleType.GetRuleType();
                        if (ruleType == RuleType.Draw)
                        {
                            gameStateMachine.currentDraws = newRuleCard.NewRuleCardInfo.NewRuleType switch
                            {
                                NewRuleCardType.Draw2 => 2,
                                NewRuleCardType.Draw3 => 3,
                                NewRuleCardType.Draw4 => 4,
                                NewRuleCardType.Draw5 => 5,
                                _ => throw new NotImplementedException(),
                            };
                            DrawToMatchDraws(gameStateMachine);
                        } else if (ruleType == RuleType.Play)
                        {
                            gameStateMachine.currentPlays = newRuleCard.NewRuleCardInfo.NewRuleType switch
                            {
                                NewRuleCardType.Play2 => 2,
                                NewRuleCardType.Play3 => 3,
                                NewRuleCardType.Play4 => 4,
                                _ => 1
                            };
                        }
                        newRuleCard.SetCanBeSelected(newRuleCard.NewRuleCardInfo.NewRuleType.Actionable());
                        break;
                }
                gameStateMachine.Board.RearrangePlayer1Hand();
                gameStateMachine.PlayedCard();
            }
            else if (card is NewRuleCard newRule && gameStateMachine.Board.GetNewRuleCards().Contains(newRule))
            {
                //TODO
            }
        }
        else
        {
            if (gameStateMachine.Board.GetPlayer2HandCards().Remove(card))
            {
                switch (card)
                {
                    case KeeperCard keeperCard:
                        gameStateMachine.Board.AddKeeperToPlayer2(keeperCard);
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
                        gameStateMachine.Board.GetPlayer2HandCards().Add(actionCard);//TODO
                        break;
                    case NewRuleCard newRuleCard:
                        gameStateMachine.Board.AddNewRule(newRuleCard);
                        var ruleType = newRuleCard.NewRuleCardInfo.NewRuleType.GetRuleType();
                        if (ruleType == RuleType.Draw)
                        {
                            gameStateMachine.currentDraws = newRuleCard.NewRuleCardInfo.NewRuleType switch
                            {
                                NewRuleCardType.Draw2 => 2,
                                NewRuleCardType.Draw3 => 3,
                                NewRuleCardType.Draw4 => 4,
                                NewRuleCardType.Draw5 => 5,
                                _ => throw new NotImplementedException(),
                            };
                            DrawToMatchDraws(gameStateMachine);
                        }
                        else if (ruleType == RuleType.Play)
                        {
                            gameStateMachine.currentPlays = newRuleCard.NewRuleCardInfo.NewRuleType switch
                            {
                                NewRuleCardType.Play2 => 2,
                                NewRuleCardType.Play3 => 3,
                                NewRuleCardType.Play4 => 4,
                                _ => 1
                            };
                        }
                        newRuleCard.SetCanBeSelected(newRuleCard.NewRuleCardInfo.NewRuleType.Actionable());
                        break;
                }
                gameStateMachine.Board.RearrangePlayer2Hand();
                gameStateMachine.PlayedCard();
            }
            else if (card is NewRuleCard newRule && gameStateMachine.Board.GetNewRuleCards().Contains(newRule))
            {
                //TODO
            }
        }

        if (gameStateMachine.Played >= gameStateMachine.currentPlays)
        {
            gameStateMachine.SetState(new StartOfTurnState(gameStateMachine, gameStateMachine.CurrentPlayer == GameStateMachine.Player.Player1 ? GameStateMachine.Player.Player2 : GameStateMachine.Player.Player1));
        }
        yield break;
    }

    void DrawToMatchDraws(GameStateMachine gameStateMachine)
    {
        for (; gameStateMachine.Drawed < gameStateMachine.currentDraws; gameStateMachine.DrawedCard())
        {
            var card = gameStateMachine.Board.DrawCard();
            if (card == null)
            {
                continue;
            }
            card.SetCanBeSelected(true);
            if (gameStateMachine.CurrentPlayer == GameStateMachine.Player.Player1)
            {
                gameStateMachine.Board.AddCardToPlayer1Hand(card);
            }
            else
            {
                gameStateMachine.Board.AddCardToPlayer2Hand(card);
            }
        }
    }
}
