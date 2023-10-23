using System.Collections;
using System.Collections.Generic;

public class HandLimitState : State
{
    int numberToKeep;
    readonly List<Card> cardsToKeep = new();
    readonly List<NewRuleCard> rulesThatCouldBeSelected = new();
    readonly GameStateMachine.Player player, otherPlayer;

    public HandLimitState(GameStateMachine.Player player)
    {
        this.player = player;
        this.otherPlayer = player == GameStateMachine.Player.Player1 ? GameStateMachine.Player.Player2 : GameStateMachine.Player.Player1;
    }

    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        numberToKeep = gameStateMachine.CurrentHandLimitRule switch
        {
            NewRuleCardType.HandLimit0 => 0,
            NewRuleCardType.HandLimit1 => 1,
            NewRuleCardType.HandLimit2 => 2,
            _ => throw new System.NotImplementedException(),
        };
        if (gameStateMachine.Inflation)
        {
            ++numberToKeep;
        }
        var handCards = gameStateMachine.Board.GetPlayerHandCards(player);
        if (handCards.Count <= numberToKeep)
        {
            gameStateMachine.PopState();
            yield break;
        }
        if (numberToKeep == 0)
        {
            for (int i = handCards.Count - 1; i >= 0; --i) { 
                var card = handCards[i];
                handCards.RemoveAt(i);
                card.SetCanBeSelected(false);
                gameStateMachine.Board.AddToDiscardPile(card);
            }
            gameStateMachine.PopState();
            yield break;
        }
        rulesThatCouldBeSelected.AddRange(gameStateMachine.Board.GetNewRuleCards().FindAll(r => r.CanBeSelected));
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(false);
        }
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(player, true, true);
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(otherPlayer, false, false);
        gameStateMachine.SetCameraFacing(player);
        gameStateMachine.GameUI.LimitUI.SetText(player == GameStateMachine.Player.Player1 ? "Player 1" : "Player 2");
        gameStateMachine.GameUI.LimitUI.gameObject.SetActive(true);
        yield break;
    }

    public override IEnumerator OnResume(GameStateMachine gameStateMachine)
    {
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(player, true, true);
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(otherPlayer, false, false);
        gameStateMachine.SetCameraFacing(player);
        gameStateMachine.GameUI.LimitUI.SetText(player == GameStateMachine.Player.Player1 ? "Player 1" : "Player 2");
        gameStateMachine.GameUI.LimitUI.gameObject.SetActive(true);
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(true);
        }
        cardsToKeep.Clear();
        rulesThatCouldBeSelected.Clear();
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(player, false, false);
        gameStateMachine.SetCameraFacing(gameStateMachine.CurrentPlayer);
        gameStateMachine.GameUI.LimitUI.gameObject.SetActive(false);
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        var handCards = gameStateMachine.Board.GetPlayerHandCards(player);
        if (handCards.Remove(card))
        {
            card.SetCanBeSelected(false);
            cardsToKeep.Add(card);
            if (cardsToKeep.Count >= numberToKeep)
            {
                for (int i = handCards.Count - 1; i >= 0; --i)
                {
                    var handCard = handCards[i];
                    handCards.RemoveAt(i);
                    handCard.SetCanBeSelected(false);
                    gameStateMachine.Board.AddToDiscardPile(handCard);
                }
                foreach (var cardToKeep in cardsToKeep)
                {
                    gameStateMachine.Board.AddHandCardTo(player, cardToKeep);
                }
                gameStateMachine.PopState();
            }
        }
        yield break;
    }
}
