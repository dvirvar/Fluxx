using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewRuleCard : Card
{
    [SerializeField] TMP_Text ruleTypeText;
    [SerializeField] TMP_Text descriptionText;
    public override Type type => Type.Keeper;

    public void Init(NewRuleCardInfo info)
    {
        base.Init(info);
        ruleTypeText.text = info.NewRuleType.GetRuleType().GetName();
        descriptionText.text = info.NewRuleType.GetDescription();
    }
}
public class NewRuleCardInfo : CardInfo
{
    public readonly NewRuleCardType NewRuleType;

    public NewRuleCardInfo(NewRuleCardType newRuleType): base(newRuleType.GetName())
    {
        NewRuleType = newRuleType;
    }
}

public enum NewRuleCardType
{
    FirstPlayRandom,
    GetOnWithIt,
    HandLimit0,
    HandLimit1,
    HandLimit2,
    KeeperLimit2,
    KeeperLimit3,
    KeeperLimit4,
    Draw2,
    Draw3,
    Draw4,
    Draw5,
    Play2,
    Play3,
    Play4,
    PlayAllBut1,
    PlayAll,
    Inflation,
    DoubleAgenda,
    RichBonus,
    PartyBonus,
    PoorBonus,
    NoHandBonus,
    Recycling,
    SwapPlaysForDraws,
    GoalMill,
    MysteryPlay,
}

public enum RuleType
{
    StartOfTurn,
    InstantEffect,
    FreeAction,
    HandLimit,
    KeeperLimit,
    Draw,
    Play
}