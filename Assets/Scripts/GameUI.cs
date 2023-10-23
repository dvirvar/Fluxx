using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] TMP_Text currentPlayerText, playedText;
    [field: SerializeField] public RockPaperScissorsManager RockPaperScissorsManager { get; private set; }
    [field: SerializeField] public DoubleAgendaManager DoubleAgendaManager { get; private set; }
    [field: SerializeField] public LimitUI LimitUI { get; private set; }
    [field: SerializeField] public GameObject LetsDoThatAgainUI { get; private set; }
    [field: SerializeField] public PlayerWonUI PlayerWonUI { get; private set; }

    public void SetCurrentPlayer(string currentPlayer)
    {
        currentPlayerText.text = currentPlayer;
    }

    public void SetPlayed(string played)
    {
        playedText.text = played;
    }
}
