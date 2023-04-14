using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Text textClock;
    void Start()
    {
        
    }
    void Update() 
    {
         textClock.text = Clock.instance.GetCurrentTimeText().text;
    }

}
