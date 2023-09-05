using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardPrefabInfo", menuName = "Card/PrefabInfo")]
public class CardPrefabInfosSO : ScriptableObject
{
    public List<CardPrefabInfo> prefabInfos;
}
[Serializable]
public struct CardPrefabInfo
{
    public Card.Type type;
    public GameObject prefab;
}