using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInteractable : Interactable
{
    public Action action;

    protected override void Interact()
    {
        action.Invoke();
    }
}
