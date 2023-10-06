using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RockPaperScissorsManager : MonoBehaviour
{
    ///
    /// Summary: 
    /// Returns 1 if player1 won otherwise returns 2
    public event Action<int> PlayerHasWon = delegate { };
    enum Option
    {
        R,P,S
    }
    [SerializeField] Button rock, paper, scissors;
    [SerializeField] TMP_Text scoreText, currentPlayerText;
    Option? player1Option;
    string player1, player2;
    int rounds;
    int player1Score, player2Score;

    void Choose(Option option)
    {
        if (player1Option == null)
        {
            player1Option = option;
        } else
        {
            if (player1Option != option)
            {
                if (HasPlayerWonRound(player1Option.Value, option))
                {
                    ++player1Score;
                }
                else
                {
                    ++player2Score;
                }
                UpdateScore();
                NotifyIfGameFinished();
            }
            player1Option = null;
        }
        UpdateCurrentPlayer();
    }

    bool HasPlayerWonRound(Option player, Option other)
    {
        return (player == Option.R && other == Option.S) || 
            (player == Option.P && other == Option.R) || 
            (player == Option.S && other == Option.P);
    }

    void UpdateCurrentPlayer()
    {
        var currentPlayer = player1Option == null ? player1 : player2;
        currentPlayerText.text = $"{currentPlayer} choose:";
    }

    void UpdateScore()
    {
        scoreText.text = $"Score:\n{player1Score} : {player2Score}";
    }

    void NotifyIfGameFinished()
    {
        var roundsToFinishGame = rounds - 1;
        if (player1Score >= roundsToFinishGame)
        {
            PlayerHasWon.Invoke(1);
            EndGame();
        } else if (player2Score >= roundsToFinishGame)
        {
            PlayerHasWon.Invoke(2);
            EndGame();
        }
    }

    void EndGame()
    {
        rock.onClick.RemoveAllListeners();
        paper.onClick.RemoveAllListeners();
        scissors.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }

    public void StartGame(string player1, string player2, int rounds)
    {
        this.player1 = player1;
        this.player2 = player2;
        this.rounds = rounds;
        player1Score = 0;
        player2Score = 0;
        rock.onClick.AddListener(() => { Choose(Option.R); });
        paper.onClick.AddListener(() => { Choose(Option.P); });
        scissors.onClick.AddListener(() => { Choose(Option.S); });
        UpdateCurrentPlayer();
        gameObject.SetActive(true);
    }
}
