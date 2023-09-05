using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public static class Extensions
{

    public static string GetName(this ActionCardType type) => type switch
    {
        ActionCardType.Draw3AndPlay2 => "Draw 3,\nPlay 2 of Them",
        ActionCardType.EverybodyGets1 => "Everybody gets 1",
        ActionCardType.RotateHands => "Rotate Hands",
        ActionCardType.StealAKeeper => "Steal a Keeper",
        ActionCardType.LetsSimplify => "Let's Simplify",
        ActionCardType.Jackpot => "Jackpot!",
        ActionCardType.TrashAKeeper => "Trash a Keeper",
        ActionCardType.TodaysSpecial => "Today's Special!",
        ActionCardType.Draw2AndUseThem => "Draw 2 and Use 'Em",
        ActionCardType.TakeAnotherTurn => "Take Another Turn",
        ActionCardType.RulesReset => "Rules Reset",
        ActionCardType.NoLimits => "No Limits",
        ActionCardType.TrashANewRule => "Trash a New Rule",
        ActionCardType.ZapACard => "Zap a Card",
        ActionCardType.DiscardAndDraw => "Discard and Draw",
        ActionCardType.LetsDoThatAgain => "Let's Do That Again!",
        ActionCardType.UseWhatYouTake => "Use What You Take",
        ActionCardType.EmptyTheTrash => "Empty the Trash",
        ActionCardType.RandomTax => "Random Tax",
        ActionCardType.TradeHands => "Trade Hands",
        ActionCardType.RockPaperScissorsShowdown => "Rock-Paper-Scissors Showdown",
        ActionCardType.ExchangeKeepers => "Exchange Keepers",
        ActionCardType.ShareTheWealth => "Share the Wealth",
        _ => throw new NotImplementedException(),
    };

    public static string GetDescription(this ActionCardType type) => type switch
    {
        ActionCardType.Draw3AndPlay2 => "Set your hand aside.\n\nDraw 3 cards and play 2 of them. Discard the last card, then pick up your hand and continue your turn.\n\nThis card, and all cards played because of it, are counted as single play.",
        ActionCardType.EverybodyGets1 => "Set your hand aside.\n\nCount the number of players in the game (including yourself). Draw enough cards to give 1 card to each player, and then distribute them evenly amongst all the players.\n\nYou look at the cards and decide who gets what, handing the cards out face down to each player.",
        ActionCardType.RotateHands => "All players pass their hands to the player next to them.\n\nYou decide which direction.",
        ActionCardType.StealAKeeper => "Steal a Keeper from in front of another player, and add it to your collection of Keepers on the table.",
        ActionCardType.LetsSimplify => "Discard your choice of up to half (rounded up) of the New Rule cards in play.",
        ActionCardType.Jackpot => "Draw 3 extra cards!",
        ActionCardType.TrashAKeeper => "Take a Keeper from in front of any player and put it on the discard pile.\n\nIf no one has any Keepers in play nothing happens when you play this card.",
        ActionCardType.TodaysSpecial => "Set your hand aside and draw 3 cards. If today is your birthday, play all 3 cards. If today is a holiday or special anniversary, play 2 of the cards. If it's just another day, play only 1 card. Discard the remainder.",
        ActionCardType.Draw2AndUseThem => "Set your hand aside.\n\nDraw 2 cards, play them in any order you choose, then pick up your hand and continue with your turn.\n\nThis card, and all cards played because of it, are counted as a single play.",
        ActionCardType.TakeAnotherTurn => "Take another turn as soon as you finish this one.\n\nThe maximum number of turns you can take in a row using this card is two.",
        ActionCardType.RulesReset => "Reset to the Basic Rules.\n\nDiscard all New Rule cards, and leave only the Basic Rules in play.\n\nDo not discard the current Goal.",
        ActionCardType.NoLimits => "Discard all Hand and Keeper Limits currently in play.",
        ActionCardType.TrashANewRule => "Select one of the New Rule cards in play and place it in the discard pile.",
        ActionCardType.ZapACard => "Choose any card in play, anywhere on the table (except for the Basic Rules) and add it to your hand.",
        ActionCardType.DiscardAndDraw => "Discard your entire hand then draw as many cards as you discarded.\n\nDo not count this card when determining how many cards to draw.",
        ActionCardType.LetsDoThatAgain => "Search through the discard pile. Take any Action or New Rule card you wish and immediately play it.\n\nAnyone may look through the discard pile at any time, but the order of what's in the pile should never be changed.",
        ActionCardType.UseWhatYouTake => "Take a card at random from another player's hand, and play it.",
        ActionCardType.EmptyTheTrash => "Start a new discard pile with this card and shuffle the rest of the discard pile back into the draw pile.",
        ActionCardType.RandomTax => "Take 1 card at random from the hand of each other player and add these cards to your own hand.",
        ActionCardType.TradeHands => "Trade your hand for the hand of one of your opponents.\n\nThis is one of those times when you can get somthing for nothing!",
        ActionCardType.RockPaperScissorsShowdown => "Challenge another player to a 3-round Rock-Paper-Scissors tournament.\n\nWinner takes loser's entire hands of cards.",
        ActionCardType.ExchangeKeepers => "Pick any Keeper another player has on the table and exchange it for one you have on the table.\n\nIf you have no Keepers in play, or if no one else has a Keeper, nothing happens.",
        ActionCardType.ShareTheWealth => "Gather up all Keepers on the table, shuffle them together, and deal them back out to all players, starting with yourself. These go immediately into play in front of their new owners.\n\nEveryone will probably end up with a different number of Keepers in play than they started with.",
        _ => throw new NotImplementedException(),
    };

    public static string GetName(this GoalCardType type) => type switch
    {
        GoalCardType.BreadAndChocolate => "Bread & Chocolate",
        GoalCardType.GreatThemeSong => "Great Theme Song",
        GoalCardType.TheAppliances => "The Appliances",
        GoalCardType.SquishyChocolate => "Squishy Chocolate",
        GoalCardType.Toast => "Toast",
        GoalCardType.CantBuyMeLove => "Can't Buy Me Love",
        GoalCardType.Lullaby => "Lullaby",
        GoalCardType.BedTime => "Bed Time",
        GoalCardType.TimeIsMoney => "Time Is Money",
        GoalCardType.PartyTime => "Party Time!",
        GoalCardType.HeartsAndMinds => "Hearts & Minds",
        GoalCardType.WorldPeace => "World Peace",
        GoalCardType.TurnItUp => "Turn It Up!",
        GoalCardType.DayDreams => "Day Dreams",
        GoalCardType.RocketScience => "Rocket Science",
        GoalCardType.TheMindsEye => "The Mind's Eye",
        GoalCardType.Dreamland => "Dreamland",
        GoalCardType.WinningTheLottery => "Winning the Lottery",
        GoalCardType.ChocolateCookies => "Chocolate Cookies",
        GoalCardType.ChocolateMilk => "Chocolate Milk",
        GoalCardType.Hippyism => "Hippyism",
        GoalCardType.TheEyeOfTheBeholder => "The Eye of the Beholder",
        GoalCardType.MilkAndCookies => "Milk & Cookies",
        GoalCardType.NightAndDay => "Night & Day",
        GoalCardType.BakedGoods => "Baked Goods",
        GoalCardType.RocketToTheMoon => "Rocket to the Moon",
        GoalCardType.TenCardsInHand => "10 Cards in Hand",
        GoalCardType.TheBrain => "The Brain\n(No TV)",
        GoalCardType.FiveKeepers => "5 Keepers",
        GoalCardType.PartySnacks => "Party Snacks",
        _ => throw new NotImplementedException(),
    };
    
    public static string GetDescription(this GoalCardType type) => type.GetGoalType() switch
    {
        GoalType.Keepers => string.Join(" + ", type.GetKeepersToAchieveGoal().Select(keeper => keeper.GetName())),
        GoalType.NumberOfKeepersInPlay => $"If someone has {type.GetNumberOfKeepersToAcheiveGoal()} or more Keepers on the table, then the player with the most Keepers in play wins.\n\nInthe even of a tie, continue playing until a clear winner emerges.",
        GoalType.NumberOfCardsInHand => $"If someone has {type.GetNumberOfCardsInHandAcheiveGoal()} or more cards in his or her hand, then the player with the most cards in hand wins.\n\nIn the event of a tie continue playing until a clear winner emerges.",
        GoalType.KeeperAndNotInPlay => $"If no one has {type.GetKeeperAndNotInPlayToAcheiveGoal().KeeperNotInPlay.GetName()} on the table, the player with {type.GetKeeperAndNotInPlayToAcheiveGoal().KeeperInPlay.GetName()} on the table wins.",
        GoalType.KeeperAndAtLeastOneFood => $"{type.GetKeeperForAtLeastOneFoodToAcheiveGoal().GetName()} + at least 1 food Keeper",
        _ => throw new NotImplementedException(),
    };

    public static GoalType GetGoalType(this GoalCardType type) => type switch
    {
        GoalCardType.BreadAndChocolate or
        GoalCardType.GreatThemeSong or
        GoalCardType.TheAppliances or
        GoalCardType.SquishyChocolate or
        GoalCardType.Toast or
        GoalCardType.CantBuyMeLove or
        GoalCardType.Lullaby or
        GoalCardType.BedTime or
        GoalCardType.TimeIsMoney or
        GoalCardType.PartyTime or
        GoalCardType.HeartsAndMinds or
        GoalCardType.WorldPeace or
        GoalCardType.TurnItUp or
        GoalCardType.DayDreams or
        GoalCardType.RocketScience or
        GoalCardType.TheMindsEye or
        GoalCardType.Dreamland or
        GoalCardType.WinningTheLottery or
        GoalCardType.ChocolateCookies or
        GoalCardType.ChocolateMilk or
        GoalCardType.Hippyism or
        GoalCardType.TheEyeOfTheBeholder or
        GoalCardType.MilkAndCookies or
        GoalCardType.NightAndDay or
        GoalCardType.BakedGoods or
        GoalCardType.RocketToTheMoon
        => GoalType.Keepers,
        GoalCardType.TenCardsInHand => GoalType.NumberOfCardsInHand,
        GoalCardType.TheBrain => GoalType.KeeperAndNotInPlay,
        GoalCardType.FiveKeepers => GoalType.NumberOfKeepersInPlay,
        GoalCardType.PartySnacks => GoalType.KeeperAndAtLeastOneFood,
        _ => throw new NotImplementedException(),
    };

    public static KeeperCardType[] GetKeepersToAchieveGoal(this GoalCardType type) => type switch
    {
        GoalCardType.BreadAndChocolate => new KeeperCardType[2] { KeeperCardType.Bread, KeeperCardType.Chocolate },
        GoalCardType.GreatThemeSong => new KeeperCardType[2] { KeeperCardType.Music, KeeperCardType.Television },
        GoalCardType.TheAppliances => new KeeperCardType[2] { KeeperCardType.TheToaster, KeeperCardType.Television },
        GoalCardType.SquishyChocolate => new KeeperCardType[2] { KeeperCardType.Chocolate, KeeperCardType.TheSun },
        GoalCardType.Toast => new KeeperCardType[2] { KeeperCardType.Bread, KeeperCardType.TheToaster },
        GoalCardType.CantBuyMeLove => new KeeperCardType[2] { KeeperCardType.Money, KeeperCardType.Love },
        GoalCardType.Lullaby => new KeeperCardType[2] { KeeperCardType.Sleep, KeeperCardType.Music },
        GoalCardType.BedTime => new KeeperCardType[2] { KeeperCardType.Sleep, KeeperCardType.Time },
        GoalCardType.TimeIsMoney => new KeeperCardType[2] { KeeperCardType.Time, KeeperCardType.Money },
        GoalCardType.PartyTime => new KeeperCardType[2] { KeeperCardType.TheParty, KeeperCardType.Time },
        GoalCardType.HeartsAndMinds => new KeeperCardType[2] { KeeperCardType.Love, KeeperCardType.TheBrain },
        GoalCardType.WorldPeace => new KeeperCardType[2] { KeeperCardType.Dreams, KeeperCardType.Peace },
        GoalCardType.TurnItUp => new KeeperCardType[2] { KeeperCardType.Music, KeeperCardType.TheParty },
        GoalCardType.DayDreams => new KeeperCardType[2] { KeeperCardType.TheSun, KeeperCardType.Dreams },
        GoalCardType.RocketScience => new KeeperCardType[2] { KeeperCardType.TheRocket, KeeperCardType.TheBrain },
        GoalCardType.TheMindsEye => new KeeperCardType[2] { KeeperCardType.TheBrain, KeeperCardType.TheEye},
        GoalCardType.Dreamland => new KeeperCardType[2] { KeeperCardType.Sleep, KeeperCardType.Dreams },
        GoalCardType.WinningTheLottery => new KeeperCardType[2] { KeeperCardType.Dreams, KeeperCardType.Money },
        GoalCardType.ChocolateCookies => new KeeperCardType[2] { KeeperCardType.Chocolate, KeeperCardType.Cookies },
        GoalCardType.ChocolateMilk => new KeeperCardType[2] { KeeperCardType.Chocolate, KeeperCardType.Milk },
        GoalCardType.Hippyism => new KeeperCardType[2] { KeeperCardType.Peace, KeeperCardType.Love },
        GoalCardType.TheEyeOfTheBeholder => new KeeperCardType[2] { KeeperCardType.TheEye, KeeperCardType.Love },
        GoalCardType.MilkAndCookies => new KeeperCardType[2] { KeeperCardType.Milk, KeeperCardType.Cookies },
        GoalCardType.NightAndDay => new KeeperCardType[2] { KeeperCardType.TheSun, KeeperCardType.TheMoon },
        GoalCardType.BakedGoods => new KeeperCardType[2] { KeeperCardType.Bread, KeeperCardType.Cookies },
        GoalCardType.RocketToTheMoon => new KeeperCardType[2] { KeeperCardType.TheRocket, KeeperCardType.TheMoon },
        _ => throw new NotImplementedException()
    };
    public static int GetNumberOfCardsInHandAcheiveGoal(this GoalCardType type) => type switch
    {
        GoalCardType.TenCardsInHand=> 10,
        _ => throw new NotImplementedException()
    };
    public static int GetNumberOfKeepersToAcheiveGoal(this GoalCardType type) => type switch
    {
        GoalCardType.FiveKeepers => 5,
        _ => throw new NotImplementedException()
    };
    public static KeeperCardType GetKeeperForAtLeastOneFoodToAcheiveGoal(this GoalCardType type) => type switch
    {
        GoalCardType.PartySnacks => KeeperCardType.TheParty,
        _ => throw new NotImplementedException()
    };
    public static KeeperAndNotInPlay GetKeeperAndNotInPlayToAcheiveGoal(this GoalCardType type) => type switch {
        GoalCardType.TheBrain => new KeeperAndNotInPlay(KeeperCardType.TheBrain, KeeperCardType.Television),
        _ => throw new NotImplementedException()
    };

    public static string GetName(this KeeperCardType type)
    {
        var nameBuilder = new StringBuilder(type.ToString());
        for (var i = 1; i < nameBuilder.Length; ++i)
        {
            if (char.IsUpper(nameBuilder[i]))
            {
                nameBuilder.Insert(i, ' ');
                ++i;
            }
        }
        return nameBuilder.ToString();
    }

    public static string GetName(this NewRuleCardType type) => type switch
    {
        NewRuleCardType.FirstPlayRandom => "First Play Random",
        NewRuleCardType.GetOnWithIt => "Get On With It!",
        NewRuleCardType.HandLimit0 => "Hand Limit 0",
        NewRuleCardType.HandLimit1 => "Hand Limit 1",
        NewRuleCardType.HandLimit2 => "Hand Limit 2",
        NewRuleCardType.KeeperLimit2 => "Keeper Limit 2",
        NewRuleCardType.KeeperLimit3 => "Keeper Limit 3",
        NewRuleCardType.KeeperLimit4 => "Keeper Limit 4",
        NewRuleCardType.Draw2 => "Draw 2",
        NewRuleCardType.Draw3 => "Draw 3",
        NewRuleCardType.Draw4 => "Draw 4",
        NewRuleCardType.Draw5 => "Draw 5",
        NewRuleCardType.Play2 => "Play 2",
        NewRuleCardType.Play3 => "Play 3",
        NewRuleCardType.Play4 => "Play 4",
        NewRuleCardType.PlayAllBut1 => "Play All But 1",
        NewRuleCardType.PlayAll => "Play All",
        NewRuleCardType.Inflation => "Inflation",
        NewRuleCardType.DoubleAgenda => "Double Agenda",
        NewRuleCardType.RichBonus => "Rich Bonus",
        NewRuleCardType.PartyBonus => "Party Bonus",
        NewRuleCardType.PoorBonus => "Poor Bonus",
        NewRuleCardType.NoHandBonus => "No-Hand Bonus",
        NewRuleCardType.Recycling => "Recycling",
        NewRuleCardType.SwapPlaysForDraws => "Swap Plays for Draws",
        NewRuleCardType.GoalMill => "Goal Mill",
        NewRuleCardType.MysteryPlay => "Mystery Play",
        _ => throw new NotImplementedException(),
    };

    public static RuleType GetRuleType(this NewRuleCardType type) => type switch
    {
        NewRuleCardType.FirstPlayRandom or
        NewRuleCardType.Inflation or
        NewRuleCardType.DoubleAgenda or
        NewRuleCardType.RichBonus or
        NewRuleCardType.PartyBonus or
        NewRuleCardType.PoorBonus or
        NewRuleCardType.SwapPlaysForDraws
        => RuleType.InstantEffect,

        NewRuleCardType.GetOnWithIt or
        NewRuleCardType.Recycling or
        NewRuleCardType.GoalMill or
        NewRuleCardType.MysteryPlay
        => RuleType.FreeAction,

        NewRuleCardType.HandLimit0 or
        NewRuleCardType.HandLimit1 or
        NewRuleCardType.HandLimit2
        => RuleType.HandLimit,

        NewRuleCardType.KeeperLimit2 or
        NewRuleCardType.KeeperLimit3 or
        NewRuleCardType.KeeperLimit4
        => RuleType.KeeperLimit,

        NewRuleCardType.Draw2 or
        NewRuleCardType.Draw3 or
        NewRuleCardType.Draw4 or
        NewRuleCardType.Draw5
        => RuleType.Draw,

        NewRuleCardType.Play2 or
        NewRuleCardType.Play3 or
        NewRuleCardType.Play4 or
        NewRuleCardType.PlayAllBut1 or
        NewRuleCardType.PlayAll
        => RuleType.Play,

        NewRuleCardType.NoHandBonus => RuleType.StartOfTurn,
        _ => throw new NotImplementedException(),
    };

    public static string GetDescription(this NewRuleCardType type) => type switch
    {
        NewRuleCardType.FirstPlayRandom => "The first card you play must be chosen at random rom your hand by the player on your left, Ignore this rule if the current Rule cards allow you to play only one card.",
        NewRuleCardType.GetOnWithIt => "Before your final play, if you are not empty handed, you may discard your entire hand and draw 3 cards. Your turn then ends immediately",
        NewRuleCardType.HandLimit0 => "If it isn't your turn, you can only have 0 cards in your hand. Discard extras immediately. During your turn, this rule does not apply to you; after your turn ends, discard down to 0 cards",
        NewRuleCardType.HandLimit1 => "If it isn't your turn, you can only have 1 card in your hand. Discard extras immediately. During your turn, this rule does not apply to you; after your turn ends, discard down to 1 card",
        NewRuleCardType.HandLimit2 => "If it isn't your turn, you can only have 2 cards in your hand. Discard extras immediately. During your turn, this rule does not apply to you; after your turn ends, discard down to 2 cards",
        NewRuleCardType.KeeperLimit2 => "If it isn't your turn, you can only have 2 Keepers in play. Discard extras immediately. You may acquire new Keepers during your turn as long as you discard down to 2 when your turn ends.",
        NewRuleCardType.KeeperLimit3 => "If it isn't your turn, you can only have 3 Keepers in play. Discard extras immediately. You may acquire new Keepers during your turn as long as you discard down to 3 when your turn ends.",
        NewRuleCardType.KeeperLimit4 => "If it isn't your turn, you can only have 4 Keepers in play. Discard extras immediately. You may acquire new Keepers during your turn as long as you discard down to 4 when your turn ends.",
        NewRuleCardType.Draw2 => "Draw 2 cards per turn.\nIf you just played this card, draw extra cards as needed to reach 2 cards drawn.",
        NewRuleCardType.Draw3 => "Draw 3 cards per turn.\nIf you just played this card, draw extra cards as needed to reach 3 cards drawn.",
        NewRuleCardType.Draw4 => "Draw 4 cards per turn.\nIf you just played this card, draw extra cards as needed to reach 4 cards drawn.",
        NewRuleCardType.Draw5 => "Draw 5 cards per turn.\nIf you just played this card, draw extra cards as needed to reach 5 cards drawn.",
        NewRuleCardType.Play2 => "Play 2 cards per turn.\nIf you have fewer than that, play all your cards.",
        NewRuleCardType.Play3 => "Play 3 cards per turn.\nIf you have fewer than that, play all your cards.",
        NewRuleCardType.Play4 => "Play 4 cards per turn.\nIf you have fewer than that, play all your cards.",
        NewRuleCardType.PlayAllBut1 => "Play all but 1 of your cards. If you started with no cards in your hand and only drew 1, draw an extra card.",
        NewRuleCardType.PlayAll => "Play all your cards per turn.",
        NewRuleCardType.Inflation => "Any time a numeral is seen on another card, add one to that numeral. For example, 1 becomes 2, while one remains one. Yes, this affects the Basic Rules.",
        NewRuleCardType.DoubleAgenda => "A second Goal can now be played. After this, whoever plays a new Goal must choose which of the current Goals to discard. You win if you satisfy either Goal.",
        NewRuleCardType.RichBonus => "If one player has more Keepers in play than any other player, the number of cards played by this player on their turn is increased by 1.\nIn a tie, no one gets the bonus.",
        NewRuleCardType.PartyBonus => "If someone has the Party on the table, all players draw 1 extra card and play 1 extra card during their turns.",
        NewRuleCardType.PoorBonus => "If one player has fewer Keepers in play than any other player, the number of cards drawn by this player on their turn is increased by 1.\nIn a tie, no one gets the bonus.",
        NewRuleCardType.NoHandBonus => "If empty handed, draw 3 cards before observing the current draw rule.",
        NewRuleCardType.Recycling => "Once during your turn, you may discard one of your Keepers from the table and draw 3 extra cards.",
        NewRuleCardType.SwapPlaysForDraws => "During your turn, you may decide to play no more cards and instead draw as many cards as you have plays remaining. If Play All, draw as many cards as you hold.",
        NewRuleCardType.GoalMill => "Once during your turn, discard as many of your Goal cards as you choose then draw that many cards.",
        NewRuleCardType.MysteryPlay => "Once during your turn, you may take the top card from the draw pile and play it immediately.",
        _ => throw new NotImplementedException(),
    };

    public static string GetName(this RuleType type) => type switch
    {
        RuleType.StartOfTurn => "Start-of-Turn Event",
        RuleType.InstantEffect => "Takes Instant Effect",
        RuleType.FreeAction => "Free Action",
        RuleType.HandLimit => "Replaces Hand Limit",
        RuleType.KeeperLimit => "Replaces Keeper Limit",
        RuleType.Draw => "Replaces Draw Rule",
        RuleType.Play => "Replaces Play Rule",
        _ => throw new NotImplementedException(),
    };

}
