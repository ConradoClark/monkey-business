using UnityEngine;
using System.Collections;

public class FrostyPoolableObject : MonoBehaviour
{
    public FrostyPoolInstance poolInstance;
    public virtual void ResetState()
    {

    }

    public virtual void Connect(FrostyPoolInstance instance, bool isPostConector = false)
    {
        if (!isPostConector)
        {
            this.poolInstance = instance;
        }
    }
}
