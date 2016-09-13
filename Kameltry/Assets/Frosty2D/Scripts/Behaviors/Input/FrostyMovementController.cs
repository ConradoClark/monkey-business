using UnityEngine;
using System.Linq;
using System.Collections;
using Assets.Frosty2D.Scripts.Core.Movement;
using System.Collections.Generic;

[AddComponentMenu("Frosty-Movement/Movement Controller")]
public class FrostyMovementController : MonoBehaviour
{
    public FrostyMovementControllerInput[] inputs;
    public float releaseTolerance=0f;
    private float currentReleaseTolerance = 0f;
    bool isEvaluating = false;
    bool isReleasing = false;
    IEnumerator<bool> enumerator;
    public TimeLayers timeLayer;

    void Update()
    {
        FrostyMovementControllerInput[] orderedInputs = inputs.OrderBy(i => i.priority).ToArray();
        for (int i =0; i< orderedInputs.Length;i++)
        {
            FrostyMovementControllerInput input = orderedInputs[i];

            if (input.action == null) return;

            if (!isEvaluating)
            {
                enumerator = input.action.EvaluateInput();
            }

            if (!enumerator.MoveNext())
            {
                isEvaluating = false;
                return;
            }

            if (!enumerator.Current && !isReleasing)
            {
                isReleasing = true;
                currentReleaseTolerance = releaseTolerance;
            }

            bool isHeld = enumerator.Current;
            bool isReleased = !enumerator.Current && currentReleaseTolerance <= releaseTolerance;

            if ((isHeld && !input.movement.IsActive()) && input.conditions.All(condition=>condition.Value))
            {
                if (input.toggle && !input.movement.HasFinished)
                {
                    input.movement.Deactivate();
                    continue;
                }

                if (input.exclusive)
                {
                    for (int j = 0; j < orderedInputs.Length; j++)
                    {
                        if (i != j)
                        {
                            orderedInputs[j].movement.Deactivate();
                        }
                    }
                }
                input.movement.Reactivate(input.keepSpeed);
                continue;
            }

            if (isReleased && !input.toggle && input.deactivateOnRelease)
            {
                input.movement.Deactivate();
            }

            if (isReleasing)
            {
                currentReleaseTolerance -= Toolbox.Instance.time.GetDeltaTime(timeLayer);
                if (currentReleaseTolerance <= 0) isReleasing = false;
            }
        }
    }
}
