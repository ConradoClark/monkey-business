using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FrostyInputSequence : FrostyInputActionFragment
{
    public FrostyInputActionFragment[] actionFragments;
    private IEnumerator<bool> evaluation;

    public int currentMove { get; private set; }
    public TimeLayers timeLayer;

    string debugText;
    float debugDelay = 0f;

    public override IEnumerator<bool> EvaluateInput()
    {
        if (evaluation == null || !evaluation.MoveNext())
        {
            evaluation = Evaluate();
        }
        else
        {
            yield return evaluation.Current;
        }

        while (evaluation.MoveNext())
        {
            yield return evaluation.Current;
        }
        evaluation = null;
    }

    //void OnGUI()
    //{
    //    GUI.contentColor = new Color(0, 0.55f, 1);
    //    GUI.Label(new Rect(150, 335, 1000, 100), "Attack (Combo) Z -> Z -> Z");
    //    GUI.contentColor = Color.green;
    //    GUI.Label(new Rect(150, 350, 1000, 100), debugText);
    //}

    void Update()
    {
        if (debugDelay > 0f)
        {
            debugDelay -= Toolbox.Instance.time.GetDeltaTime(timeLayer);
        }
    }

    IEnumerator<bool> Evaluate()
    {
        currentMove = 0;
        if (actionFragments.Length == 0)
        {
            yield return true;
            yield break;
        }
        bool result = true;
        for (int i = 0; i < actionFragments.Length; i++)
        {
            currentMove = i;
            IEnumerator<bool> actionEvaluation = actionFragments[i].EvaluateInput();

            if (debugDelay <= 0 || i!=0)
            {
                if (i == 0)
                {
                    debugText = "Waiting for initial input... ";
                }
                else
                {
                    debugText = "";
                    for (int j = 0; j < i; j++)
                    {
                        debugText += actionFragments[j].actionName + " pressed! -> ";
                    }
                    debugText += "Waiting for next input... ";
                }
            }

            while (actionEvaluation.MoveNext())
            {
                yield return false;
            }

            result = actionEvaluation.Current;
            if (!result)
            {
                yield return false;
                yield break;
            }
        }

        if (debugDelay <= 0)
        {
            debugText = "";
            for (int j = 0; j < actionFragments.Length; j++)
            {
                debugText += actionFragments[j].actionName + " pressed! -> ";
            }
        }

        debugText += "Sequence complete! ";
        debugDelay = 1f;
        currentMove = actionFragments.Length;
        yield return true;
    }
}
