using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrostyInputActionFragment : MonoBehaviour {

    public FrostyInputManager inputManager;
    public enum InputFragmentType
    {
        NoInput,
        Click,
        Hold,
        Release
    }

    public string actionName;

    public virtual IEnumerator<bool> EvaluateInput()
    {
        yield return true;
        yield break;
    }

    protected virtual IEnumerator<bool> EvaluateNoInput()
    {
        yield return true;
        yield break;
    }

    protected virtual IEnumerator<bool> EvaluateClick()
    {
        yield return true;
        yield break;
    }

    protected virtual IEnumerator<bool> EvaluateHold()
    {
        yield return true;
        yield break;
    }

    protected virtual IEnumerator<bool> EvaluateRelease()
    {
        yield return true;
        yield break;
    }
}