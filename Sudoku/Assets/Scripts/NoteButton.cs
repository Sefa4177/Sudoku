using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class NoteButton : Selectable, IPointerClickHandler
{
    public Sprite on_image;
    public Sprite off_image;
    public Text ActiveInfo;
    

    private bool active_;
    
    void Start()
    {
        active_ =false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        active_ = !active_;

        if (active_)
        {
            ActiveInfo.text = "(Active)";
        }
        else
        {
             ActiveInfo.text = "(DeActive)";
        }

        GameEvents.OnNotesActiveMethod(active_);
    }
}
