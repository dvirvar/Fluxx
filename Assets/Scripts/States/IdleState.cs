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
            gameStateMachine.SetState(new PlayHandCardState(card));
        } else if (card is NewRuleCard newRule && gameStateMachine.Board.GetNewRuleCards().Contains(newRule))
        {
            //Card is in new rules on board
            gameStateMachine.SetState(new NewRuleActionState(newRule));
        }
        yield break;
    }
}
