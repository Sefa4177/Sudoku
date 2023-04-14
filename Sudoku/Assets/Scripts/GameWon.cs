using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour
{
    public GameObject WinPopUp;
    public Text ClockText;
    void Start()
    {
        WinPopUp.SetActive(false);
        ClockText.text = Clock.instance.GetCurrentTimeText().text;
    }

    private void OnBoardCompleted()
    {
        WinPopUp.SetActive(true);
        ClockText.text = Clock.instance.GetCurrentTimeText().text;
    }

    private void OnEnable()
    {
        GameEvents.OnBoardCompleted += OnBoardCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= OnBoardCompleted;
    }

    
}
