using System.Collections;
using System.Collections.Generic;

public class ZapACardState : State
{
    readonly List<NewRuleCard> rulesThatCouldNotBeSelected = new();
    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        var newRuleCards = gameStateMachine.Board.GetNewRuleCards();
        var cardsInPlayCount = newRuleCards.Count;
        foreach (var player in EnumUtil.GetArrayOf<GameStateMachine.Player>())
        {
            var keeperCards = gameStateMachine.Board.GetPlayerKeeperCards(player);
            foreach (var card in keeperCards)
            {
                card.SetCanBeSelected(true);
            }
            cardsInPlayCount += keeperCards.Count;
        }
        if (cardsInPlayCount == 0)
        {
            gameStateMachine.PopState();
            yield break;
        }
        rulesThatCouldNotBeSelected.AddRange(newRuleCards.FindAll(r => !r.CanBeSelected));
        foreach (var rule in newRuleCards)
        {
            rule.SetCanBeSelected(true);
        }
        foreach (var card in gameStateMachine.Board.GetPlayerHandCards(gameStateMachine.CurrentPlayer))
        {
            card.SetCanBeSelected(false);
        }
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        foreach (var rule in rulesThatCouldNotBeSelected)
        {
            rule.SetCanBeSelected(false);
        }
        foreach (var player in EnumUtil.GetArrayOf<GameStateMachine.Player>())
        {
            var keeperCards = gameStateMachine.Board.GetPlayerKeeperCards(player);
            foreach (var card in keeperCards)
            {
                card.SetCanBeSelected(false);
            }
        }
        rulesThatCouldNotBeSelected.Clear();
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        if (card is KeeperCard keeperCard)
        {
            foreach (var player in EnumUtil.GetArrayOf<GameStateMachine.Player>())
            {
                if (gameStateMachine.Board.GetPlayerKeeperCards(player).Remove(keeperCard))
                {
                    gameStateMachine.Board.RearrangePlayerKeepers(player);
                    gameStateMachine.Board.AddHandCardTo(gameStateMachine.CurrentPlayer, keeperCard);
                    gameStateMachine.PopState();
                    yield break;
                }
            }
        } else if (card is NewRuleCard ruleCard)
        {
            if (gameStateMachine.Board.GetNewRuleCards().Remove(ruleCard))
            {
                rulesThatCouldNotBeSelected.Remove(ruleCard);
                var state = gameStateMachine.RemoveNewRuleEffect(ruleCard);
                gameStateMachine.Board.RearrangeNewRules();
                gameStateMachine.Board.AddHandCardTo(gameStateMachine.CurrentPlayer, ruleCard);
                if (state == null)
                {
                    gameStateMachine.PopState();
                } else
                {
                    gameStateMachine.SetState(state);
                }
                yield break;
            }
        }
        yield break;
    }
    
}
