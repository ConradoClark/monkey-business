using UnityEngine;
using System.Collections;

public class FrostyPooledConnector : FrostyPoolableObject
{
    public override void Connect(FrostyPoolInstance instance, bool isChildrenConector)
    {
        if (isChildrenConector)
        {
            base.Connect(instance);
        }
    }
}
