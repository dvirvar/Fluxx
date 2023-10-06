using System.Collections;
using System.Collections.Generic;

public class EverybodyGets1State : State
{
    readonly List<Card> cardsToShow = new();
    readonly List<Card> myCards = new();
    readonly List<NewRuleCard> rulesThatCouldBeSelected = new();
    int cardsPerPlayer;
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        var numOfPlayers = EnumUtil.GetArrayOf<GameStateMachine.Player>().Length;
        var toDraw = numOfPlayers;
        if (gameStateMachine.Inflation)
        {
            toDraw *= 2;
        }
        cardsPerPlayer = toDraw / numOfPlayers;
        for (int i = 0; i < toDraw; ++i)
        {
            var card = gameStateMachine.Board.DrawCard();
            if (card != null)
            {
                card.SetCanBeSelected(true);
                cardsToShow.Add(card);
            }
        }
        if (cardsToShow.Count == 0)
        {
            gameStateMachine.PopState();
            yield break;
        }
        rulesThatCouldBeSelected.AddRange(gameStateMachine.Board.GetNewRuleCards().FindAll(r => r.CanBeSelected));
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(false);
        }        
        gameStateMachine.SetCardsInfrontOfCamera(cardsToShow);
        gameStateMachine.ShowCardsInfrontOfCamera(true);
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(gameStateMachine.CurrentPlayer, false, false);
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(true);
        }
        cardsToShow.Clear();
        myCards.Clear();
        rulesThatCouldBeSelected.Clear();
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(gameStateMachine.CurrentPlayer, true, true);
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        if (cardsToShow.Remove(card))
        {
            card.SetCanBeSelected(false);
            myCards.Add(card);
            if (myCards.Count == cardsPerPlayer || cardsToShow.Count == 0)
            {
                foreach (var myCard in myCards)
                {
                    gameStateMachine.Board.AddHandCardTo(gameStateMachine.CurrentPlayer, myCard);
                }
                var otherPlayer = gameStateMachine.CurrentPlayer == GameStateMachine.Player.Player1 ? GameStateMachine.Player.Player2 : GameStateMachine.Player.Player1;
                foreach (var otherCard in cardsToShow)
                {
                    gameStateMachine.Board.AddHandCardTo(otherPlayer, otherCard);
                }
                gameStateMachine.PopState();
            }
        }
        yield break;
    }
}
