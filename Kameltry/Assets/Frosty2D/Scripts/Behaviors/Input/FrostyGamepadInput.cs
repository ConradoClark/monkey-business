using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class FrostyGamepadInput : FrostyInput
{

    [Serializable]
    public class FrostyGamepadMap
    {
        public string Action;
        public string UnityAction;
        public bool isAxis;
    }

    public FrostyGamepadMap[] buttonMapping;

    public override bool GetActionClicked(string action)
    {
        return CheckForAction(action, Input.GetButtonDown, Input.GetAxisRaw);
    }

    public override bool GetActionHeld(string action)
    {
        return CheckForAction(action, Input.GetButton, Input.GetAxisRaw);
    }

    public override bool GetActionReleased(string action)
    {
        return CheckForAction(action, Input.GetButtonUp, (a) => 0f);
    }

    void OnGUI()
    {
        GUI.contentColor = Color.green;
        GUI.Label(new Rect(150, 350, 1000, 100), Input.inputString);
    }

    private bool CheckForAction(string action, Func<string, bool> func, Func<string, float> funcAxis)
    {
        var keys = buttonMapping.Where(k => k.Action == action).ToArray();
        if (keys.Length == 0) return false;

        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].isAxis && funcAxis(keys[i].UnityAction) == 1) return true;
            if (func(keys[i].UnityAction)) return true;
        }
        return false;
    }
}
