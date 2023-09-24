using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] TMP_Text currentPlayerText, playedText;

    public void SetCurrentPlayer(string currentPlayer)
    {
        currentPlayerText.text = currentPlayer;
    }

    public void SetPlayed(string played)
    {
        playedText.text = played;
    }
}
