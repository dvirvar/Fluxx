using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LimitUI : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public void SetText(string player)
    {
        text.text = $"{player} Choose which cards to keep";
    }
}
