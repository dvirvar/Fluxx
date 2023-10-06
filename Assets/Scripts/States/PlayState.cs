using System.Collections;

public class PlayState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        yield break;
    }

    public override IEnumerator OnResume(GameStateMachine gameStateMachine)
    {
        gameStateMachine.SetState(new EndOfPlayState());
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        var handCards = gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer);
        if (handCards.Remove(card))
        {
            //Card is from hand
            gameStateMachine.PushState(new HandCardActionState(card));
        } else if (card is NewRuleCard newRule && gameStateMachine.Board.GetNewRuleCards().Contains(newRule))
        {
            //Card is in new rules on board
            gameStateMachine.PushState(new NewRuleActionState(newRule));
        }
        yield break;
    }
}
