using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] Transform deck, discardPile, newRules, currentGoal, secondCurrentGoal;
    [SerializeField] Transform player1Keepers, player2Keepers, player1Hand, player2Hand;
    Stack<Card> deckCards = new();
    readonly Stack<Card> discardPileCards = new();
    readonly List<NewRuleCard> newRuleCards = new();
    GoalCard currentGoalCard, secondCurrentGoalCard;
    readonly List<KeeperCard> player1KeeperCards = new(), player2KeeperCards = new();
    readonly List<Card> player1HandCards = new(), player2HandCards = new();

    readonly float spaceBetweenDeckCards = 0.01f;

    readonly float xSpaceBetweenRuleCards = 3f;
    readonly float ySpaceBetweenRuleCards = 4.5f;
    readonly int newRuleCardsPerRow = 4;

    readonly float xSpaceBetweenKeepers = 3f;

    readonly float xSpaceBetweenHandCards = 3f;
    readonly float ySpaceBetweenHandCards = 4.5f;
    readonly int handCardsPerRow = 13;

    public void SetDeck(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            card.transform.SetParent(deck, false);
            card.transform.SetLocalPositionAndRotation(new Vector3(0, 0, i * -spaceBetweenDeckCards), Quaternion.Euler(0,180,0));
        }
        deckCards = new Stack<Card>(cards);
    }

    public Card DrawCard()
    {
        if (deckCards.Count == 0 && discardPileCards.Count == 0)
        {
            return null;
        }
        var card = deckCards.Pop();
        if (deckCards.Count == 0)
        {
            var newDeck = discardPileCards.ToList();
            newDeck.Shuffle();
            SetDeck(newDeck);
            discardPileCards.Clear();
        }
        return card;
    }

    public void AddToDiscardPile(Card card)
    {
        card.transform.SetParent(discardPile, false);
        card.transform.SetLocalPositionAndRotation(new Vector3(0, 0, discardPileCards.Count * -spaceBetweenDeckCards), Quaternion.identity);
        discardPileCards.Push(card);
    }

    public void SetCurrentGoal(GoalCard card)
    {
        SetGoalTo(ref currentGoalCard, currentGoal, card);
    }

    public void SetSecondCurrentGoal(GoalCard card)
    {
        SetGoalTo(ref secondCurrentGoalCard, secondCurrentGoal, card);
    }

    private void SetGoalTo(ref GoalCard currentGoalCard, Transform goal, GoalCard goalCard)
    {
        if (currentGoalCard != null)
        {
            AddToDiscardPile(currentGoalCard);
        }
        currentGoalCard = goalCard;
        goalCard.transform.SetParent(goal, false);
        goalCard.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void DiscardSecondCurrentGoal()
    {
        if (secondCurrentGoalCard != null)
        {
            AddToDiscardPile(secondCurrentGoalCard);
            secondCurrentGoalCard = null;
        }
    }

    public void AddNewRule(NewRuleCard newRuleCard)
    {
        var newRuleCardIndex = newRuleCards.Count;
        if (newRuleCard.NewRuleCardInfo.NewRuleType.GetRuleType().IsReplacingRule())
        {
            var currentRuleIndex = newRuleCards.FindIndex(nr => nr.NewRuleCardInfo.NewRuleType.GetRuleType() == newRuleCard.NewRuleCardInfo.NewRuleType.GetRuleType());
            if (currentRuleIndex >= 0)
            {
                newRuleCardIndex = currentRuleIndex;
                AddToDiscardPile(newRuleCards[currentRuleIndex]);
                newRuleCards.RemoveAt(currentRuleIndex);
            }
        }

        newRuleCards.Insert(newRuleCardIndex, newRuleCard);

        newRuleCard.transform.SetParent(newRules, false);
        newRuleCard.transform.SetLocalPositionAndRotation(new Vector3((newRuleCardIndex % newRuleCardsPerRow) * xSpaceBetweenRuleCards, (newRuleCardIndex / newRuleCardsPerRow) * -ySpaceBetweenRuleCards, 0), Quaternion.identity);
    }

    public void DiscardNewRules(List<NewRuleCard> newRuleCards)
    {
        for (int i = this.newRuleCards.Count - 1; i >= 0; --i)
        {
            var card = this.newRuleCards[i];
            if (newRuleCards.Contains(card))
            {
                this.newRuleCards.RemoveAt(i);
                AddToDiscardPile(card);
            }
        }
    }

    public void DiscardAllNewRules()
    {
        for (int i = newRuleCards.Count - 1; i >= 0; --i)
        {
            var card = newRuleCards[i];
            newRuleCards.RemoveAt(i);
            AddToDiscardPile(card);
        }
    }

    public void AddKeeperToPlayer1(KeeperCard keeperCard)
    {
        AddKeeperTo(player1KeeperCards, player1Keepers, keeperCard);
    }

    public void AddKeeperToPlayer2(KeeperCard keeperCard)
    {
        AddKeeperTo(player2KeeperCards, player2Keepers, keeperCard);
    }

    void AddKeeperTo(List<KeeperCard> keeperCards, Transform keepers, KeeperCard keeperCard)
    {
        keeperCard.transform.SetParent(keepers, false);
        keeperCard.transform.SetLocalPositionAndRotation(new Vector3(keeperCards.Count * xSpaceBetweenKeepers, 0), Quaternion.identity);
        keeperCards.Add(keeperCard);
    }

    public void AddCardToPlayer1Hand(Card card)
    {
        AddCardToPlayerHand(player1HandCards, player1Hand, card);
    }

    public void AddCardToPlayer2Hand(Card card)
    {
        AddCardToPlayerHand(player2HandCards, player2Hand, card);
    }

    void AddCardToPlayerHand(List<Card> handCards, Transform hand, Card card)
    {
        card.transform.SetParent(hand, false);
        card.transform.SetLocalPositionAndRotation(new Vector3((handCards.Count % handCardsPerRow) * xSpaceBetweenHandCards, (handCards.Count / handCardsPerRow) * -ySpaceBetweenHandCards, 0), Quaternion.identity);
        handCards.Add(card);
    }

    public void RearrangePlayer1Hand()
    {
        RearrangePlayerHand(player1HandCards);
    }

    public void RearrangePlayer2Hand()
    {
        RearrangePlayerHand(player2HandCards);
    }

    void RearrangePlayerHand(List<Card> handCards)
    {
        for (int i = 0; i < handCards.Count; ++i)
        {
            var card = handCards[i];
            card.transform.localPosition = new Vector3((i % handCardsPerRow) * xSpaceBetweenHandCards, (i / handCardsPerRow) * -ySpaceBetweenHandCards, 0);
        }
    }

    public void ShowPlayer1Hand(bool show)
    {
        ShowPlayerHand(show, player1HandCards);
    }

    public void ShowPlayer2Hand(bool show)
    {
        ShowPlayerHand(show, player2HandCards);
    }

    void ShowPlayerHand(bool show, List<Card> handCards)
    {
        foreach (var card in handCards)
        {
            card.transform.localEulerAngles = new Vector3(0, show ? 0 : 180, 0);
            card.SetCanBeSelected(show);
        }
    }

    public List<NewRuleCard> GetNewRuleCards() => newRuleCards;
    public GoalCard GetCurrentGoalCard() => currentGoalCard;
    public GoalCard GetSecondCurrentGoalCard() => secondCurrentGoalCard;
    public List<KeeperCard> GetPlayer1KeeperCards() => player1KeeperCards;
    public List<KeeperCard> GetPlayer2KeeperCards() => player2KeeperCards;
    public List<Card> GetPlayer1HandCards() => player1HandCards;
    public List<Card> GetPlayer2HandCards() => player2HandCards;
}
