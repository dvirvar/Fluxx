using System.Collections;
using System.Collections.Generic;

public class RecyclingState : State
{
    readonly List<NewRuleCard> rulesThatCouldBeSelected = new();

    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        var keepers = gameStateMachine.Board.GetPlayerKeeperCards(gameStateMachine.CurrentPlayer);
        if (keepers.Count == 0)
        {
            gameStateMachine.PopState();
            yield break;
        }
        rulesThatCouldBeSelected.AddRange(gameStateMachine.Board.GetNewRuleCards().FindAll(r => r.CanBeSelected));
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(false);
        }
        foreach (var keeper in keepers)
        {
            keeper.SetCanBeSelected(true);
        }
        var handCards = gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer);
        foreach (var card in handCards)
        {
            card.SetCanBeSelected(false);
        }
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        var keepers = gameStateMachine.Board.GetPlayerKeeperCards(gameStateMachine.CurrentPlayer);
        foreach (var keeper in keepers)
        {
            keeper.SetCanBeSelected(false);
        }
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(true);
        }
        rulesThatCouldBeSelected.Clear();
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        if (card is KeeperCard keeperCard && gameStateMachine.Board.GetPlayerKeeperCards(gameStateMachine.CurrentPlayer).Remove(keeperCard))
        {
            gameStateMachine.Board.AddToDiscardPile(keeperCard);
            keeperCard.SetCanBeSelected(false);
            var draw = gameStateMachine.Inflation ? 4 : 3;
            for (var i = 0; i < draw; ++i)
            {
                var drawedCard = gameStateMachine.Board.DrawCard();
                if (drawedCard != null)
                {
                    gameStateMachine.Board.AddHandCardTo(gameStateMachine.CurrentPlayer, drawedCard);
                }
            }
            gameStateMachine.PopState();
        }
        yield break;
    }
}
