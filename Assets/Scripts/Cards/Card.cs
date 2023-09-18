using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [SerializeField] Material cardMaterial, canBeSelectedMaterial;
    [SerializeField] MeshRenderer cardMeshRenderer;
    [SerializeField] TMP_Text nameText;

    public abstract CardType Type { get; }
    protected CardInfo Info { get; private set; }
    public bool CanBeSelected { get; private set; }

    protected void Init(CardInfo info)
    {
        Info = info;
        nameText.text = info.Name;
    }

    public void SetCanBeSelected(bool canBeSelected)
    {
        CanBeSelected = canBeSelected;
        cardMeshRenderer.material = canBeSelected ? canBeSelectedMaterial : cardMaterial;
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

public enum CardType
{
    Action,
    Goal,
    Keeper,
    NewRule
}