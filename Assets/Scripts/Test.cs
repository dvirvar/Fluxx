using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Transform deck;
    [SerializeField] CardPrefabInfosSO cardPrefabInfoSO;
    private void Start()
    {
        var actionCardPrefab = cardPrefabInfoSO.prefabInfos.Find(p => p.type == Card.Type.Action).prefab;
        var index = 0;
        foreach (var item in DeckData.ActionCards)
        {
            var actionCard = Instantiate(actionCardPrefab, deck);
            actionCard.GetComponent<ActionCard>().Init(item);
            var position = deck.transform.position;
            actionCard.transform.rotation = deck.transform.rotation;
            actionCard.transform.position = new Vector3(position.x, position.y + index * 0.035f, position.z);
            index++;
        }
    }
}
