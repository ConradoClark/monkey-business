using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class FrostyInputCombination : FrostyInputActionFragment
{
    public FrostyInputActionFragment[] actionFragments;
    public bool AnyOfThem;

    public override IEnumerator<bool> EvaluateInput()
    {
        IEnumerator<bool>[] inputEvaluation = actionFragments.Select(af => af.EvaluateInput()).ToArray();
        bool[] results = new bool[inputEvaluation.Length];
        bool[] hasResults = new bool[inputEvaluation.Length];

        evaluate:
        for (int i = 0; i < inputEvaluation.Length; i++)
        {
            hasResults[i] = inputEvaluation[i].MoveNext();
            if (hasResults[i])
            {
                results[i] = inputEvaluation[i].Current;
            }
        }

        if (hasResults.All(h => !h) && !AnyOfThem)
        {
            yield return results.Aggregate(false, (a, b) => a & b);
        }
        else if (hasResults.Any(h=>!h) && results.Any(h=>h) && AnyOfThem)
        {
            yield return true;
        }
        else if (hasResults.All(h => !h) && results.All(h => !h))
        {
            yield return false;
            yield break;
        }
        else
        {
            yield return false;
            goto evaluate;
        }
    }
}
