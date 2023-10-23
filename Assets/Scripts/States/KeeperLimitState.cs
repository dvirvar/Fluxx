using System.Collections;
using System.Collections.Generic;

public class KeeperLimitState : State
{
    int numberToKeep;
    readonly List<KeeperCard> keepersToKeep = new();
    readonly List<NewRuleCard> rulesThatCouldBeSelected = new();
    readonly GameStateMachine.Player player, otherPlayer;

    public KeeperLimitState(GameStateMachine.Player player)
    {
        this.player = player;
        this.otherPlayer = player == GameStateMachine.Player.Player1 ? GameStateMachine.Player.Player2 : GameStateMachine.Player.Player1;
    }

    public override IEnumerator OnEnter(GameStateMachine gameStateMachine)
    {
        numberToKeep = gameStateMachine.CurrentKeeperLimitRule switch
        {
            NewRuleCardType.KeeperLimit2=> 2,
            NewRuleCardType.KeeperLimit3 => 3,
            NewRuleCardType.KeeperLimit4 => 4,
            _ => throw new System.NotImplementedException(),
        };
        if (gameStateMachine.Inflation)
        {
            ++numberToKeep;
        }
        var keeperCards = gameStateMachine.Board.GetPlayerKeeperCards(player);
        if (keeperCards.Count <= numberToKeep)
        {
            gameStateMachine.PopState();
            yield break;
        }
        rulesThatCouldBeSelected.AddRange(gameStateMachine.Board.GetNewRuleCards().FindAll(r => r.CanBeSelected));
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(false);
        }
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(player, false, false);
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(otherPlayer, false, false);
        gameStateMachine.SetCameraFacing(player);
        gameStateMachine.GameUI.LimitUI.SetText(player == GameStateMachine.Player.Player1 ? "Player 1" : "Player 2");
        gameStateMachine.GameUI.LimitUI.gameObject.SetActive(true);
        foreach (var keeper in gameStateMachine.Board.GetPlayerKeeperCards(player))
        {
            keeper.SetCanBeSelected(true);
        }
        yield break;
    }

    public override IEnumerator OnResume(GameStateMachine gameStateMachine)
    {
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(player, false, false);
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(otherPlayer, false, false);
        gameStateMachine.SetCameraFacing(player);
        gameStateMachine.GameUI.LimitUI.SetText(player == GameStateMachine.Player.Player1 ? "Player 1" : "Player 2");
        gameStateMachine.GameUI.LimitUI.gameObject.SetActive(true);
        foreach (var keeper in gameStateMachine.Board.GetPlayerKeeperCards(player))
        {
            keeper.SetCanBeSelected(true);
        }
        yield break;
    }

    public override IEnumerator OnExit(GameStateMachine gameStateMachine)
    {
        foreach (var rule in rulesThatCouldBeSelected)
        {
            rule.SetCanBeSelected(true);
        }
        keepersToKeep.Clear();
        rulesThatCouldBeSelected.Clear();
        gameStateMachine.Board.ShowAndCanBeSelectedPlayerHand(player, false, false);
        gameStateMachine.SetCameraFacing(gameStateMachine.CurrentPlayer);
        gameStateMachine.GameUI.LimitUI.gameObject.SetActive(false);
        foreach (var keeper in gameStateMachine.Board.GetPlayerKeeperCards(player))
        {
            keeper.SetCanBeSelected(false);
        }
        yield break;
    }

    public override IEnumerator Play(GameStateMachine gameStateMachine, Card card)
    {
        var keeperCards = gameStateMachine.Board.GetPlayerKeeperCards(player);
        if (card is KeeperCard keeperCard && keeperCards.Remove(keeperCard))
        {
            card.SetCanBeSelected(false);
            keepersToKeep.Add(keeperCard);
            if (keepersToKeep.Count >= numberToKeep)
            {
                for (int i = keeperCards.Count - 1; i >= 0; --i)
                {
                    var keeperCardToDiscard = keeperCards[i];
                    keeperCards.RemoveAt(i);
                    keeperCardToDiscard.SetCanBeSelected(false);
                    gameStateMachine.Board.AddToDiscardPile(keeperCardToDiscard);
                }
                foreach (var keeperToKeep in keepersToKeep)
                {
                    gameStateMachine.Board.AddKeeperTo(player, keeperToKeep);
                }
                gameStateMachine.PopState();
            }
        }
        yield break;
    }
}
