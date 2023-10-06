using System.Collections;
using System.Linq;

public class TradeHandsState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        var otherPlayer = gameStateMachine.CurrentPlayer == GameStateMachine.Player.Player1 ? GameStateMachine.Player.Player2 : GameStateMachine.Player.Player1;
        var currentPlayerHand = gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer);
        var otherPlayerHand = gameStateMachine.Board.GetPlayerHandCards(otherPlayer);
        var currentPlayerHandCopy = currentPlayerHand.ToList();
        currentPlayerHand.Clear();
        foreach (var card in otherPlayerHand)
        {
            gameStateMachine.Board.AddHandCardTo(gameStateMachine.CurrentPlayer, card);
        }
        gameStateMachine.Board.RearrangePlayerHand(gameStateMachine.CurrentPlayer);
        
        otherPlayerHand.Clear();
        foreach (var card in currentPlayerHandCopy)
        {
            gameStateMachine.Board.AddHandCardTo(otherPlayer, card);
        }
        gameStateMachine.Board.RearrangePlayerHand(otherPlayer);

        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(gameStateMachine.CurrentPlayer, true, true);
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(otherPlayer, false, false);

        gameStateMachine.PopState();
        yield break;
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
