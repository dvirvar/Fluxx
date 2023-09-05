using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeeperCard : Card
{

    public override Type type => Type.Keeper;

    public void Init(KeeperCardInfo info)
    {
        base.Init(info);
    }    
}

public class KeeperCardInfo : CardInfo
{
    public readonly KeeperCardType KeeperType;

    public KeeperCardInfo(KeeperCardType keeperType): base(keeperType.GetName())
    {
        KeeperType = keeperType;
    }
}

public enum KeeperCardType
{
    TheMoon,
    Bread,
    Chocolate,
    Music,
    Time,
    TheToaster,
    TheBrain,
    Dreams,
    TheEye,
    Sleep,
    Milk,
    TheParty,
    Peace,
    Television,
    Cookies,
    TheSun,
    Love,
    Money,
    TheRocket
}