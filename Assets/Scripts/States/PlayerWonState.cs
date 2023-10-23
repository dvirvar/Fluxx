using System.Collections;
using UnityEngine;

public class PlayerWonState : State
{
    readonly GameStateMachine.Player winningPlayer;
    public PlayerWonState(GameStateMachine.Player player) {
        winningPlayer = player;
    }

    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        Debug.Log($"Winning player: {winningPlayer}");
        gameStateMachine.GameUI.PlayerWonUI.gameObject.SetActive(true);
        gameStateMachine.GameUI.PlayerWonUI.SetWinningPlayer(winningPlayer);
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        gameStateMachine.GameUI.PlayerWonUI.gameObject.SetActive(false);
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        yield break;
    }
}
