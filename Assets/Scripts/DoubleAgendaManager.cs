using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoubleAgendaManager : MonoBehaviour
{
    //If first button pressed returns false
    //If second button pressed returns true
    public event Action<bool> ButtonPressed = delegate { };
    [SerializeField] TMP_Text titleText;
    [SerializeField] Button firstButton, secondButton;

    public void ChooseDoubleAgenda(string first, string second)
    {
        titleText.text = "Which goal would you like to replace?";
        firstButton.GetComponentInChildren<TMP_Text>().text = first;
        secondButton.GetComponentInChildren<TMP_Text>().text = second;
        gameObject.SetActive(true);
    }

    public void RemoveDoubleAgenda(string first, string second)
    {
        titleText.text = "Which goal would you like to keep?";
        firstButton.GetComponentInChildren<TMP_Text>().text = first;
        secondButton.GetComponentInChildren<TMP_Text>().text = second;
        gameObject.SetActive(true);
    }

    void OnEnable()
    {
        firstButton.onClick.AddListener(() => { 
            ButtonPressed.Invoke(false);
            gameObject.SetActive(false);
        });
        secondButton.onClick.AddListener(() => { 
            ButtonPressed.Invoke(true);
            gameObject.SetActive(false);
        });
    }

    void OnDisable()
    {
        firstButton.onClick.RemoveAllListeners();
        secondButton.onClick.RemoveAllListeners();
    }
}
