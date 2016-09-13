using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FrostyInput : MonoBehaviour
{
    string[] handledActions;
        
    public virtual bool GetActionHeld(string action)
    {
        return false;
    }

    public virtual bool GetActionReleased(string action)
    {
        return false;
    }

    public virtual bool GetActionClicked(string action)
    {
        return false;
    }
}
