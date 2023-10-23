using System.Collections;

public class RockPaperScissorsShowdownState : State
{
    GameStateMachine gameStateMachine;
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        this.gameStateMachine = gameStateMachine;
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(gameStateMachine.CurrentPlayer, false, false);
        var player = gameStateMachine.CurrentPlayer == GameStateMachine.Player.Player1 ? "Player 1" : "Player 2";
        var otherPlayer = gameStateMachine.CurrentPlayer != GameStateMachine.Player.Player1 ? "Player 1" : "Player 2";
        gameStateMachine.GameUI.RockPaperScissorsManager.StartGame(player, otherPlayer, gameStateMachine.Inflation ? 4 : 3);
        gameStateMachine.GameUI.RockPaperScissorsManager.PlayerHasWon += RockPaperScissorsManager_PlayerHasWon;
        yield break;
    }

    private void RockPaperScissorsManager_PlayerHasWon(int player)
    {
        var otherPlayer = gameStateMachine.CurrentPlayer == GameStateMachine.Player.Player1 ? GameStateMachine.Player.Player2 : GameStateMachine.Player.Player1;
        var playerWon = player == 1 ? gameStateMachine.CurrentPlayer : otherPlayer;
        var playerLost = playerWon == GameStateMachine.Player.Player1 ? GameStateMachine.Player.Player2 : GameStateMachine.Player.Player1;
        var playerLostCards = gameStateMachine.Board.GetPlayerHandCards(playerLost);
        foreach (var card in playerLostCards)
        {
            gameStateMachine.Board.AddHandCardTo(playerWon, card);
        }
        playerLostCards.Clear();
        gameStateMachine.PopState();
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        gameStateMachine.GameUI.RockPaperScissorsManager.PlayerHasWon -= RockPaperScissorsManager_PlayerHasWon;
        this.gameStateMachine = null;
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        yield break;
    }
}
