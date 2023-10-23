using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ShareTheWealthState : State
{
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        var keepers = new List<KeeperCard>();
        var allPlayers = EnumUtil.GetArrayOf<GameStateMachine.Player>();
        foreach (var player in allPlayers)
        {
            var keeperCards = gameStateMachine.Board.GetPlayerKeeperCards(player);
            keepers.AddRange(keeperCards);
            keeperCards.Clear();
        }
        if (keepers.Count == 0) {
            gameStateMachine.PopState();
            yield break;
        }
        keepers.Shuffle();
        int currentPlayerIndex = allPlayers.ToList().IndexOf(gameStateMachine.CurrentPlayer);
        for (var i = 0; i < keepers.Count; ++i)
        {
            gameStateMachine.Board.AddKeeperTo(allPlayers[currentPlayerIndex], keepers[i]);
            currentPlayerIndex = (currentPlayerIndex + 1) % allPlayers.Length;
        }
        var state = gameStateMachine.CheckHasPlayerWon();
        if (state != null)
        {
            gameStateMachine.ResetAndSetState(state);
        }
        else
        {
            gameStateMachine.PopState();
        }
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
