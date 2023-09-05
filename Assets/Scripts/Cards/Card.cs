using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public enum Type
    {
        Action,
        Goal,
        Keeper,
        NewRule
    }


    [SerializeField] TMP_Text typeText;
    [SerializeField] TMP_Text nameText;

    public abstract Type type { get; }
    protected CardInfo cardInfo;

    public void Init(CardInfo info)
    {
        cardInfo = info;
        typeText.text = type.ToString();
        nameText.text = info.Name;
    }
}

public abstract class CardInfo
{
    public readonly string Name;

    public CardInfo(string name)
    {
        Name = name;
    }
}