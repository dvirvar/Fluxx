using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionCard : Card
{
    [SerializeField] TMP_Text descriptionText;
    public override CardType Type => CardType.Action;
    public ActionCardInfo ActionCardInfo => Info as ActionCardInfo;
    
    public void Init(ActionCardInfo info)
    {
        base.Init(info);
        descriptionText.text = info.ActionType.GetDescription();
    }
}

public class ActionCardInfo: CardInfo
{
    public readonly ActionCardType ActionType;

    public ActionCardInfo(ActionCardType actionType): base(actionType.GetName())
    {
        ActionType = actionType;
    }
}

public enum ActionCardType
{
    Draw3AndPlay2,
    EverybodyGets1,
    RotateHands,
    StealAKeeper,
    LetsSimplify,
    Jackpot,
    TrashAKeeper,
    TodaysSpecial,
    Draw2AndUseThem,
    TakeAnotherTurn,
    RulesReset,
    NoLimits,
    TrashANewRule,
    ZapACard,
    DiscardAndDraw,
    LetsDoThatAgain,
    UseWhatYouTake,
    EmptyTheTrash,
    RandomTax,
    TradeHands,
    RockPaperScissorsShowdown,
    ExchangeKeepers,
    ShareTheWealth
}