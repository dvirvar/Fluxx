using TMPro;
using UnityEngine;

public class PlayerWonUI : MonoBehaviour
{

    [SerializeField] TMP_Text text;

    public void SetWinningPlayer(GameStateMachine.Player player)
    {
        var playerStr = player == GameStateMachine.Player.Player1 ? "Player 1" : "Player 2";
        text.text = $"{playerStr} has won!";
    }
}
