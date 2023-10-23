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
    public GameUI GameUI {  get; private set; }
    InputManager inputManager;
    new Camera camera;
    Transform cardsHolder;
    public Board Board { get; private set; }
    readonly List<State> states = new() { State.NULL };
    int currentStateIndex = 0;

    public Player CurrentPlayer {
        get { return _currentPlayer; }
        private set { 
            _currentPlayer = value;
            GameUI.SetCurrentPlayer(value == Player.Player1 ? "Player 1" : "Player 2");
        }
    }
    Player _currentPlayer;

    public int Played {
        get { return _played; }
        private set { 
            _played = value;
            GameUI.SetPlayed(value.ToString());
        } 
    }
    int _played = 0;
    public int Drawed { get; private set; } = 0;
    
    public int CurrentPlays
    {
        get
        {
            var plays = CurrentPlayRule switch
            {
                NewRuleCardType.Play2 => Inflation ? 3 : 2,
                NewRuleCardType.Play3 => Inflation ? 4 : 3,
                NewRuleCardType.Play4 => Inflation ? 5 : 4,
                NewRuleCardType.PlayAllBut1 => Board.GetPlayerHandCards(CurrentPlayer).Count - (Inflation ? 2 : 1) + Played,
                NewRuleCardType.PlayAll => Board.GetPlayerHandCards(CurrentPlayer).Count + Played,
                null => Inflation ? 2 : 1,
                _ => throw new NotImplementedException(),
            };
            if (PartyBonus)
            {
                foreach (var player in EnumUtil.GetArrayOf<Player>())
                {
                    if (Board.GetPlayerKeeperCards(player).Exists(k => k.KeeperCardInfo.KeeperType == KeeperCardType.TheParty))
                    {
                        plays += Inflation ? 2 : 1;
                        break;
                    }
                }
            }
            if (RichBonus)
            {
                var keepersCount = new List<int>();
                var players = EnumUtil.GetArrayOf<Player>();
                foreach (var player in players)
                {
                    keepersCount.Add(Board.GetPlayerKeeperCards(player).Count);
                }
                var maxKeepers = keepersCount.Max();
                if (keepersCount.Count(c=> c == maxKeepers) == 1 && players[keepersCount.IndexOf(maxKeepers)] == CurrentPlayer)
                {
                    plays += Inflation ? 2 : 1;
                }
            }
            return plays;
        }
    }
    public int CurrentDraws
    {
        get
        {
            var draws = CurrentDrawRule switch
            {
                NewRuleCardType.Draw2 => Inflation ? 3 : 2,
                NewRuleCardType.Draw3 => Inflation ? 4 : 3,
                NewRuleCardType.Draw4 => Inflation ? 5 : 4,
                NewRuleCardType.Draw5 => Inflation ? 6 : 5,
                null => Inflation ? 2 : 1,
                _ => throw new NotImplementedException(),
            };
            if (PartyBonus)
            {
                foreach (var player in EnumUtil.GetArrayOf<Player>())
                {
                    if (Board.GetPlayerKeeperCards(player).Exists(k => k.KeeperCardInfo.KeeperType == KeeperCardType.TheParty))
                    {
                        draws += Inflation ? 2 : 1;
                        break;
                    }
                }
            }
            if (PoorBonus)
            {
                var keepersCount = new List<int>();
                var players = EnumUtil.GetArrayOf<Player>();
                foreach (var player in players)
                {
                    keepersCount.Add(Board.GetPlayerKeeperCards(player).Count);
                }
                var minKeepers = keepersCount.Min();
                if (keepersCount.Count(c => c == minKeepers) == 1 && players[keepersCount.IndexOf(minKeepers)] == CurrentPlayer)
                {
                    draws += Inflation ? 2 : 1;
                }
            }
            return draws;
        }
    }
    public NewRuleCardType? CurrentPlayRule { get; private set; }
    public NewRuleCardType? CurrentDrawRule { get; private set; }
    public NewRuleCardType? CurrentHandLimitRule { get; private set; }
    public NewRuleCardType? CurrentKeeperLimitRule { get; private set; }

    [HideInInspector] public bool Inflation = false;
    [HideInInspector] public bool IsFirstPlayRandom = false;
    [HideInInspector] public bool HasDoubleAgenda = false;
    [HideInInspector] public bool TakeAnotherTurn = false;
    [HideInInspector] public bool NoHandBonus = false;
    [HideInInspector] public bool PartyBonus = false;
    [HideInInspector] public bool RichBonus = false;
    [HideInInspector] public bool PoorBonus = false;
    bool isAnotherTurn = false;

    public void StartGame(GameUI gameUI, InputManager inputManager, Camera camera, Transform cardsHolder, Board board)
    {
        GameUI = gameUI;
        this.inputManager = inputManager;
        this.camera = camera;
        this.cardsHolder = cardsHolder;
        Board = board;
        CurrentPlayer = Player.Player1;
        DrawToMatchDraws();
        SetState(new StartOfPlayState());
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
        yield return states[currentStateIndex].OnPause(this);
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
        yield return states[currentStateIndex].OnResume(this);
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
        if (TakeAnotherTurn && !isAnotherTurn)
        {
            TakeAnotherTurn = false;
            isAnotherTurn = true;
        } else
        {
            isAnotherTurn = false;
            CurrentPlayer = CurrentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
        }
        ResetDrawedAndPlayed();
        SetCameraFacing(CurrentPlayer);
        inputManager.ReturnToCameraOrigin();
    }

    public void SetCameraFacing(Player player)
    {
        var lea = camera.transform.localEulerAngles;
        camera.transform.localEulerAngles = new Vector3(lea.x, player == Player.Player1 ? 0 : 180, lea.z);
        inputManager.SetInverse(player == Player.Player2);
    }

    public void LookAtDiscardPile(bool look)
    {
        inputManager.SetDiscardPileControl(look);
        if (!look)
        {
            inputManager.ReturnToCameraOrigin();
            SetCameraFacing(CurrentPlayer);
        }
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
            CurrentPlayRule = null;
            return;
        }
        Assert.IsTrue(rule.Value.GetRuleType() == RuleType.Play);
        CurrentPlayRule = rule;
    }

    public void SetCurrentDrawRule(NewRuleCardType? rule)
    {
        if (rule == null)
        {
            CurrentDrawRule = null;
            return;
        }
        Assert.IsTrue(rule.Value.GetRuleType() == RuleType.Draw);
        CurrentDrawRule = rule;
    }

    public void SetCurrentHandLimitRule(NewRuleCardType? rule)
    {
        if (rule == null)
        {
            CurrentHandLimitRule = null;
            return;
        }
        Assert.IsTrue(rule.Value.GetRuleType() == RuleType.HandLimit);
        CurrentHandLimitRule = rule;
    }

    public void SetCurrentKeeperLimitRule(NewRuleCardType? rule)
    {
        if (rule == null)
        {
            CurrentKeeperLimitRule = null;
            return;
        }
        Assert.IsTrue(rule.Value.GetRuleType() == RuleType.KeeperLimit);
        CurrentKeeperLimitRule = rule;
    }

    public State DiscardNewRules(List<NewRuleCard> discardedRules)
    {
        State state = null;
        var newRuleCards = Board.GetNewRuleCards();
        foreach (var discardedRule in discardedRules)
        {
            if (newRuleCards.Remove(discardedRule))
            {
                discardedRule.SetCanBeSelected(false);
                var discardedRuleState = DiscardNewRule(discardedRule);
                if (discardedRuleState != null)
                {
                    state = discardedRuleState;
                }
            }
        }
        Board.RearrangeNewRules();
        return state;
    }

    public State DiscardAllNewRules()
    {
        State state = null;
        var newRuleCards = Board.GetNewRuleCards();
        for (int i = newRuleCards.Count - 1; i >= 0; --i)
        {
            var card = newRuleCards[i];
            card.SetCanBeSelected(false);
            newRuleCards.RemoveAt(i);
            var discardedRuleState = DiscardNewRule(card);
            if (discardedRuleState != null)
            {
                state = discardedRuleState;
            }
        }
        Board.RearrangeNewRules();
        return state;
    }

    public State RemoveNewRuleEffect(NewRuleCard ruleCard)
    {
        var ruleType = ruleCard.NewRuleCardInfo.NewRuleType.GetRuleType();
        if (ruleType == RuleType.Draw)
        {
            SetCurrentDrawRule(null);
        }
        else if (ruleType == RuleType.Play)
        {
            SetCurrentPlayRule(null);
        }
        else if (ruleType == RuleType.HandLimit)
        {
            SetCurrentHandLimitRule(null);
        }
        else if (ruleType == RuleType.KeeperLimit)
        {
            SetCurrentKeeperLimitRule(null);
        }
        else if (ruleCard.NewRuleCardInfo.NewRuleType == NewRuleCardType.Inflation)
        {
            Inflation = false;
        }
        else if (ruleCard.NewRuleCardInfo.NewRuleType == NewRuleCardType.FirstPlayRandom)
        {
            IsFirstPlayRandom = false;
        }
        else if (ruleCard.NewRuleCardInfo.NewRuleType == NewRuleCardType.DoubleAgenda)
        {
            HasDoubleAgenda = false;
            return new RemoveDoubleAgendaState();
        }
        else if (ruleCard.NewRuleCardInfo.NewRuleType == NewRuleCardType.NoHandBonus)
        {
            NoHandBonus = false;
        }
        else if (ruleCard.NewRuleCardInfo.NewRuleType == NewRuleCardType.RichBonus)
        {
            RichBonus = false;
        }
        else if (ruleCard.NewRuleCardInfo.NewRuleType == NewRuleCardType.PartyBonus)
        {
            PartyBonus = false;
        }
        else if (ruleCard.NewRuleCardInfo.NewRuleType == NewRuleCardType.PoorBonus)
        {
            PoorBonus = false;
        }
        return null;
    }

    State DiscardNewRule(NewRuleCard ruleCard)
    {
        var state = RemoveNewRuleEffect(ruleCard);
        Board.AddToDiscardPile(ruleCard);
        return state;
    }

    public void SetCardsInfrontOfCamera(List<Card> cards)
    {
        var space = 3;
        var startOfCards = ((cards.Count - 1) * space) / -2f;
        for (int i = 0; i < cards.Count; ++i)
        {
            var card = cards[i];
            card.transform.SetParent(cardsHolder);
            card.transform.SetLocalPositionAndRotation(new Vector3(i * space + startOfCards, 0), Quaternion.identity);
        }
    }

    public void ShowCardsInfrontOfCamera(bool show)
    {
        cardsHolder.gameObject.SetActive(show);
    }

    public PlayerWonState CheckHasPlayerWon()
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
            return new PlayerWonState(playersMatchingGoals.First());
        }
        return null;
    }
}
