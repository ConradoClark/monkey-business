using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class FrostyKeyboardInput : FrostyInput {

    [Serializable]
    public class FrostyKeyboardMap
    {
        public string Action;
        public KeyCode Key;
    }

    public FrostyKeyboardMap[] keyMapping;

    public override bool GetActionClicked(string action)
    {
        return CheckForAction(action, Input.GetKeyDown);
    }

    public override bool GetActionHeld(string action)
    {
        return CheckForAction(action, Input.GetKey);
    }

    public override bool GetActionReleased(string action)
    {
        return CheckForAction(action, Input.GetKeyUp);
    }

    private bool CheckForAction(string action, Func<KeyCode,bool> func)
    {
        var keys = keyMapping.Where(k => k.Action == action).ToArray();
        if (keys.Length == 0) return false;

        for (int i = 0; i < keys.Length; i++)
        {
            if (func(keys[i].Key)) return true;
        }
        return false;
    }
}
