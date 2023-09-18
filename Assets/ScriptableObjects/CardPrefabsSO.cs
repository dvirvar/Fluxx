using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardPrefabsSO", menuName = "Card/Prefabs")]
public class CardPrefabsSO : ScriptableObject
{
    [field: SerializeField] public ActionCard ActionCardPrefab { get; private set; }
    [field: SerializeField] public GoalCard GoalCardPrefab { get; private set; }
    [field: SerializeField] public KeeperCard KeeperCardPrefab { get; private set; }
    [field: SerializeField] public NewRuleCard NewRuleCardPrefab { get; private set; }
}