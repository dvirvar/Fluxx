using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class GameStateMachine: MonoBehaviour
{
    public enum Player
    {
        Player1,
        Player2,
    }
    public Board Board { get; private set; }
    State state = State.NULL;

    [HideInInspector] public Player CurrentPlayer = Player.Player1;
    public int Played { get; private set; } = 0;
    public int Drawed { get; private set; } = 0;
    [HideInInspector] public int currentPlays = 1;
    [HideInInspector] public int currentDraws = 1;

    public void Init(State state, Board board)
    {
        this.Board = board;
        SetState(state);
    }

    public void SetState(State state)
    {
        StartCoroutine(SetStateC(state));
    }

    IEnumerator SetStateC(State state)
    {
        yield return this.state.OnExit(this);
        this.state = state;
        yield return this.state.OnEnter(this);
    }

    public void PlayCard(Card card)
    {
        StartCoroutine(state.Play(card));
    }

    public void DrawedCard()
    {
        ++Drawed;
    }

    public void PlayedCard()
    {
        ++Played;
    }

    public void ResetDrawedAndPlayed()
    {
        Played = 0;
        Drawed = 0;
    }

    public Player? CheckHasPlayerWon()
    {
        var goalsList = new List<GoalCardInfo>();
        var currentGoalCard = Board.GetCurrentGoalCard();
        if (currentGoalCard != null)
        {
            goalsList.Add(currentGoalCard.GoalCardInfo);
        }
        var secondCurrentGoalCard = Board.GetSecondCurrentGoalCard();
        if (secondCurrentGoalCard != null)
        {
            goalsList.Add(secondCurrentGoalCard.GoalCardInfo);
        }
        foreach (var goal in goalsList)
        {
            switch (goal.GoalType.GetGoalType())
            {
                case GoalType.Keepers:
                    var keepersToAchieveGoal = goal.GoalType.GetKeepersToAchieveGoal();
                    var player1Keepers = Board.GetPlayer1KeeperCards();
                    foreach (var keeper in keepersToAchieveGoal)
                    {
                        if (!player1Keepers.Exists(k => k.KeeperCardInfo.KeeperType == keeper))
                        {
                            goto p2;
                        }
                    }
                    return Player.Player1;
                p2:
                    var player2Keepers = Board.GetPlayer2KeeperCards();
                    foreach (var keeper in keepersToAchieveGoal)
                    {
                        if (!player2Keepers.Exists(k => k.KeeperCardInfo.KeeperType == keeper))
                        {
                            return null;
                        }
                    }
                    return Player.Player2;
                case GoalType.NumberOfKeepersInPlay:
                    var numberOfKeepersToAchieveGoal = goal.GoalType.GetNumberOfKeepersToAcheiveGoal();
                    var player1KeepersCount = Board.GetPlayer1KeeperCards().Count;
                    var player2KeepersCount = Board.GetPlayer2KeeperCards().Count;
                    if (player1KeepersCount >= numberOfKeepersToAchieveGoal && player2KeepersCount >= numberOfKeepersToAchieveGoal) 
                    {
                        return null;
                    }
                    if (player1KeepersCount >= numberOfKeepersToAchieveGoal)
                    {
                        return Player.Player1;
                    }
                    if (player2KeepersCount >= numberOfKeepersToAchieveGoal)
                    {
                        return Player.Player2;
                    }
                    break;
                case GoalType.NumberOfCardsInHand:
                    var numberOfCardsInCardsToAchieveGoal = goal.GoalType.GetNumberOfCardsInHandAcheiveGoal();
                    var player1CardsCount = Board.GetPlayer1HandCards().Count;
                    var player2CardsCount = Board.GetPlayer2HandCards().Count;
                    if (player1CardsCount >= numberOfCardsInCardsToAchieveGoal && player2CardsCount >= numberOfCardsInCardsToAchieveGoal)
                    {
                        return null;
                    }
                    if (player1CardsCount >= numberOfCardsInCardsToAchieveGoal)
                    {
                        return Player.Player1;
                    }
                    if (player2CardsCount >= numberOfCardsInCardsToAchieveGoal)
                    {
                        return Player.Player2;
                    }
                    break;
                case GoalType.KeeperAndAtLeastOneFood:
                    var keeperForAtLeastOneFoodType = goal.GoalType.GetKeeperForAtLeastOneFoodToAcheiveGoal();
                    var p1Keepers = Board.GetPlayer1KeeperCards();
                    if (p1Keepers.Exists(k=> k.KeeperCardInfo.KeeperType == keeperForAtLeastOneFoodType) && p1Keepers.Exists(k => k.KeeperCardInfo.KeeperType.IsFood()))
                    {
                        return Player.Player1;
                    }
                    var p2Keepers = Board.GetPlayer1KeeperCards();
                    if (p2Keepers.Exists(k => k.KeeperCardInfo.KeeperType == keeperForAtLeastOneFoodType) && p2Keepers.Exists(k => k.KeeperCardInfo.KeeperType.IsFood()))
                    {
                        return Player.Player2;
                    }
                    break;
                case GoalType.KeeperAndNotInPlay:
                    var keeperAndNotInPlay = goal.GoalType.GetKeeperAndNotInPlayToAcheiveGoal();
                    var p1K = Board.GetPlayer1KeeperCards();
                    var p2K = Board.GetPlayer2KeeperCards();
                    if (p1K.Exists(k => k.KeeperCardInfo.KeeperType == keeperAndNotInPlay.KeeperNotInPlay) || p2K.Exists(k => k.KeeperCardInfo.KeeperType == keeperAndNotInPlay.KeeperNotInPlay))
                    {
                        return null;
                    }
                    if (p1K.Exists(k => k.KeeperCardInfo.KeeperType == keeperAndNotInPlay.KeeperInPlay))
                    {
                        return Player.Player1;
                    }
                    if (p2K.Exists(k => k.KeeperCardInfo.KeeperType == keeperAndNotInPlay.KeeperInPlay))
                    {
                        return Player.Player2;
                    }
                    break;
            }
        }
        return null;
    }
}
