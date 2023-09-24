using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

public class GameStateMachine: MonoBehaviour
{
    public enum Player
    {
        Player1,
        Player2,
    }
    GameUI gameUI;
    InputManager inputManager;
    new Camera camera;
    public Board Board { get; private set; }
    readonly List<State> states = new() { State.NULL };
    int currentStateIndex = 0;

    public Player CurrentPlayer {
        get { return _currentPlayer; }
        private set { 
            _currentPlayer = value;
            gameUI.SetCurrentPlayer(value == Player.Player1 ? "Player 1" : "Player 2");
        }
    }
    Player _currentPlayer;

    public int Played {
        get { return _played; }
        private set { 
            _played = value;
            gameUI.SetPlayed(value.ToString());
        } 
    }
    int _played = 0;
    public int Drawed { get; private set; } = 0;
    
    public int CurrentPlays => CurrentPlayRule switch
    {
        NewRuleCardType.Play2 => Inflation ? 3 : 2,
        NewRuleCardType.Play3 => Inflation ? 4 : 3,
        NewRuleCardType.Play4 => Inflation ? 5 : 4,
        NewRuleCardType.PlayAllBut1 => Board.GetPlayerHandCards(CurrentPlayer).Count - (Inflation ? 2 : 1) + Played,
        NewRuleCardType.PlayAll => Board.GetPlayerHandCards(CurrentPlayer).Count + Played,
        null => Inflation ? 2 : 1,
        _ => throw new NotImplementedException(),
    };
    public int CurrentDraws => CurrentDrawRule switch
    {
        NewRuleCardType.Draw2 => Inflation ? 3 : 2,
        NewRuleCardType.Draw3 => Inflation ? 4 : 3,
        NewRuleCardType.Draw4 => Inflation ? 5 : 4,
        NewRuleCardType.Draw5 => Inflation ? 6 : 5,
        null => Inflation ? 2 : 1,
        _ => throw new NotImplementedException(),
    };
    public NewRuleCardType? CurrentPlayRule { get; private set; }
    public NewRuleCardType? CurrentDrawRule { get; private set; }
    [HideInInspector] public bool Inflation = false;
    [HideInInspector] public bool IsFirstPlayRandom = false;

    public void StartGame(GameUI gameUI, InputManager inputManager, Camera camera, Board board)
    {
        this.gameUI = gameUI;
        this.inputManager = inputManager;
        this.camera = camera;
        Board = board;
        CurrentPlayer = Player.Player1;
        DrawToMatchDraws();
        SetState(new StartOfTurnState());
    }

    public void ResetAndSetState(State state)
    {
        StartCoroutine(ResetAndSetStateC(state));
    }

    public IEnumerator ResetAndSetStateC(State state)
    {
        for (; currentStateIndex >= 0; --currentStateIndex)
        {
            yield return states[currentStateIndex].OnExit(this);
            states.RemoveAt(currentStateIndex);
        }
        currentStateIndex = 0;
        states.Add(state);
        yield return states[currentStateIndex].OnEnter(this);
    }

    public void SetState(State state)
    {
        StartCoroutine(SetStateC(state));
    }

    IEnumerator SetStateC(State state)
    {
        yield return states[currentStateIndex].OnExit(this);
        states[currentStateIndex] = state;
        yield return states[currentStateIndex].OnEnter(this);
    }

    public void PushState(State state)
    {
        StartCoroutine(PushStateC(state));
    }

    IEnumerator PushStateC(State state)
    {
        states.Add(state);
        yield return states[++currentStateIndex].OnEnter(this);
    }

    public void PopState()
    {
        StartCoroutine(PopStateC());
    }

    public IEnumerator PopStateC()
    {
        yield return states[currentStateIndex].OnExit(this);
        states.RemoveAt(currentStateIndex--);
        yield return states[currentStateIndex].OnEnter(this);
    }

    public void PlayCard(Card card)
    {
        StartCoroutine(states[currentStateIndex].Play(this, card));
    }

    public void DrawedCard()
    {
        ++Drawed;
    }

    public void PlayedCard()
    {
        ++Played;
    }

    public void AdvanceTurn()
    {
        CurrentPlayer = CurrentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
        ResetDrawedAndPlayed();
        var lea = camera.transform.localEulerAngles;
        camera.transform.localEulerAngles = new Vector3(lea.x, CurrentPlayer == Player.Player1 ? 0 : 180, lea.z);
        inputManager.ReturnToCameraOrigin();
        inputManager.SetInverse(CurrentPlayer == Player.Player2);
    }

    void ResetDrawedAndPlayed()
    {
        Drawed = 0;
        Played = 0;
    }

    public void DrawToMatchDraws()
    {
        for (; Drawed < CurrentDraws; DrawedCard())
        {
            var card = Board.DrawCard();
            if (card == null)
            {
                continue;
            }
            card.SetCanBeSelected(true);
            Board.AddHandCardTo(CurrentPlayer, card);
        }
    }

    public void SetCurrentPlayRule(NewRuleCardType? rule)
    {
        if (rule == null)
        {
            CurrentPlayRule = rule;
            return;
        }
        Assert.IsTrue(rule.Value.GetRuleType() == RuleType.Play);
        CurrentPlayRule = rule;
    }

    public void SetCurrentDrawRule(NewRuleCardType? rule)
    {
        if (rule == null)
        {
            CurrentDrawRule = rule;
            return;
        }
        Assert.IsTrue(rule.Value.GetRuleType() == RuleType.Draw);
        CurrentDrawRule = rule;
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
        var playersMatchingGoals = new HashSet<Player>();
        foreach (var goal in goalsList)
        {
            switch (goal.GoalType.GetGoalType())
            {
                case GoalType.Keepers:
                    var keepersToAchieveGoal = goal.GoalType.GetKeepersToAchieveGoal();
                    foreach (var player in EnumUtil.GetArrayOf<Player>())
                    {
                        var keepers = Board.GetPlayerKeeperCards(player);
                        foreach (var keeper in keepersToAchieveGoal)
                        {
                            if (!keepers.Exists(k => k.KeeperCardInfo.KeeperType == keeper))
                            {
                                goto continueLoop;
                            }
                        }
                        playersMatchingGoals.Add(player);
                        break;
                    continueLoop: continue;
                    }
                    break;
                case GoalType.NumberOfKeepersInPlay:
                    var numberOfKeepersToAchieveGoal = goal.GoalType.GetNumberOfKeepersToAcheiveGoal();
                    if (Inflation)
                    {
                        ++numberOfKeepersToAchieveGoal;
                    }
                    Player? winnerByKeepersCount()
                    {
                        int numberOfWinners = 0;
                        Player? winnerPlayer = null;
                        foreach (var player in EnumUtil.GetArrayOf<Player>())
                        {
                            var keepers = Board.GetPlayerKeeperCards(player);
                            if (keepers.Count >= numberOfKeepersToAchieveGoal)
                            {
                                winnerPlayer = player;
                                ++numberOfWinners;
                            }
                        }
                        if (numberOfWinners == 1)
                        {
                            return winnerPlayer;
                        }
                        return null;
                    }
                    var wk = winnerByKeepersCount();
                    if (wk != null)
                    {
                        playersMatchingGoals.Add(wk.Value);
                    }
                    break;
                case GoalType.NumberOfCardsInHand:
                    var numberOfCardsInCardsToAchieveGoal = goal.GoalType.GetNumberOfCardsInHandAcheiveGoal();
                    if (Inflation)
                    {
                        ++numberOfCardsInCardsToAchieveGoal;
                    }
                    Player? winnerByCardsCountInHand()
                    {
                        int numberOfWinners = 0;
                        Player? winnerPlayer = null;
                        foreach (var player in EnumUtil.GetArrayOf<Player>())
                        {
                            var keepers = Board.GetPlayerHandCards(player);
                            if (keepers.Count >= numberOfCardsInCardsToAchieveGoal)
                            {
                                winnerPlayer = player;
                                ++numberOfWinners;
                            }
                        }
                        if (numberOfWinners == 1)
                        {
                            return winnerPlayer;
                        }
                        return null;
                    }
                    var wh = winnerByCardsCountInHand();
                    if (wh != null)
                    {
                        playersMatchingGoals.Add(wh.Value);
                    }
                    break;
                case GoalType.KeeperAndAtLeastOneFood:
                    var keeperForAtLeastOneFoodType = goal.GoalType.GetKeeperForAtLeastOneFoodToAcheiveGoal();
                    var atLeast = Inflation ? 2 : 1;
                    foreach (var player in EnumUtil.GetArrayOf<Player>())
                    {
                        var keepers = Board.GetPlayerKeeperCards(player);
                        if (keepers.Exists(k => k.KeeperCardInfo.KeeperType == keeperForAtLeastOneFoodType) && keepers.Count(k => k.KeeperCardInfo.KeeperType.IsFood()) >= atLeast)
                        {
                            playersMatchingGoals.Add(player);
                            break;
                        }
                    }
                    break;
                case GoalType.KeeperAndNotInPlay:
                    var keeperAndNotInPlay = goal.GoalType.GetKeeperAndNotInPlayToAcheiveGoal();
                    foreach (var player in EnumUtil.GetArrayOf<Player>())
                    {
                        var keepers = Board.GetPlayerKeeperCards(player);
                        if (keepers.Exists(k => k.KeeperCardInfo.KeeperType == keeperAndNotInPlay.KeeperNotInPlay)) {
                            goto breakLoop;
                        }
                    }
                    foreach (var player in EnumUtil.GetArrayOf<Player>())
                    {
                        var keepers = Board.GetPlayerKeeperCards(player);
                        if (keepers.Exists(k => k.KeeperCardInfo.KeeperType == keeperAndNotInPlay.KeeperInPlay)) {
                            playersMatchingGoals.Add(player);
                            break;
                        }
                    }
                    breakLoop: break;
            }
        }
        if (playersMatchingGoals.Count == 1)
        {
            return playersMatchingGoals.First();
        }
        return null;
    }
}
