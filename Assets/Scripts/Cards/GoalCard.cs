using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalCard : Card
{

    [SerializeField] TMP_Text descriptionText;
    public override CardType Type => CardType.Goal;
    public GoalCardInfo GoalCardInfo => Info as GoalCardInfo;

    public void Init(GoalCardInfo info)
    {
        base.Init(info);
        descriptionText.text = info.GoalType.GetDescription();
    }
}

public class GoalCardInfo : CardInfo
{
    public readonly GoalCardType GoalType;

    public GoalCardInfo(GoalCardType goalType): base(goalType.GetName())
    {
        GoalType = goalType;
    }
}

public enum GoalType
{
    Keepers,
    NumberOfKeepersInPlay,
    NumberOfCardsInHand,
    KeeperAndAtLeastOneFood,
    KeeperAndNotInPlay
}

public enum GoalCardType
{
    BreadAndChocolate,
    GreatThemeSong,
    TheAppliances,
    SquishyChocolate,
    Toast,
    CantBuyMeLove,
    Lullaby,
    BedTime,
    TimeIsMoney,
    PartyTime,
    HeartsAndMinds,
    WorldPeace,
    TurnItUp,
    DayDreams,
    RocketScience,
    TheMindsEye,
    Dreamland,
    WinningTheLottery,
    ChocolateCookies,
    ChocolateMilk,
    Hippyism,
    TheEyeOfTheBeholder,
    MilkAndCookies,
    NightAndDay,
    BakedGoods,
    RocketToTheMoon,
    TenCardsInHand,
    TheBrain,
    FiveKeepers,
    PartySnacks
}

public readonly struct KeeperAndNotInPlay
{
    public readonly KeeperCardType KeeperInPlay;
    public readonly KeeperCardType KeeperNotInPlay;

    public KeeperAndNotInPlay(KeeperCardType keeper, KeeperCardType notInPlay)
    {
        KeeperInPlay = keeper;
        KeeperNotInPlay = notInPlay;
    }
}
